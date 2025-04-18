# Documentation Guidelines

## Table of Contents

- [Documentation Guidelines](#documentation-guidelines)
  - [Table of Contents](#table-of-contents)
  - [Principles](#principles)
  - [Documentation Organization](#documentation-organization)
    - [Solution-Wide Documentation](#solution-wide-documentation)
    - [Project-Specific Documentation](#project-specific-documentation)
  - [Key Guidelines for Documentation Organization](#key-guidelines-for-documentation-organization)
  - [Markdown Style Guide](#markdown-style-guide)
  - [Documentation Structure](#documentation-structure)
  - [Maintaining Documentation](#maintaining-documentation)
  - [Working with Documentation Files](#working-with-documentation-files)

## Principles

- Documentation should be clear, concise, and accurate
- Keep documentation close to what it documents
- Update documentation when you update code
- Use Markdown for all documentation
- Follow the formatting rules in `.markdownlint.json`

## Documentation Organization

### Solution-Wide Documentation

Solution-wide documentation belongs in the `docs` folder at the root of the repository:

```
docs/
├── general/                    # General solution documentation
│   ├── architecture/           # System architecture
│   ├── development/            # Development guidelines
│   ├── operations/             # Operational documentation
│   └── guidelines/             # General guidelines
├── api/                        # API documentation
└── assets/                     # Documentation assets
```

### Project-Specific Documentation

Project-specific documentation belongs in a `docs` folder within the project folder:

```
code/MatureWeb/[ProjectName]/
└── docs/
    ├── README.md              # Project overview
    ├── features/              # Feature documentation
    └── implementation/        # Implementation details
```

## Key Guidelines for Documentation Organization

1. **Documentation Placement**
   - Solution-wide documentation should ALWAYS be placed in the root `docs/` folder
   - Project-specific documentation should ALWAYS be placed in the relevant project folder
   - Never create duplicate documentation hierarchies

2. **Cross-Project Concerns**
   - If documentation applies to multiple projects but not the entire solution, consider:
     - Generalizing it for inclusion in the solution-wide documentation
     - Placing it in a shared/common location and referencing it
     - Creating project-specific versions only when necessary

3. **Avoid Redundancy**
   - Don't maintain the same information in multiple places
   - Use links to reference related documentation
   - Update all references when documentation structure changes

## Markdown Style Guide

- Use headers (`#`) for section titles
- Use bullet points (`*`) for lists
- Use numbered lists (`1.`) for sequential steps
- Use code blocks (``` ```) for code snippets
- Use tables for structured data
- Use images sparingly and provide alt text
- Keep line length under 120 characters

## Documentation Structure

- Start with a clear title using a level 1 header (`#`)
- Include a brief introduction
- Use level 2 headers (`##`) for main sections
- Use level 3 headers (`###`) for subsections
- Include examples where appropriate
- Include references to related documentation

## Maintaining Documentation

- Review documentation regularly
- Update documentation when you update code
- Remove outdated documentation
- Use clear, simple language
- Use spell checking
- Have others review your documentation

## Working with Documentation Files

- Use meaningful file names in lowercase with hyphens between words
- Keep documentation files in their appropriate directories
- Use relative paths for links between documentation files
- For images, place them in `docs/assets/images/` and reference them using relative paths
- For templates, place them in `docs/assets/templates/` for reuse