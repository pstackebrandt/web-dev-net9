# Task Workflow Guide

## Overview

This guide explains how to use the task management system for the web-dev-net9 project when working with multiple AI assistants (Claude Code, Cursor AI, VS Code assistants).

## Core Principles

### Single Source of Truth
- **TASKS.md** is the authoritative task list for all AI assistants
- All assistants read from and update this same file
- Maintains project continuity across sessions and AI switches

### Lightweight Task Descriptions
- Keep task descriptions brief and actionable
- Link to detailed planning documents when needed
- Focus on "what" not "how" in the main task list

### Cross-AI Visibility
- Every AI can see what others have worked on
- Prevents duplicate work and conflicting approaches
- Enables informed decision-making about task priorities

## Task Lifecycle

### 1. Task Creation
```markdown
- [ ] **P2** T007 [M] [Unassigned] Implement user authentication system
```

### 2. Assignment & Work Begin
```markdown
- [ ] **P2** T007 [M] [Claude] Implement user authentication system
```
- Move from **Backlog** to **Active Tasks**
- Update AI assignment field
- AI begins work on the task

### 3. Task Completion
```markdown
- [x] ✅ **P2** T007 [M] [Claude] Implement user authentication system (2025-01-22)
```
- Move to **Completed Tasks** section
- Add completion date
- Keep AI assignment for reference

### 4. Task Handoff (Between AIs)
```markdown
- [ ] **P2** T007 [M] [Claude→Cursor] Implement user authentication system [Started by Claude, continuing with Cursor]
```
- Update AI field to show handoff
- Add context comment about current state
- Update status/progress notes

## File Organization

### Main Task File
- **File**: `tasks/TASKS.md`
- **Purpose**: High-level task tracking
- **Content**: Task summaries, priorities, assignments, dependencies

### Detailed Planning (Optional)
- **Location**: `tasks/planning/`
- **When to use**: Complex features requiring step-by-step breakdown
- **Naming**: `T###-feature-name.md`
- **Link from**: Main task description

### Session Documentation
- **Location**: `tasks/session-logs/`
- **Purpose**: Track what each AI worked on per session
- **Files**: `claude-sessions.md`, `cursor-sessions.md`

## Task Format Rules

### Standard Format
```
- [ ] **P[1-4]** T[###] [S/M/L/XL] [AI] Description [Optional: Dependencies/Links]
```

### Priority Levels
- **P1**: Critical/Blocking (fix build errors, security issues)
- **P2**: High (core features, major bugs)
- **P3**: Medium (enhancements, minor bugs)
- **P4**: Low (nice-to-have, documentation)

### Size Estimates
- **S**: 1-2 hours (small fixes, simple features)
- **M**: 4-8 hours (medium features, refactoring)
- **L**: 1-2 days (complex features, major changes)
- **XL**: Multi-day epics (require planning breakdown)

### AI Assignment
- **Claude**: Claude Code sessions
- **Cursor**: Cursor AI assistant
- **VS**: VS Code assistants
- **Unassigned**: Available for any AI
- **Multiple**: `[Claude→Cursor]` for handoffs

## Best Practices

### For All AI Assistants

1. **Always read TASKS.md first** in new sessions
2. **Update task status** when starting/completing work
3. **Move tasks between sections** as they progress
4. **Add completion dates** for finished tasks
5. **Reference session logs** for complex handoffs

### Task Creation Guidelines

1. **Check for duplicates** before adding new tasks
2. **Use clear, actionable descriptions**
3. **Add dependencies** when tasks depend on others
4. **Link to planning docs** for complex tasks
5. **Assign realistic priorities** and sizes

### Multi-AI Collaboration

1. **Mark AI assignment** when starting work
2. **Update progress** in task descriptions if needed
3. **Document handoff context** when switching AIs
4. **Review completed tasks** to understand project history
5. **Maintain task numbering** consistency (T001, T002, etc.)

## Common Scenarios

### Starting New Session
```bash
# 1. Read current tasks
cat tasks/TASKS.md

# 2. Check what's in progress
grep "Active Tasks" -A 20 tasks/TASKS.md

# 3. Choose task and update assignment
# Edit TASKS.md to assign yourself
```

### Completing a Task
1. Move task from **Active** to **Completed**
2. Add ✅ checkbox and completion date
3. Check if any dependent tasks can now start

### Handoff to Another AI
1. Add detailed comment about current state
2. Update AI assignment field
3. Consider creating session log entry
4. Link to relevant files or decisions made

### Complex Task Breakdown
1. Create planning document in `tasks/planning/`
2. Link from main task: `[Details](./planning/T###-feature.md)`
3. Keep main task description simple
4. Update planning doc as work progresses

## Integration with Development

### Git Workflow
- Reference task IDs in commit messages: `git commit -m "T007: Add authentication middleware"`
- Use task branches: `git checkout -b T007-user-auth`
- Link pull requests to task numbers

### Documentation
- Update CLAUDE.md when tasks affect project guidelines
- Reference tasks in architecture decisions
- Keep README.md current with completed features

### Testing
- Include testing tasks for major features
- Reference test files from tasks when relevant
- Ensure test coverage for critical paths (P1/P2 tasks)