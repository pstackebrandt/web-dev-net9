# AI Handoff Guide

## Purpose

Guidelines for seamless task handoffs between different AI assistants (Claude Code, Cursor AI, VS Code assistants) working on the web-dev-net9 project.

## Quick Reference

### Before Handoff (Current AI)
1. Update task status in TASKS.md
2. Document current state/progress
3. Log session summary
4. Commit/save all work in progress

### After Handoff (New AI)
1. Read TASKS.md for context
2. Review session logs
3. Check git status and recent commits
4. Update task assignment

## Detailed Handoff Process

### 1. Preparation (Outgoing AI)

**Update Task Status:**
```markdown
# Change from:
- [ ] **P2** T007 [M] [Claude] Implement user authentication

# To:
- [ ] **P2** T007 [M] [Claude→Cursor] Implement user authentication [Auth middleware completed, need UI integration]
```

**Document Progress:**
- What's been completed
- Current blockers or decisions needed
- Files modified
- Next logical steps

**Session Log Entry:**
```markdown
## Claude Session - 2025-01-22

### T007: User Authentication Implementation
- ✅ Created authentication middleware
- ✅ Setup JWT configuration
- 🔄 Started login controller (50% complete)
- ❌ UI integration not started
- **Next**: Complete LoginController.cs, add login form
- **Files**: Controllers/AuthController.cs, Services/AuthService.cs
- **Branch**: T007-user-auth
```

### 2. Context Transfer (Incoming AI)

**Read Current State:**
```bash
# 1. Check active tasks
grep -A 5 "Active Tasks" tasks/TASKS.md

# 2. Review recent session logs
tail -20 tasks/session-logs/claude-sessions.md

# 3. Check git status
git status
git log --oneline -10
```

**Update Assignment:**
```markdown
# Change from:
- [ ] **P2** T007 [M] [Claude→Cursor] Implement user authentication [Auth middleware completed, need UI integration]

# To:  
- [ ] **P2** T007 [M] [Cursor] Implement user authentication [Continuing from Claude: auth middleware done, working on UI]
```

## Session Logging Templates

### Starting Session Template
```markdown
## [AI] Session - [Date]

### Session Goals
- [ ] Complete T### task
- [ ] Fix issue X
- [ ] Start work on Y

### Handoff Context (if applicable)
- Previous AI: [Name]
- Current state: [Description]
- Files to focus on: [List]
```

### Ending Session Template  
```markdown
### Session Summary
- ✅ Completed: [List achievements]  
- 🔄 In Progress: [Current work state]
- ❌ Blocked: [Issues encountered]
- **Next Session**: [Recommended next steps]
- **Files Modified**: [List]
- **Commits**: [Commit hashes if applicable]

### Notes for Next AI
- [Context, decisions, gotchas]
```

## Common Handoff Scenarios

### 1. Mid-Task Handoff
**Situation**: Complex task needs to continue with different AI

**Process**:
- Document exactly where you stopped
- List files in progress (uncommitted changes)
- Describe approach taken so far
- Suggest next steps

**Example**:
```markdown
T012 [M] [Claude→Cursor] Database migration system [Tables created, need seeding logic]
- ✅ Created migration files for Users, Products tables  
- ✅ Updated DbContext configuration
- 🔄 Working on data seeding (SeedData.cs 30% done)
- **Next**: Complete SeedData.cs, test migration commands
- **Uncommitted**: Migrations/20250122_AddUserTables.cs, Data/SeedData.cs
```

### 2. Different Expertise Handoff
**Situation**: Task requires different AI's strengths

**Process**:
- Explain why handoff is beneficial
- Provide full context of technical decisions
- Hand over cleanly completed portions

**Example**:
```markdown  
T015 [L] [Claude→Cursor] Frontend component optimization [Backend API ready, need React expertise]
- ✅ Completed REST API endpoints (/api/users, /api/products)
- ✅ Added comprehensive API documentation  
- ✅ Unit tests for all endpoints (100% coverage)
- **Handoff Reason**: Need React/UI optimization expertise
- **Next**: Optimize React components for performance, add lazy loading
```

### 3. Blocked Task Handoff
**Situation**: Current AI is blocked, another might have solution

**Process**:
- Document the blocker clearly
- Explain attempts made to resolve
- Suggest alternative approaches

## File Management During Handoffs

### Work in Progress Files
- **Commit partial work** with clear commit messages
- **Use feature branches** named after task IDs
- **Document uncommitted changes** in handoff notes

### Session State Preservation
```bash
# Before ending session
git add .
git commit -m "T007: WIP - Auth middleware completed, UI integration started"
git push origin T007-user-auth

# Note in handoff:
# "Branch T007-user-auth has latest work, AuthController.cs needs completion"
```

## Best Practices

### For Smooth Handoffs

1. **Be Explicit**: Don't assume next AI understands context
2. **Document Decisions**: Why you chose approach X over Y  
3. **List Dependencies**: What other tasks/files this affects
4. **Provide Examples**: Show format/patterns you've established
5. **Update Immediately**: Don't delay updating TASKS.md

### Communication Style
- Use clear, factual language
- Avoid subjective opinions ("I think", "maybe")
- Focus on actionable information
- Include file paths and line numbers when relevant

### Common Mistakes to Avoid
- ❌ Leaving tasks in "Active" without progress notes
- ❌ Not documenting current approach/reasoning  
- ❌ Forgetting to update AI assignment in TASKS.md
- ❌ Leaving uncommitted changes without explanation
- ❌ Not providing enough context for continuation

## Emergency Handoffs

### Session Crash/Unexpected Termination

**If Previous AI Session Crashed:**
1. Check git log for last commits
2. Look for any session logs from that day
3. Check file modification times  
4. Read TASKS.md for last known status
5. Start conservatively - understand before changing

**Recovery Steps:**
```bash
# Check what was being worked on
git log --since="1 day ago" --oneline
git status
find . -name "*.cs" -mtime -1  # Files modified in last day

# Review task file
grep -n "T0" tasks/TASKS.md | grep -v "✅"  # Find active tasks
```

## Integration with Claude Code

### Resuming After Claude Code Crashes
- Use `claude-code --resume` to restore session context
- Check if task status in TASKS.md matches actual progress  
- Verify git state matches documented progress
- Update session logs with any recovery actions needed