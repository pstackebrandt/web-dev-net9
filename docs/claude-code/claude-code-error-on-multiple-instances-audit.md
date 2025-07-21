# Claude Code Multiple Instances Issue

## Table of Contents

- [Issue Description](#issue-description)
- [Problem Details](#problem-details)
- [Current Understanding](#current-understanding)
- [Conflicting Information](#conflicting-information)
- [Recommendations](#recommendations)
- [Testing Guidelines](#testing-guidelines)
- [Additional Resources](#additional-resources)

## Issue Description

A crash occurs when attempting to run multiple Claude Code instances simultaneously within the same project directory
on Windows 11.

## Problem Details

### Environment
- **OS**: Windows 11
- **IDE**: Cursor AI
- **Project**: web-dev-net9 (.NET 9 project)
- **Claude Code**: v1.0.56 (npm-global install)

### Crash Scenario
1. First Claude Code instance running in Git Bash terminal within Cursor AI
2. Second Claude Code instance started in PowerShell 7.5.2 terminal within same Cursor AI workspace
3. Result: First instance shut down with an error

### Error Symptoms
- First Claude Code instance terminates unexpectedly
- Error message displayed (specific error details not captured)
- Both instances cannot run concurrently

## Current Understanding

### Resource Conflicts
Claude Code appears to maintain state and lock certain resources during operation. Running multiple instances can create:
- Race conditions
- Resource conflicts
- File locking issues
- State management conflicts

### Platform Behavior
This limitation appears to be consistent across platforms, not specific to Windows 11.

## Conflicting Information

### YouTube Reports
Some YouTube content creators claim to successfully use multiple Claude Code windows for the same project, which conflicts with the observed behavior.

### Possible Explanations
1. **Different project directories** - Creators may be using completely separate folders/projects
2. **Version differences** - Behavior might have changed in recent Claude Code updates
3. **Undisclosed issues** - Creators might experience conflicts but not mention them in videos
4. **Configuration differences** - Different setups or environments may behave differently

### Official Documentation Gap
The official Claude Code troubleshooting documentation does not specifically address multiple instance scenarios or concurrent usage limitations.

## Recommendations

### Primary Approach
- **Use one Claude Code instance at a time per project**
- Exit current instance before starting in a different terminal type
- Restart Claude Code when switching between Git Bash and PowerShell

### Workflow for Terminal Switching
1. Exit Claude Code from current terminal (Ctrl+C or exit command)
2. Open preferred terminal type (Git Bash or PowerShell)
3. Navigate to project directory
4. Start Claude Code again in new terminal

### Alternative Solutions
- Use separate project directories if multiple instances are absolutely required
- Consider using Claude Code's built-in terminal switching capabilities (if available)
- Monitor future updates for improved multi-instance support

## Testing Guidelines

### Safe Testing Approach
If you want to verify multiple instance behavior:

1. **Save your work** before testing
2. Start first Claude Code instance in one terminal
3. Start second instance in another terminal
4. **Be prepared to restart** if conflicts occur
5. Document specific error messages for troubleshooting

### What to Monitor
- Specific error messages
- Which instance fails (first or second)
- Resource usage during conflicts
- File locking behavior

## Reporting This Issue

### Bug Report Recommendation
This appears to be a legitimate bug that should be reported to the Claude Code development team. Consider filing an issue with:

**Issue Title**: "Multiple Claude Code instances crash in same project/workspace"

**Include in Report**:
- Environment details (Windows 11, Cursor AI, same workspace)
- Steps to reproduce
- Expected vs actual behavior
- Specific error messages (when captured)

### GitHub Issue Template
```
**Bug Description**
Running multiple Claude Code instances in the same project directory/workspace causes the first instance to crash.

**Environment**
- OS: Windows 11
- IDE: Cursor AI (both terminals in same workspace)
- Terminal Types: Git Bash 5.2.37 (MINGW64) + PowerShell 7.5.2
- Claude Code: v1.0.56 (npm-global install)

**Steps to Reproduce**
1. Start Claude Code in Git Bash terminal within Cursor AI
2. Start second Claude Code instance in PowerShell terminal in same Cursor workspace
3. First instance crashes with error

**Expected Behavior**
Either multiple instances should work, or a clear error message should prevent the second instance from starting.

**Actual Behavior**
First instance crashes unexpectedly.
```

## Additional Resources

### Documentation Links
- [Claude Code Official Documentation](https://docs.anthropic.com/en/docs/claude-code)
- [Claude Code Troubleshooting](https://docs.anthropic.com/en/docs/claude-code/troubleshooting)

### Support Channels
- [Claude Code GitHub Issues](https://github.com/anthropics/claude-code/issues) - **Report this bug here**
- Anthropic Support (for definitive guidance)

### Related Documentation
- `CLAUDE.md` - Project-specific Claude Code configuration
- `docs/general/development/` - Other development troubleshooting guides

---

**Document Status**: Initial documentation based on user-reported issue  
**Last Updated**: 2025-07-20  
**Author**: Documented from user experience and Claude Code analysis  
**Review Status**: Needs verification with official Anthropic documentation