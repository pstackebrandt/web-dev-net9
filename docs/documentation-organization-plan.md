# Documentation Organization Plan

## Overview

This document outlines the plan for organizing all documentation files according to the structure defined in `docs/README.md`. The goal is to ensure consistent documentation organization across the solution.

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
ProjectName/
└── docs/
    ├── README.md              # Project overview
    ├── features/              # Feature documentation
    ├── configuration.md       # Configuration details
    └── testing.md             # Testing approach
```

## Organization Plan

### Root Level Documents

- `refactor-northwind-context-plan.md` → Move to `docs/general/development/refactor-plans/`

### Solution-Wide Documentation

All solution-wide documentation will be organized within the `docs/` folder structure:

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

### Project-Specific Documentation

All project-specific documentation will be organized within their respective project folders:

1. For each project (e.g., `code/MatureWeb/`):
   - Project README → `code/[ProjectName]/README.md`
   - Feature documentation → `code/[ProjectName]/docs/features/`
   - Configuration documentation → `code/[ProjectName]/docs/configuration.md`
   - Testing documentation → `code/[ProjectName]/docs/testing.md`

2. Specific examples:
   - `code/MatureWeb/docs/user-secrets-setup.md` → Keep in place as project-specific configuration
   - `code/MatureWeb/docs/test-best-practices.md` → Keep in place as project-specific testing documentation

## Implementation Steps

1. Create any missing directories in the structure
2. Move root-level documentation files to appropriate locations
3. Reorganize existing documentation in the `docs/` folder
4. Review and update project-specific documentation
5. Update all internal documentation links to reflect new file locations
6. Update any references to documentation in code comments

## Timeline

- Phase 1: Directory structure setup (1 day)
- Phase 2: Document migration (2-3 days)
- Phase 3: Link updates and verification (1-2 days)

## Maintenance Guidelines

- Follow the patterns established in this document for all new documentation
- When adding new documentation, place it in the appropriate location
- Update this plan if new categories of documentation emerge