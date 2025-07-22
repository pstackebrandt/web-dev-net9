# Claude Code Usage Guide

## Table of Contents

- [Overview](#overview)
- [Basic Usage](#basic-usage)
  - [Starting Claude Code](#starting-claude-code)
  - [Essential Commands](#essential-commands)
- [Error Handling](#error-handling)
  - [MCP Connection Errors](#mcp-connection-errors)
  - [Common Crash Scenarios](#common-crash-scenarios)
- [Best Practices](#best-practices)
  - [Session Management](#session-management)
  - [File Operations](#file-operations)
  - [Troubleshooting Workflow](#troubleshooting-workflow)
- [Integration with Project](#integration-with-project)
  - [Project-Specific Usage](#project-specific-usage)
  - [File Structure Awareness](#file-structure-awareness)
- [Error Recovery](#error-recovery)
  - [Session Recovery Steps](#session-recovery-steps)
  - [Data Loss Prevention](#data-loss-prevention)
- [Known Issues](#known-issues)
  - [MCP Error -32000](#mcp-error--32000)
  - [VS Code Extension Dependencies](#vs-code-extension-dependencies)
- [Additional Resources](#additional-resources)

## Overview

This guide covers common usage patterns, error handling, and troubleshooting for Claude Code in the web-dev-net9 project.

## Basic Usage

### Starting Claude Code

```bash
# Start new session
claude-code

# Resume previous session after crash/interruption
claude-code --resume
```

### Essential Commands

```bash
# Get help
/help

# Exit Claude Code
exit
```

## Error Handling

### MCP Connection Errors

**Error Pattern:**
```
kW [McpError]: MCP error -32000: Connection closed
```

**Cause:** Lost connection between Claude and VS Code during operation

**Solution:**
1. Restart Claude Code:
   ```bash
   claude-code
   ```

2. Resume your session:
   ```bash
   claude-code --resume
   ```

**Prevention:**
- Keep VS Code open and stable during Claude sessions
- Avoid VS Code restarts during active Claude operations
- Save important work before major file operations

### Common Crash Scenarios

1. **During Edit Confirmation:** Error occurs while confirming file edits
2. **VS Code Extension Issues:** MCP extension becomes unresponsive
3. **Network Interruptions:** Connection drops during tool operations

## Best Practices

### Session Management

- Use `--resume` flag after any unexpected termination
- Keep VS Code and Claude Code terminals in separate windows
- Monitor VS Code for extension errors or crashes

### File Operations

- Confirm edit prompts promptly to avoid connection timeouts
- Use batch operations when possible to minimize connection overhead
- Save work frequently when making extensive changes

### Troubleshooting Workflow

1. **Check VS Code Status:** Ensure VS Code is running and responsive
2. **Restart Claude Code:** Use `claude-code --resume` to restore context
3. **Check MCP Extension:** Verify Claude Code MCP extension is active in VS Code
4. **Clear Session:** Start fresh with `claude-code` if resume fails

## Integration with Project

### Project-Specific Usage

- Claude Code reads `CLAUDE.md` for project context
- Follows project conventions defined in documentation
- Respects security rules for credential handling

### File Structure Awareness

Claude Code understands the project structure:
```
docs/
├── claude-code/                 # Claude Code documentation
│   ├── claude-code-usage-guide.md
│   └── claude-code-error-on-multiple-instances-audit.md
├── architecture/                # Architecture documentation
├── general/                     # General project documentation
└── README.md
```

## Error Recovery

### Session Recovery Steps

1. **Immediate Recovery:**
   ```bash
   claude-code --resume
   ```

2. **Full Restart (if resume fails):**
   ```bash
   claude-code
   ```

3. **Context Restoration:** Claude will read `CLAUDE.md` and project files to restore context

### Data Loss Prevention

- Important conversation context is preserved with `--resume`
- File changes in progress may need to be reapplied
- Use git to track and recover any lost changes

## Known Issues

### MCP Error -32000

- **Frequency:** Common during file edit confirmations
- **Impact:** Session termination, requires restart
- **Mitigation:** Use `--resume` flag consistently

### VS Code Extension Dependencies

- Claude Code requires VS Code MCP extension
- Extension crashes can cause connection errors
- Monitor VS Code extension host for issues

## Additional Resources

- Claude Code documentation: https://docs.anthropic.com/en/docs/claude-code
- Report issues: https://github.com/anthropics/claude-code/issues
- Project-specific guidelines: See `CLAUDE.md` in project root