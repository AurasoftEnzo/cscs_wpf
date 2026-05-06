title: WINFORM and Window Lifecycle
module: gui
topic: window-lifecycle
applies_to: CSCS_WPF
version: 1
---

# WINFORM and Window Lifecycle

## Purpose
This document describes how CSCS_WPF windows are created and how special window lifecycle events are mapped to script functions.

## Use when
- Creating a new GUI screen.
- Deciding where initialization code should run.
- Handling focus, activation, or window closing logic.
- Preventing window close in validation scenarios.

## Main concepts
A CSCS_WPF window can be started by calling `CreateWindow(...)` or by using the `WINFORM` directive.  
Special lifecycle events are triggered automatically during the window lifetime and are mapped to functions named using the pattern:

```internal-script
function <windowName>_OnOpen
function <windowName>_OnInit
function <windowName>_OnActivated
function <windowName>_OnStart
function <windowName>_OnDisplay
function <windowName>_OnDeactivated
function <windowName>_OnClosing
function <windowName>_OnClose
function <windowName>_OnUnload
```

## Entry points

### `CreateWindow(...)`
Use `CreateWindow(...)` when the window is created explicitly from script code.

```internal-script
CreateWindow(strTrim(tpath()) + "testSpecialWindow.xaml");
```

### `WINFORM`
Use `WINFORM` when the window is declared as a form entry directive.

## Important rule
`WINFORM` and `MAIMENU` are special directives and are allowed outside event functions.  
Application logic should normally be placed inside functions and event handlers, not as free-standing script statements.

## Event naming rules
Window lifecycle event functions are named as:

```text
<windowName>On<EventName>
```

Example for a window named `testSpecialWindow`:

```internal-script
function testSpecialWindow_OnOpen
function testSpecialWindow_OnInit
function testSpecialWindow_OnActivated
function testSpecialWindow_OnStart
function testSpecialWindow_OnDisplay
function testSpecialWindow_OnDeactivated
function testSpecialWindow_OnClosing
function testSpecialWindow_OnClose
function testSpecialWindow_OnUnload
```

## Event order

| Order | Event | Frequency | Purpose |
|---|---|---|---|
| 1 | `OnOpen` | Once | Window object created, initial setup starts |
| 2 | `OnInit` | Once | Native/window handle is ready |
| 3 | `OnActivated` | Multiple | Window becomes active / gets focus |
| 4 | `OnStart` | Once | Controls are loaded and ready |
| 5 | `OnDisplay` | Once | Window is rendered and visible |
| 6 | `OnDeactivated` | Multiple | Window loses focus |
| 7 | `OnClosing` | Once per close attempt | Last chance to cancel close |
| 8 | `OnClose` | Once | Window is closed |
| 9 | `OnUnload` | Once | Resources should be released |

## Event details

### `OnOpen`
Use for:
- initialize variables,
- set default values,
- prepare data structures.

Do not use it for:
- control manipulation that requires fully loaded widgets.

### `OnInit`
Use for:
- window-level configuration,
- loading settings,
- preparing handle-dependent behavior.

### `OnActivated`
Use for:
- refresh on focus,
- resume paused operations,
- restart timers or background refresh.

This event may fire multiple times.

### `OnStart`
Use for:
- loading data,
- populating controls,
- binding data sources,
- initial UI setup that requires loaded controls.

This is usually the safest place for initial data loading.

### `OnDisplay`
Use for:
- final visual adjustments,
- showing welcome/info messages,
- starting animations or visible effects.

### `OnDeactivated`
Use for:
- saving temporary state,
- pausing operations,
- stopping timers when focus is lost.

This event may fire multiple times.

### `OnClosing`
Use for:
- unsaved changes checks,
- validation before close,
- confirmation dialogs.

This is the only lifecycle event that uses a return value to control the close process.

### `OnClose`
Use for:
- final logging,
- post-close cleanup,
- application exit logic for the main window.

### `OnUnload`
Use for:
- disposing objects,
- freeing resources,
- closing connections,
- final memory cleanup.

## Return value behavior

### `OnClosing`
`OnClosing` can control whether the window is allowed to close.

```internal-script
function testSpecialWindowOnClosing
{
    // Return false => do not cancel closing
    return false;
}
```

```internal-script
function testSpecialWindowOnClosing
{
    // Return true => cancel closing
    return true;
}
```

## Complete example

```internal-script
CreateWindow(strTrim(tpath()) + "testSpecialWindow.xaml");

function testSpecialWindow_OnOpen
{
    Print("1. OPEN - Window created, starting");
}

function testSpecialWindow_OnInit
{
    Print("2. INIT - Window handle ready");
}

function testSpecialWindow_OnActivated
{
    Print("3. ACTIVATED - Window is now active/focused");
}

function testSpecialWindow_OnStart
{
    Print("4. START - Window loaded, controls ready");
}

function testSpecialWindow_OnDisplay
{
    Print("5. DISPLAY - Window visible");
}

function testSpecialWindow_OnDeactivated
{
    Print("6. DEACTIVATED - Window lost focus");
}

function testSpecialWindow_OnClosing
{
    Print("7. CLOSING - About to close");
    return false;
}

function testSpecialWindow_OnClose
{
    Print("8. CLOSE - Window is closed");
}

function testSpecialWindow_OnUnload
{
    Print("9. UNLOAD - Window removed from memory");
}
```

## Typical workflow
1. Create the window with `CreateWindow(...)` or `WINFORM`.
2. Initialize variables in `OnOpen`.
3. Configure the window in `OnInit`.
4. Load and bind data in `OnStart`.
5. Do visual-only finalization in `OnDisplay`.
6. Validate unsaved changes in `OnClosing`.
7. Free resources in `OnUnload`.

## Common mistakes
- Loading control-dependent data too early in `OnOpen`.
- Putting main initialization logic in `OnDisplay` instead of `OnStart`.
- Forgetting that `OnActivated` and `OnDeactivated` can execute multiple times.
- Returning a value from events other than `OnClosing`.
- Assuming free-standing script code should replace event-driven logic.

## AI notes
- Prefer `OnStart` for initial control population and data loading.
- Use `OnDisplay` only for visual or user-facing actions after the window is visible.
- Do not invent lifecycle event names.
- Only `OnClosing` should return a value to control close cancellation.
- If the window name is `myWindow`, event names must start with `myWindowOn...`.
```

