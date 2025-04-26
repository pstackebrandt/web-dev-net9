# Accessibility Guidelines

This document provides guidelines and solutions for common accessibility issues in our application. Following these practices ensures our application is usable by people with various disabilities and complies with accessibility standards.

## Table of Contents

- [Accessibility Guidelines](#accessibility-guidelines)
  - [Table of Contents](#table-of-contents)
  - [Empty Anchor Tags](#empty-anchor-tags)
    - [Issue](#issue)
    - [Solution](#solution)
    - [Why This Works](#why-this-works)
    - [Implementation Pattern](#implementation-pattern)
  - [Button Roles on Links](#button-roles-on-links)
    - [Issue](#issue-1)
    - [Solution](#solution-1)
    - [Implementation Pattern](#implementation-pattern-1)
  - [Model Validation](#model-validation)
    - [Issue](#issue-2)
    - [Solution](#solution-2)
    - [Implementation Pattern](#implementation-pattern-2)
  - [Screen Reader Accessibility](#screen-reader-accessibility)
  - [Keyboard Navigation](#keyboard-navigation)
  - [Color Contrast](#color-contrast)
  - [Form Inputs](#form-inputs)

## Empty Anchor Tags

### Issue

SonarQube warning: "Anchors must have content and the content must be accessible by a screen reader."

Empty anchor tags (`<a id="someId" />`) used as jump targets don't contain content that can be read by screen readers, making them inaccessible.

### Solution

Use the Bootstrap `visually-hidden` class along with `aria-label` to make anchors accessible while maintaining their visual appearance:

```html
<!-- ❌ Inaccessible anchor -->
<a id="endOfTable" />

<!-- ✅ Accessible anchor with visually hidden content -->
<a id="endOfTable" href="#endOfTable" aria-label="End of orders table">
    <span class="visually-hidden">End of orders table</span>
</a>
```

### Why This Works

1. The `visually-hidden` class (from Bootstrap) hides content visually but keeps it accessible to screen readers
2. The `aria-label` provides additional context to screen reader users
3. The self-referential `href` attribute makes it a valid link
4. Screen readers can announce the anchor's purpose

### Implementation Pattern

For jump targets or anchor points in the page:

1. Always include descriptive text within the anchor
2. Use the `visually-hidden` class to hide content visually
3. Include an `aria-label` that describes the anchor's purpose
4. Add a self-referential `href` attribute

## Button Roles on Links

### Issue

SonarQube warning: "Use <button> or <input> instead of the button role to ensure accessibility across all devices."

Adding `role="button"` to anchor tags (`<a>`) that are styled as buttons creates confusion for screen readers and doesn't follow accessibility best practices.

### Solution

For navigation links styled as buttons:
- Use Bootstrap's button classes for styling (`btn btn-primary`, etc.)
- Do not add `role="button"` to anchor tags
- Keep the anchor tag since it's a navigation element

```html
<!-- ❌ Incorrect usage with role="button" -->
<a asp-controller="Home" class="btn btn-outline-primary" role="button">This page</a>

<!-- ✅ Correct usage without role attribute -->
<a asp-controller="Home" class="btn btn-outline-primary">This page</a>
```

For actual buttons that perform an action rather than navigate:
- Use a proper `<button>` element instead of an anchor tag
- Style with Bootstrap button classes

```html
<!-- ✅ Correct usage of button element for actions -->
<button type="button" class="btn btn-primary" onclick="performAction()">Submit Form</button>
```

### Implementation Pattern

1. For navigation: Use `<a>` tags with button styling but without `role="button"`
2. For actions: Use `<button>` elements with appropriate type and styling
3. Never mix the two by adding button roles to navigation elements

## Model Validation

### Issue

SonarQube warning: "ModelState.IsValid should be checked in controller actions."

Controller actions that receive model parameters should validate the model state before processing to prevent invalid data from being processed and to provide proper feedback to users, especially those using assistive technologies.

### Solution

Always check ModelState.IsValid at the beginning of controller actions that accept model parameters:

```csharp
// ❌ Incorrect: No validation check
public IActionResult Shipper(Shipper shipper)
{
    return View(shipper);
}

// ✅ Correct: With validation check
public IActionResult Shipper(Shipper shipper)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    
    return View(shipper);
}
```

### Implementation Pattern

1. Add validation check at the start of controller actions
2. Return appropriate status code (typically BadRequest) with validation errors
3. Only proceed with the action if the model is valid
4. Consider providing accessible validation summaries in your views

## Screen Reader Accessibility

*To be documented*

## Keyboard Navigation

*To be documented*

## Color Contrast

*To be documented*

## Form Inputs

*To be documented*