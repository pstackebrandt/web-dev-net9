# Markdown Files Migration Plan

This document outlines the specific actions needed to organize all existing Markdown files according to the structure defined in the Documentation Organization Plan.

## Inventory of Markdown Files

The following Markdown files were found in the repository:

### Project-Specific Documentation (Keep in Place)

1. Project READMEs (keep in place):
   - `code/MatureWeb/README.md`
   - `code/MatureWeb/Northwind.Mvc/README.md`

2. Project-specific test documentation (keep in place):
   - `code/MatureWeb/docs/test-best-practices.md`
   - `code/MatureWeb/docs/test-configuration-guide.md`
   - `code/MatureWeb/docs/test-environment-setup.md`
   - `code/MatureWeb/docs/test-naming-conventions.md`
   - `code/MatureWeb/docs/TestStructure.md`

3. Project-specific configuration documentation (keep in place):
   - `code/MatureWeb/docs/user-secrets-setup.md`
   - `code/MatureWeb/docs/configuration-best-practices.md`

### Third-Party Documentation (Keep in Place)

- `code/MatureWeb/Northwind.Mvc/wwwroot/lib/jquery-validation/LICENSE.md`

### Solution-Wide Documentation

1. Documentation already in the correct location:
   - `docs/README.md`
   - `docs/assets/templates/project-docs-template.md`
   - `docs/general/architecture/decisions/template.md`
   - `docs/general/architecture/overview.md`
   - `docs/general/development/coding-standards.md`
   - `docs/general/guidelines/documentation.md`
   - `docs/documentation-organization-plan.md` (this migration plan)

2. Documentation that needs to be moved:
   - `refactor-northwind-context-plan.md` → `docs/general/development/refactor-plans/refactor-northwind-context-plan.md`

## Migration Actions

### 1. Create Missing Directories

```powershell
# Create missing directory for refactor plans
New-Item -ItemType Directory -Path "docs/general/development/refactor-plans" -Force
```

### 2. Move Files

```powershell
# Move refactor plan to the appropriate location
Move-Item -Path "refactor-northwind-context-plan.md" -Destination "docs/general/development/refactor-plans/"
```

### 3. Verify Project-Specific Documentation Structure

For each project with documentation, ensure the structure follows the recommended pattern:

```
ProjectName/docs/
├── README.md              # Project overview
├── features/              # Feature documentation
├── configuration.md       # Configuration details
└── testing.md             # Testing approach
```

For the MatureWeb project, we should reorganize test documentation:

```powershell
# Create features directory if it doesn't exist
New-Item -ItemType Directory -Path "code/MatureWeb/docs/features" -Force

# Recommendation: Consider consolidating test documentation into a single testing.md file
# or organize in a testing/ directory if multiple files are necessary
```

### 4. Update Links

After moving files, search for and update any references to the moved files:

```powershell
# Search for references to the moved refactor plan
Get-ChildItem -Path . -Recurse -Include "*.cs","*.md","*.html","*.cshtml" -File | 
Select-String -Pattern "refactor-northwind-context-plan.md" -List | 
Select-Object Path
```

## Follow-up Actions

1. Review all project-specific documentation to ensure it follows the established pattern
2. Consider consolidating related documentation files where appropriate
3. Update the main README.md if necessary to reflect the new documentation organization
4. Ensure all documentation follows the formatting rules in `.markdownlint.json`

## Timeline

- Execute directory creation: Day 1
- Move files to new locations: Day 1
- Update links and references: Day 2
- Verify and test documentation organization: Day 3