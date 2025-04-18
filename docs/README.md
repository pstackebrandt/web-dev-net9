# Documentation

This folder contains the general documentation for the web-dev-net9 solution.

## Table of Contents

* [Documentation](#documentation)
  * [Table of Contents](#table-of-contents)
  * [Structure](#structure)
  * [Documentation Organization Principles](#documentation-organization-principles)
  * [Project-Specific Documentation](#project-specific-documentation)
  * [Documentation Guidelines](#documentation-guidelines)

## Structure

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
│   │   └── ...                 # Other development guidelines
│   ├── operations/             # Operational documentation
│   └── guidelines/             # General guidelines
├── api/                        # API documentation
│   └── endpoints/              # Detailed endpoint documentation
└── assets/                     # Documentation assets
    ├── images/                 # Images used in documentation
    └── templates/              # Documentation templates
```

## Documentation Organization Principles

* **Solution-wide documentation** belongs in this root `docs/` folder
* **Project-specific documentation** should be placed in the respective project folders
* Use Markdown for all documentation files
* Follow the formatting rules in `.markdownlint.json`
* Keep documentation up-to-date with code changes
* Use relative links to reference other documentation files

## Project-Specific Documentation

Project-specific documentation should follow this structure within project folders:

```
code/MatureWeb/[ProjectName]/
└── docs/
    ├── README.md              # Project overview
    ├── features/              # Feature documentation
    └── implementation/        # Implementation details
```

## Documentation Guidelines

1. **Keep documentation where it belongs**
   * Solution-wide documentation should ALWAYS be placed in the root `docs/` folder
   * Project-specific documentation should ALWAYS be placed in the relevant project folder

2. **Handle cross-project documentation appropriately**
   * If a document applies to multiple projects but not the entire solution, consider whether it
     should be generalized for the entire solution or duplicated/referenced in each relevant project

3. **Maintain clear organization**
   * Follow the established folder structure
   * Don't create duplicate documentation hierarchies
   * Use consistent naming conventions

For more detailed guidance, see [Documentation Guidelines](./general/guidelines/documentation.md).