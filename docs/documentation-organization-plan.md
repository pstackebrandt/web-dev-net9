# Documentation Organization Plan

## Overview

This document outlines the completed documentation organization for the web-dev-net9 solution.

## Documentation Structure

The primary structure for documentation is:

```
docs/
├── README.md                   # Documentation overview
├── general/                    # Solution-wide documentation
│   ├── architecture/           # System architecture documents
│   │   ├── decisions/          # Architectural decision records (ADRs)
│   │   └── diagrams/           # Architecture diagrams
│   ├── development/            # Development guidelines
│   │   ├── refactor-plans/     # Plans for code refactoring
│   │   ├── testing/            # Testing documentation
│   │   ├── configuration-best-practices.md
│   │   └── user-secrets-setup.md
│   ├── operations/             # Operational documentation
│   └── guidelines/             # General guidelines
├── api/                        # API documentation
│   └── endpoints/              # Detailed endpoint documentation
└── assets/                     # Documentation assets
    ├── images/                 # Images used in documentation
    └── templates/              # Documentation templates
```

Additionally, project-specific documentation should follow this structure within project folders:

```
code/MatureWeb/[ProjectName]/
└── docs/
    ├── README.md              # Project overview
    ├── features/              # Feature documentation
    └── implementation/        # Implementation details
```

## COMPLETED Organization Plan

> **NOTE:** The documentation consolidation has been completed according to the Documentation Consolidation Plan, resolving the redundancy between `docs/` and `code/MatureWeb/docs/`.

### Root Level Documents

- ✅ `refactor-northwind-context-plan.md` → Moved to `docs/general/development/refactor-plans/`

### Solution-Wide Documentation

All solution-wide documentation is now organized within the `docs/` folder structure:

1. General documentation:
   - Architecture documentation → `docs/general/architecture/`
   - Development guidelines → `docs/general/development/`
   - Operational documentation → `docs/general/operations/`
   - General guidelines → `docs/general/guidelines/`

2. API documentation:
   - All API reference materials → `docs/api/`
   - Endpoint specifications → `docs/api/endpoints/`

3. Documentation assets:
   - Images used in documentation → `docs/assets/images/`
   - Templates for documentation → `docs/assets/templates/`

### Documentation Previously in MatureWeb Solution Folder

The following documentation previously in the MatureWeb solution folder has been moved to the root docs directory:

1. Configuration documentation:
   - ✅ Previously at: `code/MatureWeb/docs/configuration/configuration-best-practices.md`
     - Now at: `docs/general/development/configuration-best-practices.md`
   - ✅ Previously at: `code/MatureWeb/docs/configuration/user-secrets-setup.md`
     - Now at: `docs/general/development/user-secrets-setup.md`

2. Testing documentation:
   - ✅ Previously at: `code/MatureWeb/docs/testing/test-best-practices.md`
     - Now at: `docs/general/development/testing/test-best-practices.md`
   - ✅ Previously at: `code/MatureWeb/docs/testing/test-configuration-guide.md`
     - Now at: `docs/general/development/testing/test-configuration-guide.md`
   - ✅ Previously at: `code/MatureWeb/docs/testing/test-environment-setup.md`
     - Now at: `docs/general/development/testing/test-environment-setup.md`
   - ✅ Previously at: `code/MatureWeb/docs/testing/test-naming-conventions.md`
     - Now at: `docs/general/development/testing/test-naming-conventions.md`
   - ✅ Previously at: `code/MatureWeb/docs/testing/TestStructure.md`
     - Now at: `docs/general/development/testing/TestStructure.md`

### Project-Specific Documentation

Project-specific documentation belongs ONLY in individual project folders:

```
code/MatureWeb/[ProjectName]/docs/
```

This documentation should focus ONLY on concerns specific to that particular project.

## Implementation Steps

1. ✅ Create any missing directories in the structure
2. ✅ Move root-level documentation files to appropriate locations
3. ✅ Reorganize existing documentation in the `docs/` folder
4. ✅ Consolidate documentation from MatureWeb solution folder to root `docs/` folder
5. ✅ Update all internal documentation links to reflect new file locations
6. ✅ Update any references to documentation in code comments

## Timeline

- Phase 1: ✅ Directory structure setup (Completed)
- Phase 2: ✅ Initial document migration (Completed)
  - Root-level documentation migration ✅
  - Project-specific documentation reorganization ✅
- Phase 3: ✅ Documentation consolidation (Completed)
  - Move solution-level documentation from MatureWeb solution folder to `docs/` ✅
  - Update references to relocated documentation ✅

## Maintenance Guidelines

- Solution-wide documentation should ALWAYS be placed in the root `docs/` folder
- Project-specific documentation should ALWAYS be placed in the relevant project folder
- If a document applies to multiple projects but not the entire solution, consider whether it should be generalized for the entire solution or duplicated/referenced in each relevant project