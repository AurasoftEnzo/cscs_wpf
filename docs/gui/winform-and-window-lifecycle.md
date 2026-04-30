This document is especially important because without it, AI will often make mistakes in the order of events, the name of the event handler, and the location of data initialization or UI logic.WpfCSCS-UPUTE-3.docx
text
---
title: WINFORM and Window Lifecycle
module: gui
topic: window-lifecycle
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# WINFORM and Window Lifecycle

## Purpose

This document describes how CSCS_WPF run WPF/XAML windows and in what order window events are invoked.

## Entry via WINFORM

The initial window can be set by the 'WINFORM' directive.

## Example

```internal-script
WINFORM mainWindow;---> mainWindow.xmal file
```

If there is more than one 'WINFORM' directive, the first window listed is considered the initial window. Other windows can be used for validation or auxiliary scenarios.

## Alternative entry

Instead of 'WINFORM', the initial window can also be opened with the 'CreateWindow(...)`.

## Events overview

Each window has its own events. An event handler is defined as a function whose name is derived from:

'windowName + eventName'

Examples:
- `mainWindowOnOpen`
- `mainWindowOnInit`
- `mainWindowOnStart`
- `mainWindowOnDisplay`

## Event signatures

There are two forms:

### Without parameters

```internal-script
function mainWindow_OnStart() {
    MessageBox("started");
}
```

### With parameters

```internal-script
function mainWindow_OnStart(sender, args) {
    MessageBox("started");
}
```

## Window lifecycle order

A typical sequence of events is:

1. `OnOpen`
2. `OnInit`
3. `OnActivated`
4. `OnStart`
5. `OnDisplay`
6. `OnDeactivated`
7. `OnClosing`
8. `OnClose`
9. `OnUnload`

## Event meaning

| Event | Order | Frequency | Purpose |
|---|---|---|---|
| 'OnOpen' | 1 | once | window object created |
| 'OnInit' | 2 | once | Native Window Handle is ready |
| 'OnActivated' | 3 | multiple | Window Gets Focus |
| 'OnStart' | 4 | once | Controls are loaded and ready |
| 'OnDisplay' | 5 | once | window is displayed to the user |
| `OnDeactivated` | after display | multiple | prozor gubi fokus |
| 'OnClosing' | before close | once | Last Chance to Cancel Lockdown |
| 'OnClose' | after closing | once | the window is closed |
| 'OnUnload' | last | once | Cleanup & Resource Release |

## Recommended usage by event

### UnOpen

Use for:
- initialization of variables,
- setting the initial values,
- preparation of auxiliary structures.

```internal-script
function mainWindow_OnOpen() {
    MessageBox("window object created");
}
```

### OnInit

Use for:
- setting the properties of the window,
- loading application settings,
- Configuration of handle-dependent logic.

```internal-script
function mainWindow_OnInit() {
    MessageBox("window handle ready");
}
```

### OnStart

Use for:
- data loading,
- initialization of controls,
- binding and setup of the user interface.

```internal-script
function mainWindow_OnStart() {
    MessageBox("controls loaded");
}
```

### OnDisplay

Use for:
- display messages to the user,
- final visual adjustments,
- actions that only make sense when the AI is visible.

```internal-script
function mainWindow_OnDisplay() {
    MessageBox("window visible");
}
```

### OnClosing

This event is special because it can return a value.

- 'true' can cancel the closure,
- 'false' allows closure.

```internal-script
function mainWindow_OnClosing() {
    MessageBox("about to close");
    return false;
}
```

Here's an example of how to prevent closure:

```internal-script
function mainWindow_OnClosing() {
    MessageBox("cannot close yet");
    return true;
}
```

### OnClose

Use for:
- log recording,
- final preparation of the situation,
- Final business actions after closing.

### OnUnload

Use for:
- release of resources,
Close the connection.
- Cleanup of objects and memory.

## Complete example

```internal-script
WINFORM testSpecialWindow.xaml;

function testSpecialWindow_OnOpen() {
    Print("1. OPEN - Window created");
}

function testSpecialWindow_OnInit() {
    Print("2. INIT - Handle ready");
}

function testSpecialWindow_OnActivated() {
    Print("3. ACTIVATED - Window active");
}

function testSpecialWindow_OnStart() {
    Print("4. START - Controls ready");
}

function testSpecialWindow_OnDisplay() {
    Print("5. DISPLAY - Window visible");
}

function testSpecialWindow_OnDeactivated() {
    Print("6. DEACTIVATED - Window lost focus");
}

function testSpecialWindow_OnClosing() {
    Print("7. CLOSING - About to close");
    return false;
}

function testSpecialWindow_OnClose() {
    Print("8. CLOSE - Window closed");
}

function testSpecialWindow_OnUnload() {
    Print("9. UNLOAD - Cleanup");
}
```

## Modal window note

In modal windows, the owner window can be disabled at 'OnStart' and re-enabled at 'OnUnload'.

## Common mistakes

- Using the wrong name of the event function.
- Initializing controls too early, before 'OnStart'.
- Display of UI messages in 'OnOpen' instead of 'OnDisplay'.
- Forget that 'OnClosing' uses a return value.
- Assume that all events are executed only once.

## AI notes

- Do not invent events that are not documented.
- To initialize data, prefer 'OnStart'.
- For the final visual steps, use 'OnDisplay'.
- To protect against closing, use 'OnClosing' with a return value.
- The name of the event handler must match the window name from the XAML/project logic.
