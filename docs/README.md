# Documentation

This folder contains the general documentation for the web-dev-net9 solution.

## Structure

- **general/** - Solution-wide documentation
  - **architecture/** - System architecture documents
    - **decisions/** - Architectural decision records (ADRs)
    - **diagrams/** - Architecture diagrams
  - **development/** - Development guidelines
  - **operations/** - Operational documentation
  - **guidelines/** - General guidelines
- **api/** - API documentation
  - **endpoints/** - Detailed endpoint documentation
- **assets/** - Documentation assets
  - **images/** - Images used in documentation
  - **templates/** - Documentation templates

## Documentation Guidelines

- General solution-wide documentation belongs in this folder
- Project-specific documentation should be placed in the respective project folders
- Use Markdown for all documentation files
- Follow the formatting rules in `.markdownlint.json`
- Keep documentation up-to-date with code changes
- Use relative links to reference other documentation files

## Project-Specific Documentation

Project-specific documentation should follow this structure within project folders:

```
ProjectName/
└── docs/
    ├── README.md              # Project overview
    ├── features/              # Feature documentation
    ├── configuration.md       # Configuration details
    └── testing.md             # Testing approach
```