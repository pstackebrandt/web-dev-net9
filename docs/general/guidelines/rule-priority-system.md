# Rule Priority System for AI Guidelines

**Purpose**: Standard priority classification for AI development and documentation rules  
**Based on**: Industry standard severity/priority frameworks  
**Application**: All AI tools, all projects

## Table of Contents
- [Rule Priority System for AI Guidelines](#rule-priority-system-for-ai-guidelines)
  - [Table of Contents](#table-of-contents)
  - [Priority Level Definitions](#priority-level-definitions)
  - [Priority Assignment Guidelines](#priority-assignment-guidelines)
  - [Implementation Examples](#implementation-examples)
  - [Priority Enforcement](#priority-enforcement)
  - [Usage in Different Contexts](#usage-in-different-contexts)
  - [Review and Escalation Process](#review-and-escalation-process)

## Priority Level Definitions

### CRITICAL Priority
**Definition**: Rules that prevent security breaches, data corruption, or legal violations  
**Impact**: System failure, security compromise, or legal exposure  
**Action**: Never violate - immediate correction required  
**Timeline**: Fix immediately, all work stops

**Examples**:
- Never include real passwords/credentials in documentation
- Never commit secrets to version control
- Never expose production database information
- Never violate data privacy regulations (GDPR, etc.)

### HIGH Priority  
**Definition**: Rules that ensure core functionality and major business requirements  
**Impact**: Major functionality broken, significant business impact  
**Action**: Must be addressed in current work session/release  
**Timeline**: Fix within current work cycle

**Examples**:
- Always use TOCs for architecture documents (project-specific override)
- Follow established coding standards and patterns
- Maintain book-learning approach (project-specific constraint)
- Use proper error handling and logging practices

### MEDIUM Priority
**Definition**: Rules that improve quality, consistency, and maintainability  
**Impact**: Moderate impact on quality or user experience  
**Action**: Should be addressed in current or next release cycle  
**Timeline**: Fix within 1-2 work cycles

**Examples**:
- Use consistent file naming conventions
- Add proper cross-references between documents
- Follow standard markdown formatting
- Include appropriate code comments and documentation

### LOW Priority
**Definition**: Style preferences and minor consistency improvements  
**Impact**: Cosmetic or minor convenience issues  
**Action**: Address when convenient, can be deferred  
**Timeline**: Next major release or when time permits

**Examples**:
- Specific wording preferences in documentation
- Minor formatting consistency (spacing, capitalization)
- Optional documentation enhancements
- Non-critical tool-specific optimizations

## Priority Assignment Guidelines

### Security Assessment Matrix
```
Data Exposure Risk:
- Public Secrets/Credentials → CRITICAL
- Internal Infrastructure Details → HIGH  
- Development Environment Info → MEDIUM
- Generic Examples/Templates → LOW

Business Impact:
- Legal/Compliance Violation → CRITICAL
- Core Functionality Broken → HIGH
- Quality/Consistency Issue → MEDIUM  
- Cosmetic/Style Issue → LOW
```

### Context Considerations

#### Project-Specific Factors
- **Book-following projects**: Adherence to source material = HIGH
- **Production systems**: Security and reliability = CRITICAL
- **Learning projects**: Educational clarity = HIGH
- **Personal projects**: Flexibility with priorities

#### Tool-Specific Factors
- **Claude Code**: Documentation and security rules = HIGH
- **IDE Integration**: Code quality rules = HIGH  
- **CI/CD Systems**: Build and deployment rules = CRITICAL
- **Documentation Tools**: Formatting rules = MEDIUM

#### Timeline Factors
- **Immediate Release**: Only CRITICAL and HIGH
- **Current Sprint**: CRITICAL, HIGH, and selected MEDIUM
- **Next Release**: All priorities as time permits
- **Long-term Backlog**: Accumulated LOW priority items

## Implementation Examples

### In CLAUDE.md (Tool-Specific Config)
```markdown
## Security Rules

### CRITICAL Security Rules (Never Violate)
- Never write real passwords in documentation

### HIGH Priority Security Practices  
- Use placeholder text for examples

### MEDIUM Priority Guidelines
- Follow consistent naming conventions
```

### In Generic Standards
```markdown
## Security Guidelines

### Priority Levels
Rules are classified as CRITICAL, HIGH, MEDIUM, or LOW based on:
- Security impact (CRITICAL for credential exposure)
- Business impact (HIGH for functionality breaks)
- Quality impact (MEDIUM for consistency)
- Style impact (LOW for preferences)
```

## Priority Enforcement

### Automatic Enforcement (When Possible)
- **CRITICAL**: Hard stops, validation failures, automated rejection
- **HIGH**: Warnings with required acknowledgment
- **MEDIUM**: Soft warnings, optional bypass
- **LOW**: Information notices, easily dismissed

### Manual Review Process
1. **CRITICAL violations**: Immediate escalation, work stoppage
2. **HIGH violations**: Required review and approval to proceed  
3. **MEDIUM violations**: Recommended review, optional approval
4. **LOW violations**: Optional review, log for future consideration

## Usage in Different Contexts

### For Individual Developers
- Focus on CRITICAL and HIGH rules during active development
- Address MEDIUM rules during code review or refactoring
- Consider LOW rules during dedicated cleanup sessions

### For Teams
- Establish team agreement on priority thresholds
- Use priority levels for code review focus areas
- Plan sprint capacity with priority distribution

### For AI Tool Configuration
- Configure CRITICAL rules as hard constraints
- Set HIGH rules as strong recommendations with overrides
- Present MEDIUM/LOW rules as contextual suggestions

## Review and Escalation Process

### Regular Priority Review
**Quarterly**: Review rule priorities based on:
- Actual violation frequency and impact
- Team feedback and pain points  
- Project evolution and changing requirements
- Tool capabilities and limitations

### Priority Change Process
1. **Propose change** with justification
2. **Team review** for agreement
3. **Update documentation** across all relevant files
4. **Communicate changes** to all team members/AI tools

### Escalation Triggers
- Multiple CRITICAL violations indicate system problems
- Frequent HIGH violations suggest training or tool needs
- Consistent MEDIUM violations may warrant priority elevation
- Ignored LOW priorities may indicate rule relevance issues

---

**Implementation**: Apply this priority system consistently across CLAUDE.md, generic standards, and project-specific guidelines to ensure appropriate focus and resource allocation.