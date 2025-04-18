# Removal Plan: AddNorthwindContext Legacy Overload

This document outlines the plan for completely removing the deprecated overload of `AddNorthwindContext` method that doesn't accept an `IConfiguration` parameter.

## Current Status

The legacy method overload has been marked as deprecated with the `[Obsolete]` attribute:

```csharp
[Obsolete("This method builds a service provider during configuration, which is an anti-pattern. Use the overload that accepts IConfiguration instead: AddNorthwindContext(IServiceCollection, IConfiguration, string?).")]
public static IServiceCollection AddNorthwindContext(
    this IServiceCollection services,
    string? connectionString = null)
{
    // Implementation details...
}
```

This generates compiler warnings when the deprecated method is used, directing developers to use the new overload instead.

## Removal Timeline

The deprecated method will be removed according to the following timeline:

1. **Phase 1: Deprecation (Current)**
   - Method marked with `[Obsolete]` attribute
   - Documentation updated to recommend the new overload
   - All internal usage migrated to the new overload

2. **Phase 2: Breaking Change Warning (Next Minor Version)**
   - Update `[Obsolete]` attribute to trigger errors instead of warnings:
     ```csharp
     [Obsolete("This method will be removed in the next major version. Use the overload that accepts IConfiguration instead: AddNorthwindContext(IServiceCollection, IConfiguration, string?).", true)]
     ```
   - Add prominent notices in release notes
   - Add migration guide in documentation

3. **Phase 3: Removal (Next Major Version)**
   - Completely remove the deprecated method
   - Update all documentation to remove references to the old method
   - Provide migration script for users still on older versions

## Migration Assistance

To assist users in migrating away from the deprecated method, we will:

1. Provide clear documentation in the `NorthwindContext-usage-guide.md`
2. Create a migration script that can automatically update common usage patterns
3. Include examples of before/after code in release notes
4. Add a troubleshooting section to documentation addressing common migration issues

## Migration Script

A simple PowerShell script will be provided to assist with migration:

```powershell
# Find all calls to AddNorthwindContext without IConfiguration
Get-ChildItem -Path . -Filter *.cs -Recurse | 
    Select-String -Pattern "\.AddNorthwindContext\(\)" -SimpleMatch | 
    ForEach-Object {
        Write-Host "Potential migration needed in: $($_.Path) at line $($_.LineNumber)"
    }
```

## Breaking Changes

Removing the method will be a breaking change that affects:

1. Code that calls `AddNorthwindContext()` without passing `IConfiguration`
2. Code that calls `AddNorthwindContext(connectionString)` with only a connection string

## Verification Process

Before completing Phase 3 (removal), we will:

1. Use compiler warnings from Phase 2 to identify any remaining usage
2. Run automated tests with the deprecated method completely commented out
3. Perform static code analysis to find any references to the deprecated method
4. Create a pre-release version for early adopters to test

## Contingency Plan

If significant issues are discovered during the verification process:

1. Consider extending the deprecation period
2. Provide additional migration tools or guidance
3. Add temporary compatibility shims if necessary

## Communication Plan

To ensure users are aware of the upcoming changes:

1. Include clear notices in all release notes
2. Add banners to documentation
3. Notify users through community forums and newsletters
4. Monitor GitHub issues for migration-related questions

## Conclusion

By following this plan, we can safely remove the deprecated method while minimizing disruption to users. The clear timeline and migration assistance will help ensure a smooth transition to the improved implementation. 