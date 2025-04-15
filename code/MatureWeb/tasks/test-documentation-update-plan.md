# Test Documentation Update Plan

This plan outlines the necessary updates to documentation files following the refactoring of the test base classes (`TestBase`, `InMemoryTestBase`, `DatabaseTestBase`).

## 1. Update `test-configuration-guide.md`

- **Goal**: Reflect the new abstract `TestBase` and the specialized `InMemoryTestBase` and `DatabaseTestBase`.
- **Actions**:
  - [x] Remove explanations of the old, combined `TestBase`.
  - [x] Explain the purpose of the new abstract `TestBase` (providing `BuildConfiguration`).
  - [x] Detail `InMemoryTestBase`:
    - [x] Explain its purpose (isolated in-memory tests).
    - [x] Explain how to use `CreateInMemoryContext()`.
    - [x] Mention the requirement for the `Microsoft.EntityFrameworkCore.InMemory` package.
  - [x] Detail `DatabaseTestBase`:
    - [x] Explain its purpose (tests against a real database).
    - [x] Explain how `GetFileBasedTestSettings()` uses `BuildConfiguration`.
    - [x] Explain how to use `CreateDatabaseContext()`.
  - [x] Update any code examples showing test class setup to reflect the new base classes and methods.
  - [x] Add clear guidance on choosing the correct base class (`InMemoryTestBase` or `DatabaseTestBase`) to inherit from based on test needs.

## 2. Create/Update `TestStructure.md`

- **Goal**: Visually represent the new test base class hierarchy.
- **Actions**:
  - [x] Create (if it doesn't exist) or update `docs/TestStructure.md`.
  - [x] Include a diagram (e.g., using Mermaid syntax) showing the inheritance:
    ```mermaid
    graph TD
        A[TestBase (abstract)] --> B(InMemoryTestBase);
        A --> C(DatabaseTestBase);
    ```
  - [x] Briefly describe the role of each class (`TestBase`, `InMemoryTestBase`, `DatabaseTestBase`) in the structure.

## 3. Archive/Summarize `test-base-refactoring-plan.md`

- **Goal**: Clean up temporary planning documents.
- **Actions**:
  - [ ] Review `tasks/test-base-refactoring-plan.md`.
  - [ ] Decide if key rationale/decisions should be moved to a more permanent design decisions document (e.g., a new `docs/test-design-decisions.md`).
  - [ ] If summarized elsewhere or deemed unnecessary, archive or remove `tasks/test-base-refactoring-plan.md`.

## 4. Review `user-secrets-setup.md`

- **Goal**: Ensure user secrets instructions are still accurate.
- **Actions**:
  - [ ] Quickly review the steps in `docs/user-secrets-setup.md`.
  - [ ] Confirm accuracy, especially regarding how `DatabaseTestBase` utilizes configuration built by `TestBase`.

## 5. Update Main Refactoring Plan

- **Goal**: Ensure the main plan accurately reflects the documentation tasks.
- **Actions**:
  - [ ] Update `tasks/test-infrastructure-refactoring-plan.md` to:
    - Link to this detailed documentation plan.
    - Mark relevant documentation tasks as in progress or complete as they are addressed.