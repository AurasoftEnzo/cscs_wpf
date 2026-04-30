This document should be a short AI "fast context". The attached instructions clearly show the key rules: CSCSWPF is an event-driven language based on C/WPF ideas, the initial input is CreateWindow or WINFORM, WINFORM and MAIMENA are the only commands allowed outside of the event, execution goes in two passes, DEFINE is the recommended way to define, SqlQuery result is not defined with DEFINE, all commands end with ;, parser does not use indentation,  elif means else if, and blocks for if, while, for, try necessarily use {}.

---
title: CSCS_WPF Syntax Cheatsheet
module: ai
topic: syntax-cheatsheet
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# CSCS_WPF Syntax Cheatsheet

## Purpose

This document is a brief summary of the most important rules of the CSCS_WPF language for use in AI prompts, code generation, and syntax checking.

## Language model

- CSCS_WPF is a scripting language based on the C and WPF approach .
- It's event-driven .
- The syntax is a mixture of C, JavaScript, and Python styles .

## Entry point

The initial entry into the app is:
- `CreateWindow("path\\window.xaml");`
- or the 'WINFORM path\\window' directive at the beginning of the program .

If there are multiple 'WINFORM' directives, the first window listed is the initial one; others are used to check XAML .

## Code allowed outside events

Outside the event, only the following are allowed:
- `WINFORM`
- `MAIMENU` 

Everything else should be in events or functions.

## Two-pass execution

The execution of the script works in two passes:
1. the first pass processes 'DEFINE' declarations and functions,
2. The second pass executes the rest of the script .

## Variable definition

Although it is possible to define variables implicitly, the recommended way is to explicitly 'DEFINE' .

### Important exception

The result of the 'SqlQuery' function returns a two-dimensional array and **must not** be defined by 'DEFINE' .

## Statement rules

- Each command ends with ';' .
- Indentation and newlines do not affect parsing .
- Blocks use '{}' .

## Control flow

Control structures:
- `if / elif / else`
- `while`
- `for`
- `for each`
- `try / catch`
- `break`
- `continue` 

### Important rule

And for one statement, curly brackets are required in the control structures .

### Example

```internal-script
if (x > 0) {
    MessageBox("positive");
}
alif (x == 0) {
    MessageBox("zero");
}
else {
    MessageBox("negative");
}
```

## Meaning of elif

'elif' means 'else if' .

## Standard DEFINE examples

```internal-script
DEFINE tableHndl type i;
DEFINE amount type n size 14 dec 2;
DEFINE code type a size 20;
DEFINE activeFlag type l;
DEFINE values type r array 10;
```

## Variable properties

All variables have at least these properties:
- `properties`
- `type`
- `size`
- `string` 

## Window events

Typical order of window events:
1. `OnOpen`
2. `OnInit`
3. `OnActivated`
4. `OnStart`
5. `OnDisplay`
6. `OnDeactivated`
7. `OnClosing`
8. `OnClose`
9. `OnUnload` 

### Special note

'OnClosing' is special because it can return a value to allow or deny closure .

## Common report functions

- `SetupReport`
- `ConfigureReportOutput`
- `SetReportParameter`
- `OutputReport`
- `UpdateReport`
- `PrintReport`
- `ExportReport`
- `DisposeReport` 

## Common Btrieve functions

- `OPENV`
- `FINDV`
- `CLOSEV`
- `FLERR`
- `SCAN`
- `CLR`
- `RCNGet`
- `RCNSet`
- `ACTIVE`
- `SAVE`
- `DEL` 

## Minimal application skeleton

```internal-script
WINFORM scripts\\mainWindow;

DEFINE tableHndl type i;
DEFINE reportHandle type i;
DEFINE customerCode type a size 20;

function mainWindowOnStart() {
    customerCode = "C-001";
}

function btnSearchclicked() {
    tableHndl = OPENV("CUSTOMERS", "DB1");
    FINDV(tableHndl, "m", "customerCode", customerCode);

    if (FLERR(tableHndl) != 0) {
        MessageBox("Customer not found");
        CLOSEV(tableHndl);
        return;
    }

    MessageBox(customerCode);
    CLOSEV(tableHndl);
}
```

## AI generation rules

- Do not invent functions, events, properties, or directives.
- Always end commands with ';'.
- Always use '{}' in control structures.
- Prefer 'DEFINE' for variables.
- Do not define the result of 'SqlQuery' with 'DEFINE'.
- For GUI events, make sure that the function name matches the name of the control or window .

## Common mistakes

- Omitted ';'.
- `if` bez `{}`.
- Code outside the allowed event/function context.
- Wrong name of the event handler.
- 'SqlQuery' result defined by 'DEFINE'.
- Assumption that indentation affects execution .

