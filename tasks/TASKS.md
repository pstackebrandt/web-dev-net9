# Web-Dev-Net9 Project Tasks

## Active Tasks (Current Sprint)

- [ ] **P1** T010 [M] [Claude] Create task management system documentation *(Current session)*

---

## Backlog (Future Work)

### Database & Infrastructure (Critical)
- [ ] **P1** T001 [L] [Unassigned] Fix Database Connection Issues [Details](https://docs.microsoft.com/troubleshoot/sql)
  - 5 unit tests failing due to SQL Server connection errors
  - Network-related connection issues with Docker containers
- [ ] **P1** T002 [M] [Unassigned] Resolve Dual Database Launch Mode Conflicts
  - Implement dual-database-mode approach from audit document
  - Fix Aspire vs manual Docker conflicts
- [ ] **P2** T003 [S] [Unassigned] Fix Aspire Container Naming Issue
  - Align "azuresqledge-6053fc7d" with expected "azuresqledge" pattern

### Code Quality & Maintenance
- [ ] **P2** T004 [S] [Unassigned] Migrate Test Code from Obsolete AddNorthwindContext Method
  - Fix 3 compiler warnings in unit tests
  - Update to new IConfiguration overload
- [ ] **P3** T005 [S] [Unassigned] Complete AddNorthwindContext Removal Plan Phase 2
  - Make obsolete method throw errors per removal timeline

### Documentation & Process  
- [ ] **P1** T006 [M] [Unassigned] Implement Documentation Cleanup from Database Audit
  - Archive conflicting documentation files  
  - Create clear dual-mode setup guides
- [ ] **P2** T008 [M] [Unassigned] Improve SQL Edge Container Management Documentation
  - Complete 50+ incomplete tasks in sql-edge-container-management.md
- [ ] **P3** T007 [S] [Unassigned] Document Claude Code Multiple Instances Issue
  - Windows 11 crash workaround documentation
- [ ] **P4** T009 [S] [Unassigned] Update Architecture Decision Record ADR-0001
  - Reflect actual dual-mode approach instead of Aspire-only

---

## Completed Tasks (Archive)

- [x] ✅ **P1** T011 [M] [Claude] Create task workflow documentation (2025-01-22)
- [x] ✅ **P1** T012 [M] [Claude] Create AI handoff guide documentation (2025-01-22)
- [x] ✅ **P2** T013 [S] [Claude] Clean up and restructure TASKS.md file (2025-01-22)

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
- **Details**: [Planning Document](./planning/task-details-T###.md) (optional)

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
