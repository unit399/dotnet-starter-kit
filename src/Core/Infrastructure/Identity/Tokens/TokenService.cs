using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ROC.Core.Infrastructure.Auth.Jwt;
using ROC.Core.Infrastructure.Identity.Users;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Auth.Jwt;
using ROC.WebApi.Core.Exceptions;
using ROC.WebApi.Core.Identity.Tokens;
using ROC.WebApi.Core.Identity.Tokens.Features.Generate;

namespace ROC.Core.Infrastructure.Identity.Tokens;

public sealed class TokenService(
    UserManager<RocUser> userManager,
    IMultiTenantContextAccessor<RocTenantInfo>? multiTenantContextAccessor,
    IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private readonly JwtOptions jwt = jwtOptions.Value;

    public async Task<TokenGenerationResponse> GenerateTokenAsync(
        TokenGenerationCommand request,
        string ipAddress,
        CancellationToken cancellationToken)
    {
        var currentTenant = multiTenantContextAccessor!.MultiTenantContext.TenantInfo;
        if (currentTenant == null) throw new UnauthorizedException();
        if (string.IsNullOrWhiteSpace(currentTenant.Id)
            || await userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedException();

        if (!user.IsActive) throw new UnauthorizedException("user is deactivated");

        if (currentTenant.Id != IdentityConstants.RootTenant)
        {
            if (!currentTenant.IsActive) throw new UnauthorizedException($"tenant {currentTenant.Id} is deactivated");

            if (DateTime.UtcNow > currentTenant.ValidUpto)
                throw new UnauthorizedException($"tenant {currentTenant.Id} validity has expired");
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    private async Task<TokenGenerationResponse> GenerateTokensAndUpdateUser(RocUser user, string ipAddress)
    {
        var token = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwt.RefreshTokenExpirationInDays);

        await userManager.UpdateAsync(user);

        return new TokenGenerationResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(RocUser user, string ipAddress)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(jwt.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.TokenExpirationInMinutes),
            signingCredentials: signingCredentials,
            issuer: JwtAuthConstants.Issuer,
            audience: JwtAuthConstants.Audience
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private List<Claim> GetClaims(RocUser user, string ipAddress)
    {
        return new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(IdentityConstants.Claims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(IdentityConstants.Claims.IpAddress, ipAddress),
            new(IdentityConstants.Claims.Tenant, multiTenantContextAccessor!.MultiTenantContext.TenantInfo!.Id),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}