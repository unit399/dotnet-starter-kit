using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ROC.Core.Infrastructure.Constants;
using ROC.Core.Infrastructure.Identity.Persistence;
using ROC.Core.Infrastructure.Identity.Roles;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Cache;
using ROC.WebApi.Core.Exceptions;
using ROC.WebApi.Core.Identity.Handlers.LoginUser;
using ROC.WebApi.Core.Identity.Handlers.RegisterUser;
using ROC.WebApi.Core.Identity.Handlers.ToggleUserStatus;
using ROC.WebApi.Core.Identity.Handlers.UpdateUser;
using ROC.WebApi.Core.Identity.Users.Entities;
using ROC.WebApi.Core.Identity.Users.Interfaces;
using ROC.WebApi.Core.Jobs;
using ROC.WebApi.Core.Mail;
using ROC.WebApi.Core.Tenant;

namespace ROC.Core.Infrastructure.Identity.Users.Services;

internal sealed partial class UserService(
    UserManager<RocUser> userManager,
    SignInManager<RocUser> signInManager,
    RoleManager<RocRole> roleManager,
    IdentityDbContext db,
    ICacheService cache,
    IJobService jobService,
    IMailService mailService,
    IMultiTenantContextAccessor<RocTenantInfo> multiTenantContextAccessor
) : IUserService
{
    public Task<string> ConfirmEmailAsync(string userId, string code, string tenant,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.FindByEmailAsync(email.Normalize()) is RocUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        EnsureValidTenant();
        return await userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        EnsureValidTenant();
        return await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is RocUser user &&
               user.Id != exceptId;
    }

    public async Task<UserDetail> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("user not found");

        return user.Adapt<UserDetail>();
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return userManager.Users.AsNoTracking().CountAsync(cancellationToken);
    }

    public async Task<List<UserDetail>> GetListAsync(CancellationToken cancellationToken)
    {
        var users = await userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users.Adapt<List<UserDetail>>();
    }

    public Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserCommand request, string origin,
        CancellationToken cancellationToken)
    {
        // create user entity
        var user = new RocUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true
        };

        // register user
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new BaseException("error while registering a new user", errors);
        }

        // add basic role
        await userManager.AddToRoleAsync(user, IdentityConstants.Roles.Basic);

        // send confirmation mail
        if (!string.IsNullOrEmpty(user.Email))
        {
            var emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            var mailRequest = new MailRequest(
                new Collection<string> { user.Email },
                "Confirm Registration",
                emailVerificationUri);
            jobService.Enqueue(() => mailService.SendAsync(mailRequest, CancellationToken.None));
        }

        return new RegisterUserResponse(user.Id);
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserCommand request, string origin,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) throw new BaseException("Invalid email or password");
        
        var result = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!result.Succeeded) throw new BaseException("Invalid email or password");

        return new LoginUserResponse(user.Id);
    }
    
    public async Task ToggleStatusAsync(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        var isAdmin = await userManager.IsInRoleAsync(user, IdentityConstants.Roles.Admin);
        if (isAdmin) throw new BaseException("Administrators Profile's Status cannot be toggled");

        user.IsActive = request.ActivateUser;

        await userManager.UpdateAsync(user);
    }

    public async Task UpdateAsync(UpdateUserCommand request, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException("User Not Found.");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        var phoneNumber = await userManager.GetPhoneNumberAsync(user);
        if (request.PhoneNumber != phoneNumber) await userManager.SetPhoneNumberAsync(user, request.PhoneNumber);

        var result = await userManager.UpdateAsync(user);
        await signInManager.RefreshSignInAsync(user);

        if (!result.Succeeded) throw new BaseException("Update profile failed");
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id))
            throw new UnauthorizedException("invalid tenant");
    }

    private async Task<string> GetEmailVerificationUriAsync(RocUser user, string origin)
    {
        EnsureValidTenant();

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri,
            TenantConstants.Identifier,
            multiTenantContextAccessor?.MultiTenantContext?.TenantInfo?.Id!);
        return verificationUri;
    }
}