# Linting and Formatting Guidelines

## Project Structure

### Code Linting
- **C#/ASP.NET**: Use `.editorconfig` for consistent formatting
- **Markdown**: Use `.markdownlint.json` for project documentation
- **Obsidian**: Use `.obsidian/settings.json` for vault-specific settings

## Obsidian Vault Linting

### Required Plugins
1. **Linter**
   - Enable "Linter" plugin
   - Configure rules for consistent formatting
   - Set up auto-fix on save

2. **Templater**
   - Use for consistent document structure
   - Enforce metadata standards
   - Automate repetitive content

### Recommended Settings
```json
{
  "strictLineBreaks": true,
  "showFrontmatter": true,
  "showLineNumber": true,
  "showIndentGuide": true,
  "showInlineTitle": true
}
```

## Markdown Linting Rules

### Project Documentation (Outside Vault)
- Use `.markdownlint.json` rules
- Follow GitHub Flavored Markdown
- Include frontmatter for metadata
- Use relative links

### Obsidian Vault
- Follow Obsidian's markdown subset
- Use wiki-links `[[filename]]`
- Use tags for categorization
- Use callouts for important notes
- Use Mermaid for diagrams

## Code Documentation

### C# Documentation
- Use XML comments for public APIs
- Follow Microsoft documentation style
- Include code examples
- Use `<see cref=""/>` for cross-references

### API Documentation
- Use OpenAPI/Swagger
- Include request/response examples
- Document error codes
- Use proper HTTP status codes

## Automation

### Pre-commit Hooks
1. Format C# code
2. Lint markdown files
3. Check for broken links
4. Validate frontmatter

### CI/CD Pipeline
1. Run code analysis
2. Generate documentation
3. Validate markdown
4. Check for security issues

## Tools and Extensions

### VS Code
- C# extension
- Markdown All in One
- markdownlint
- YAML

### Obsidian
- Linter
- Templater
- Dataview
- Graph View
- Mermaid

## Best Practices

1. **Consistency**
   - Use consistent heading levels
   - Follow naming conventions
   - Maintain uniform formatting

2. **Validation**
   - Check links regularly
   - Validate code examples
   - Review generated docs

3. **Maintenance**
   - Update outdated content
   - Remove unused files
   - Archive old versions 