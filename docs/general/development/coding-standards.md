# Coding Standards

## Table of Contents

* [General Guidelines](#general-guidelines)
* [C# Coding Style](#c-coding-style)
  * [Naming Conventions](#naming-conventions)
  * [Code Organization](#code-organization)
  * [Comments](#comments)
  * [Error Handling](#error-handling)
* [ASP.NET Core Guidelines](#aspnet-core-guidelines)
* [Entity Framework Core Guidelines](#entity-framework-core-guidelines)
* [Testing Guidelines](#testing-guidelines)
* [Code Reviews](#code-reviews)

## General Guidelines

* Write clean, readable, and maintainable code
* Follow SOLID principles
* Use meaningful names for variables, methods, and classes
* Keep methods and classes small and focused
* Write code that is testable
* Document public APIs

## C# Coding Style

### Naming Conventions

* **PascalCase** for class names, method names, and constants
* **camelCase** for local variables and method parameters
* **_camelCase** for private fields
* **ICapitalized** for interfaces
* **TCapitalized** for generic type parameters

### Code Organization

* One class per file (except for small related classes)
* Group related types in the same namespace
* Use folders to organize code by feature
* Keep files under 500 lines where possible

### Comments

* Use XML comments for public APIs
* Use regular comments for complex algorithms
* Focus on explaining "why" rather than "what"
* Keep comments up-to-date with code changes

### Error Handling

* Use exceptions for exceptional conditions
* Handle exceptions at the appropriate level
* Log exceptions with sufficient context
* Use specific exception types

## ASP.NET Core Guidelines

*Add ASP.NET Core specific guidelines here*

## Entity Framework Core Guidelines

*Add Entity Framework Core specific guidelines here*

## Testing Guidelines

*Add testing guidelines here*

## Code Reviews

*Add code review guidelines here* 