﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ROC.WebApi.Core.Cache;
using ROC.WebApi.Core.Exceptions;
using ROC.WebApi.Shared.Authorization;

namespace ROC.Core.Infrastructure.Identity.Users.Services;

internal sealed partial class UserService
{
    public async Task<bool> HasPermissionAsync(string userId, string permission,
        CancellationToken cancellationToken = default)
    {
        var permissions = await cache.GetOrSetAsync(
            GetPermissionCacheKey(userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new UnauthorizedException();

        var userRoles = await userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (var role in await roleManager.Roles
                     .Where(r => userRoles.Contains(r.Name!))
                     .ToListAsync(cancellationToken))
            permissions.AddRange(await db.RoleClaims
                .Where(rc => rc.RoleId == role.Id && rc.ClaimType == RocClaims.Permission)
                .Select(rc => rc.ClaimValue!)
                .ToListAsync(cancellationToken));

        return permissions.Distinct().ToList();
    }

    public static string GetPermissionCacheKey(string userId)
    {
        return $"perm:{userId}";
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(GetPermissionCacheKey(userId), cancellationToken);
    }
}