using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ROC.Core.Infrastructure.Identity.RoleClaims;
using ROC.Core.Infrastructure.Identity.Roles;
using ROC.Core.Infrastructure.Identity.Users;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Core.Tenant;
using ROC.WebApi.Shared.Authorization;

namespace ROC.Core.Infrastructure.Identity.Persistence;

internal sealed class IdentityDbInitializer(
    ILogger<IdentityDbInitializer> logger,
    IdentityDbContext context,
    RoleManager<RocRole> roleManager,
    UserManager<RocUser> userManager,
    TimeProvider timeProvider,
    IMultiTenantContextAccessor<RocTenantInfo> multiTenantContextAccessor) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for identity module",
                context.TenantInfo?.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (var roleName in IdentityConstants.Roles.DefaultRoles)
        {
            if (await roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not RocRole role)
            {
                // create role
                role = new RocRole(roleName,
                    $"{roleName} Role for {multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id} Tenant");
                await roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == IdentityConstants.Roles.Basic)
            {
                await AssignPermissionsToRoleAsync(context, RocPermissions.Basic, role);
            }
            else if (roleName == IdentityConstants.Roles.Admin)
            {
                await AssignPermissionsToRoleAsync(context, RocPermissions.Admin, role);

                if (multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id == TenantConstants.Root.Id)
                    await AssignPermissionsToRoleAsync(context, RocPermissions.Root, role);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(IdentityDbContext dbContext,
        IReadOnlyList<RocPermission> permissions, RocRole role)
    {
        var currentClaims = await roleManager.GetClaimsAsync(role);
        var newClaims = permissions
            .Where(permission =>
                !currentClaims.Any(c => c.Type == IdentityConstants.Claims.Permission && c.Value == permission.Name))
            .Select(permission => new RocRoleClaim
            {
                RoleId = role.Id,
                ClaimType = IdentityConstants.Claims.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "application",
                CreatedOn = timeProvider.GetUtcNow()
            })
            .ToList();

        foreach (var claim in newClaims)
        {
            logger.LogInformation("Seeding {Role} Permission '{Permission}' for '{TenantId}' Tenant.", role.Name,
                claim.ClaimValue, multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            await dbContext.RoleClaims.AddAsync(claim);
        }

        // Save changes to the database context
        if (newClaims.Count != 0) await dbContext.SaveChangesAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        if (string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id) ||
            string.IsNullOrWhiteSpace(multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail)) return;

        if (await userManager.Users.FirstOrDefaultAsync(u =>
                u.Email == multiTenantContextAccessor.MultiTenantContext.TenantInfo!.AdminEmail)
            is not RocUser adminUser)
        {
            var adminUserName =
                $"{multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim()}.{IdentityConstants.Roles.Admin}"
                    .ToUpperInvariant();
            adminUser = new RocUser
            {
                FirstName = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id.Trim().ToUpperInvariant(),
                LastName = IdentityConstants.Roles.Admin,
                Email = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail =
                    multiTenantContextAccessor.MultiTenantContext.TenantInfo?.AdminEmail!.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true
            };

            logger.LogInformation("Seeding Default Admin User for '{TenantId}' Tenant.",
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            var password = new PasswordHasher<RocUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, IdentityConstants.DefaultPassword);
            await userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await userManager.IsInRoleAsync(adminUser, IdentityConstants.Roles.Admin))
        {
            logger.LogInformation("Assigning Admin Role to Admin User for '{TenantId}' Tenant.",
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id);
            await userManager.AddToRoleAsync(adminUser, IdentityConstants.Roles.Admin);
        }
    }
}