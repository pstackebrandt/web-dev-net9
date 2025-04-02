# Obsidian Guidelines

## Vault Location
- Current location: `docs-obsidian/` in project root
- This is a good location as it keeps documentation close to the code
- Separate from general project documentation

## Content Guidelines

### What to Store in Obsidian
✅ **Store in Obsidian:**
- Project architecture decisions
- Development workflows
- Team guidelines
- Knowledge base
- Meeting notes
- Feature specifications
- API documentation
- System design documents

❌ **Do NOT Store in Obsidian:**
- Code documentation (use XML comments)
- README files (use project root)
- CI/CD configurations
- Build scripts
- Environment settings
- Security credentials
- External API keys

## File Organization

### Naming Convention
- Use numbers for ordering: `01-`, `02-`, etc.
- Use hyphens for spaces
- Use descriptive names
- Example: `01-Project-Setup.md`

### Folder Structure
```
docs-obsidian/
├── 00-Meta/           # Guides and templates
├── 01-Architecture/   # System design
├── 02-Development/    # Development guides
├── 03-API/           # API documentation
└── 04-Meetings/      # Meeting notes
```

## Best Practices

1. **Links**
   - Use `[[filename]]` for internal links
   - Use `#tag` for categorization
   - Link to code files when relevant

2. **Content Structure**
   - Start with a clear title (H1)
   - Include a brief description
   - Use headings for hierarchy
   - Add relevant tags

3. **Images**
   - Store in project's `images/` directory
   - Use relative paths
   - Include alt text

4. **Version Control**
   - Commit meaningful changes
   - Use clear commit messages
   - Review changes before committing

## Getting Started
1. Review existing documents
2. Follow the folder structure
3. Use templates for consistency
4. Link related content
5. Add appropriate tags 