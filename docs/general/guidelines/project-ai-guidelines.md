# Project AI Guidelines

**Purpose**: Project-specific AI guidance for web-dev-net9  
**Base Standards**: [AI Documentation Standards](ai-documentation-standards.md)  
**Book Context**: "Real-World Web Development with .NET 9" by Mark J. Price (Packt)

## Table of Contents
- [Project AI Guidelines](#project-ai-guidelines)
  - [Table of Contents](#table-of-contents)
  - [Project Context](#project-context)
    - [Book-Following Approach](#book-following-approach)
    - [Technology Stack](#technology-stack)
  - [Development Philosophy](#development-philosophy)
    - [Dual-Path Approach](#dual-path-approach)
    - [Change Guidelines](#change-guidelines)
  - [Documentation Hierarchy](#documentation-hierarchy)
    - [Where to Document What (Project-Specific)](#where-to-document-what-project-specific)
      - [1. CLAUDE.md (Highest Priority)](#1-claudemd-highest-priority)
      - [2. Generic Standards (Reference)](#2-generic-standards-reference)
      - [3. This File (Project Context)](#3-this-file-project-context)
      - [4. Domain Documentation](#4-domain-documentation)
  - [Project-Specific Overrides](#project-specific-overrides)
    - [TOC Requirements (Stricter than Generic)](#toc-requirements-stricter-than-generic)
    - [File Naming (Project Pattern)](#file-naming-project-pattern)
    - [Book Reference Requirements](#book-reference-requirements)
  - [AI Tool Configuration](#ai-tool-configuration)
    - [Claude Code Integration](#claude-code-integration)
    - [Future Tool Integration](#future-tool-integration)

## Project Context

### Book-Following Approach
This project follows "Real-World Web Development with .NET 9" by Mark J. Price. Key implications:

- **Minimal Changes**: Prefer sparse changes to book examples
- **Educational Focus**: Maintain learning-oriented code structure
- **Book Patterns**: Preserve original architectural patterns from the book
- **Incremental**: Add modern practices (like Aspire) as layers, not replacements

### Technology Stack
- .NET 9.0 (SDK 9.0.202)
- ASP.NET Core MVC 9.0.5
- Entity Framework Core 9.0.5
- SQL Server via Docker (SQL Edge)
- xUnit for testing
- Bootstrap for UI
- .NET Aspire for orchestration (layered addition)

## Development Philosophy

### Dual-Path Approach
Support both learning and modern development:

1. **Understanding Mode** (Book Approach)
   - Manual Docker setup
   - Direct configuration files
   - Clear, educational code patterns

2. **Modern Mode** (Production-Ready)
   - Aspire orchestration
   - Service discovery
   - Advanced tooling integration

### Change Guidelines
- **Book code**: Change only when necessary for functionality
- **New features**: Use modern patterns while maintaining compatibility
- **Documentation**: Always explain "why" behind deviations from book
- **Testing**: Ensure both modes continue working after changes

## Documentation Hierarchy

### Where to Document What (Project-Specific)

#### 1. CLAUDE.md (Highest Priority)
- Critical project rules that override generic standards
- Book-specific constraints ("minimal changes")
- Essential development commands
- Priority overrides for this project

#### 2. Generic Standards (Reference)
- Cross-project documentation rules
- Multi-tool compatibility guidelines
- Standard formatting and structure rules
- Detailed implementation guidance

#### 3. This File (Project Context)
- Book context and constraints
- Project-specific adaptations
- Team agreements for this project
- Technology stack considerations

#### 4. Domain Documentation
- Architecture decisions (ADRs)
- Setup guides and troubleshooting
- Investigation reports and audits
- Feature-specific documentation

## Project-Specific Overrides

### TOC Requirements (Stricter than Generic)
**Override**: Always use TOCs for:
- Architecture documents and ADRs
- Investigation/audit reports  
- Any document with 3+ sections (strict enforcement)
- Setup guides and troubleshooting docs

**Rationale**: This project generates complex technical documentation that benefits from navigation aids.

### File Naming (Project Pattern)
**Pattern**: `{type}-{specific-name}.md`
- `database-launch-modes-audit.md`
- `guide-manual-docker-setup.md`
- `adr-0001-sql-edge-container-management.md`

### Book Reference Requirements
**Rule**: When deviating from book patterns, always document:
```markdown
**Book Deviation**: This differs from Mark J. Price's approach in Chapter X because [reason].
**Original Pattern**: [brief description of book's approach]
**Our Adaptation**: [explanation of our changes]
```

## AI Tool Configuration

### Claude Code Integration
- High-priority rules in `CLAUDE.md`
- Reference this file for project context
- Always explain book-learning constraints
- Enforce strict TOC requirements

### Future Tool Integration
- Cursor AI: Can reference these same guidelines
- Other AI tools: Use generic standards + this context file
- Consistency: All tools should respect book-following approach

---

**Usage**: This file provides project-specific context that works alongside the generic AI documentation 
standards to create consistent, book-respectful AI assistance for the web-dev-net9 project.