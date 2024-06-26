﻿using System;
using System.Security.Claims;

namespace ROC.Core.Infrastructure.Identity.Users;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email);
    }

    public static string? GetTenant(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(IdentityConstants.Claims.Tenant);
    }

    public static string? GetFullName(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(IdentityConstants.Claims.Fullname)?.Value;
    }

    public static string? GetFirstName(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static string? GetSurname(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(ClaimTypes.Surname)?.Value;
    }

    public static string? GetPhoneNumber(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.MobilePhone);
    }

    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static Uri? GetImageUrl(this ClaimsPrincipal principal)
    {
        var imageUrl = principal.FindFirstValue(IdentityConstants.Claims.ImageUrl);
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ? uri : null;
    }

    public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal)
    {
        return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(
            principal.FindFirstValue(IdentityConstants.Claims.Expiration)));
    }

    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
    {
        return principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;
    }
}