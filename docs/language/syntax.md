This is the most important document for prompt context. Its purpose is for the AI to be given strict syntax rules and basic patterns that it must not violate. 
text
---
title: CSCS_WPF Syntax
module: language
topic: syntax
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# CSCS_WPF Syntax

## Purpose

This document defines the basic syntactic rules of the CSCS_WPF language.

## Core rules

- All commands must end with a semicolon ';'.
- Indentation and newlines are not part of the syntax.
- Code blocks always use curly braces '{ ... }`.
- Control structures such as 'if', 'elif', 'else', 'while', 'for', 'try', 'catch' always require a block in curly brackets, even when they have only one command.
- 'elif' means 'else if'.

## Example: assignment and function call

```internal-script
a = 5;
b = 10;
MessageBox(a + b);
```

## Variables

Variables can be defined:
- implicitly,
- explicitly via "DEFINE"

The recommended approach for professional code is to define it explicitly.

## Example: explicit definition

```internal-script
DEFINE total type r;
total = 100.50;
MessageBox(total);
```

## Example: multiple operations

```internal-script
DEFINE a type i;
DEFINE b type i;
DEFINE sum type i;

a = 10;
b = 20;
sum = a + b;

MessageBox(sum);
```

## Variable properties

Variables have properties available via dot notation. The most important properties are:

- `properties`
- `type`
- `size`
- `string`

## Example: property access

```internal-script
DEFINE customerName type a size 30;
customerName = "TEST";

MessageBox(Type(customerName)); 
MessageBox(Size(customerName)); 
MessageBox(String(customerName)); or MessageBox(customerName.ToString("optional formatting"));
```

## Control flow

### If / elif / else

```internal-script
DEFINE a type i;
a = 5;

if (a > 10) {
    MessageBox("greater than 10");
}
elif (a > 0) {
    MessageBox("positive");
}
else {
    MessageBox("zero or negative");
}
```

### While

```internal-script
DEFINE i type i;
i = 0;

while (i < 3) {
    MessageBox(i);
    i = i + 1;
}
```

### For

```internal-script
DEFINE i type i;

for (i = 0; i < 5; i = i + 1) {
    MessageBox(i);
}
```

### Foreach-style loop

```internal-script
arr = {10, 20, 30, 40};

for (item : arr) {
    MessageBox(item);
}
```

## Functions

Functions are declared using 'function'.

## Example: function without parameters

```internal-script
function ShowHello() {
    MessageBox("Hello");
}
```

## Example: function with parameters

```internal-script
function SumValues(a, b) {
    return a + b;
}
```

## Return behavior

'return' completes the execution of the function and returns a value if necessary.

Note: the 'return' behavior within loops should be used with caution, as it is not identical to all classical languages.

## Exceptions

```internal-script
try {
    MessageBox("before error");
    throw "Something went wrong";
}
catch (err) {
    MessageBox(err);
}
```

## Preprocess tokens

Certain preprocess tokens must be written in lowercase.

Typical examples:
- `include`
- `startdebugger`
- `function`
- `csfunction`
- `addcompnamespace`
- `addcompdefinition`
- `dllfunction`
- `dllsub`
- `importdll`
- `define`

## Script execution model

The script is executed in two passes:

1. processing of "DEFINE" declarations and functions,
2. Execute the rest of the script.

This means that the writing order of some definitions is not exactly the same as the runtime execution order.

## Special note: SqlQuery

The result of the 'SqlQuery' function returns a two-dimensional array and does not need to be defined with 'DEFINE'.

## Valid example

```internal-script
WINFORM mainWindow;

DEFINE counter type i;
counter = 0;

function mainWindowOnStart() {
    while (counter < 3) {
        MessageBox(counter);
        counter = counter + 1;
    }
}
```

## Invalid examples

### Missing semicolon

```internal-script
DEFINE counter type i
counter = 0;
```

Problem: every command must end with ';'.

### Missing curly braces

```internal-script
if (a > 0)
    MessageBox(a);
```

Problem: Control structures must have a block '{ ... }`.

### Undocumented assumptions

```internal-script
function main() {
    Print("Hello");
}
```

Problem: The existence of a 'main()' entry point should not be assumed.

## AI notes

- Do not use the Python indent style as a substitute for curly braces.
- Do not omit ';'.
- Do not assume that the GUI code is executed outside of event functions.
- The DataContext in XAML must match the prefix used in the event handler (e.g., btn in btn@OnClick).
- For click events to work, each XAML control MUST have a Name property. The name can be anything, but it must exist and be unique within the XAML.
- Prefer 'DEFINE' and documented functions instead of implicit patterns.
