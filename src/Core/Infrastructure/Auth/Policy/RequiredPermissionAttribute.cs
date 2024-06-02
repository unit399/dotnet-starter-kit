﻿using System;
using System.Collections.Generic;

namespace ROC.Core.Infrastructure.Auth.Policy;

public interface IRequiredPermissionMetadata
{
    HashSet<string> RequiredPermissions { get; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequiredPermissionAttribute(
    string? requiredPermission,
    params string[]? additionalRequiredPermissions)
    : Attribute, IRequiredPermissionMetadata
{
    public string? RequiredPermission { get; }
    public string[]? AdditionalRequiredPermissions { get; }
    public HashSet<string> RequiredPermissions { get; } = [requiredPermission!, .. additionalRequiredPermissions];
}