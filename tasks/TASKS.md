# Web-Dev-Net9 Project Tasks

## Active Tasks (Current Sprint)

*(Currently being worked on by AI assistants)*

---

## Backlog (Future Work)

*(Prioritized tasks ready to be started)*

---

## Completed Tasks (Archive)

*(Most recent completions first)*

---

## Task Management Guidelines

### Task Format
```
- [ ] **P[1-4]** T[###] [S/M/L/XL] [AI] Description [Depends on T###] [Link to details]
```

**Components:**
- **Priority**: P1 (Critical) → P4 (Low)
- **Task ID**: T001, T002, T003... (sequential, project-specific)
- **Size**: S(mall), M(edium), L(arge), XL (epic)
- **AI**: Claude, Cursor, VS, Other (which assistant is assigned/working)
- **Dependencies**: [Depends on T###] (optional)
- **Details**: [Link](./planning/task-details-T###.md) (optional)

### Task Lifecycle
1. **Backlog** → **Active** (when work begins)
2. **Active** → **Completed** (when finished)
3. **Completed** tasks move to archive (most recent first)

### Linking Strategy
- **Keep TASKS.md lightweight**: Brief descriptions only
- **Detailed planning**: Create separate files in `tasks/planning/` folder
- **Reference format**: `[Details](./planning/T001-feature-implementation.md)`
- **Epic breakdown**: Link to planning documents for complex features

### Multi-AI Collaboration
- **AI Assignment**: Mark which assistant is working on each task
- **Handoff Protocol**: Update status when passing tasks between AIs
- **Session Tracking**: Reference session logs in task comments when needed
- **Context Sharing**: Use this unified file for all assistants to see full picture

### Examples
```markdown
## Active Tasks
- [ ] **P1** T003 [M] [Claude] Implement user authentication system [Details](./planning/T003-auth-system.md)
- [ ] **P2** T005 [S] [Cursor] Fix responsive layout issues

## Backlog  
- [ ] **P1** T001 [L] Setup database connection and migrations [Depends on T003]
- [ ] **P3** T004 [S] Add unit tests for user service

## Completed Tasks
- [x] ✅ **P1** T002 [S] [Claude] Setup project documentation structure (2025-01-22)
```
