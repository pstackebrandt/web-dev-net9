1# AI Documentation Standards

**Purpose**: Generic documentation standards for AI tools across all projects  
**Scope**: Claude Code, Cursor AI, and future AI tools  
**Reusability**: Copy this file to any project

## Table of Contents
- [AI Documentation Standards](#ai-documentation-standards)
  - [Table of Contents](#table-of-contents)
  - [Table of Contents Rules](#table-of-contents-rules)
  - [Markdown Structure Standards](#markdown-structure-standards)
  - [File Naming Conventions](#file-naming-conventions)
  - [Documentation Hierarchy](#documentation-hierarchy)
  - [Content Organization](#content-organization)
  - [Cross-Reference Standards](#cross-reference-standards)
  - [AI Tool Integration](#ai-tool-integration)

## Table of Contents Rules

### When to Add TOC
**Standard Rule**: Add Table of Contents to markdown files with **3+ main sections** OR **>150 lines**

### TOC Format Requirements
- **Placement**: After title/summary, before first content section
- **Structure**: Nested bullet points with anchor links
- **Depth**: Include H2 and H3 levels, optionally H4 for complex documents
- **Naming**: Use exact heading text for link anchors

### TOC Template
```markdown
## Table of Contents
- [Main Section 1](#main-section-1)
  - [Subsection 1.1](#subsection-11)
  - [Subsection 1.2](#subsection-12)
- [Main Section 2](#main-section-2)
- [Main Section 3](#main-section-3)
```

### Exceptions to TOC Rule
- **Sequential tutorials**: Pure step-by-step guides meant for linear reading
- **Simple lists**: Basic reference lists or indexes
- **Meeting notes**: Time-based sequential content
- **Short forms**: Brief templates or forms

## Markdown Structure Standards

### Heading Hierarchy
- **H1 (#)**: Document title only (one per document)
- **H2 (##)**: Main sections
- **H3 (###)**: Subsections
- **H4 (####)**: Sub-subsections (use sparingly)

### Required Front Matter
All documentation files should include:
```markdown
# Document Title

**Purpose**: Brief description of document purpose
**Date**: Creation/last major update date
**Context**: Project or situation context (when relevant)
```

### Document Structure Template
```markdown
# Title

**Purpose**: What this document does
**Context**: When/why it was created

## Table of Contents
[Generated TOC]

## Overview/Summary
Brief summary of content

## Main Content Sections
[Your content sections]

## Next Steps / Conclusion
[Action items or summary]

## References / See Also
[Related documentation links]
```

## File Naming Conventions

### Standard Pattern
- Use **kebab-case**: `my-document-name.md`
- Be **descriptive**: `database-setup-guide.md` not `setup.md`
- Include **scope**: `project-ai-guidelines.md` not `guidelines.md`

### Document Type Prefixes (Optional)
- `guide-`: How-to documentation
- `ref-`: Reference materials  
- `spec-`: Specifications or requirements
- `adr-`: Architecture Decision Records
- `audit-`: Investigation or audit reports

### Examples
✅ Good:
- `ai-documentation-standards.md`
- `database-launch-modes-audit.md`
- `guide-manual-docker-setup.md`

❌ Avoid:
- `docs.md` (too generic)
- `Setup Guide.md` (spaces, inconsistent case)
- `temp-file.md` (temporary naming)

## Documentation Hierarchy

### Where to Document What

#### 1. Tool-Specific Configs (Highest Priority)
**Files**: `CLAUDE.md`, `.cursor/rules/`, tool-specific configs  
**Content**: High-priority overrides, tool-specific syntax, critical rules  
**Pattern**: Brief rule + reference to detailed implementation

#### 2. Generic Standards (Base Implementation)
**Files**: `docs/general/guidelines/ai-*.md`  
**Content**: Detailed rules, examples, rationale, cross-tool standards  
**Pattern**: Complete implementation details

#### 3. Project-Specific Guidelines (Context)
**Files**: `docs/general/guidelines/project-ai-guidelines.md`  
**Content**: Project context, specific adaptations, team agreements  
**Pattern**: Context + references to generic standards

#### 4. Domain-Specific Documentation
**Files**: Various locations based on domain  
**Content**: Implementation guides, setup instructions, troubleshooting  
**Pattern**: Detailed how-to with references to standards

## Content Organization

### Section Ordering
1. **Purpose/Context** (what, why, when)
2. **Table of Contents** (if 3+ sections)
3. **Overview/Summary** (key points)
4. **Main Content** (detailed sections)
5. **Implementation/Next Steps** (actionable items)
6. **References/See Also** (related documentation)

### Writing Guidelines
- **Be concise**: Prefer shorter sentences and paragraphs
- **Use active voice**: "Create the file" not "The file should be created"
- **Include examples**: Show don't just tell
- **Add context**: Explain the "why" behind rules
- **Cross-reference**: Link to related documentation

## Cross-Reference Standards

### Internal Links
```markdown
[Link Text](relative-path-to-file.md)
[Link to Section](file.md#section-anchor)
[Link to Same File](#section-anchor)
```

### Reference Patterns
- **Generic to Specific**: Generic files reference specific implementations
- **Specific to Generic**: Specific files reference generic standards
- **Tool configs reference details**: Brief rule + link to full documentation

### Link Maintenance
- Use **relative paths** for portability
- **Test links** when documents move or rename
- **Update references** when reorganizing documentation

## AI Tool Integration

### Claude Code Integration
- Place high-priority rules in `CLAUDE.md`
- Reference this file from `CLAUDE.md`
- Use markdown formatting (Claude Code renders markdown)

### Cursor AI Integration
- Use `.cursor/rules/` directory for Cursor-specific rules
- Reference these standards from Cursor config files
- Maintain consistency with generic standards

### Future AI Tool Support
- Generic markdown works with most AI tools
- Tool-specific syntax goes in tool-specific configs
- Always provide generic fallback rules

### Multi-Tool Consistency
- **Same rule across tools**: Put in generic standards
- **Tool-specific adaptation**: Put in tool config with reference
- **Conflicting requirements**: Document in tool config with explanation

## Security Guidelines for AI-Generated Documentation

### CRITICAL Security Rules
**These rules apply to ALL AI tools and projects**:

- **Never include real passwords, API keys, or secrets** in any documentation
- **Never include real database credentials** in examples or configuration samples
- **Never expose production URLs, server names, or sensitive infrastructure details**

### Security Documentation Standards

#### Placeholder Requirements
**Always use placeholders for sensitive information**:
```markdown
✅ Correct Examples:
- Password: `your_password`, `YourStrongPassword`, `[your-secure-password]`
- API Key: `your_api_key`, `[API_KEY]`, `sk-...truncated`
- Server: `your-server.com`, `[SERVER_NAME]`, `localhost`

❌ Never Include:
- Real passwords: `MyRealPassword123!`
- Real API keys: `sk-1234567890abcdef...` 
- Real server names: `prod-db-01.company.com`
```

#### Documentation Review Checklist
Before finalizing AI-generated documentation:
- [ ] Scan for real passwords or credentials
- [ ] Verify all examples use placeholders
- [ ] Check configuration samples for real values
- [ ] Ensure server/URL references are genericized

### Tool-Specific Security Integration
Each AI tool should enforce these security guidelines through:
- **Pre-generation checks**: Warn before including potential credentials
- **Post-generation review**: Highlight suspicious patterns
- **Template enforcement**: Provide secure example templates

---

**Usage Instructions**: Copy this file to `docs/general/guidelines/` in any project and adapt the 
`project-ai-guidelines.md` file for project-specific context.