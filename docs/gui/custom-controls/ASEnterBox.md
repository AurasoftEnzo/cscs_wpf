title: ASEnterBox Control
module: gui
topic: custom-control
applies_to: CSCS_WPF
version: 1
---

# ASEnterBox Control

## Purpose
`ASEnterBox` is a custom WPF control used for text entry.  
It combines a text input area with an optional button and supports validation events and keyboard traps.

## Use when
- Entering short text values.
- Binding a script variable to a custom text control.
- Requiring optional button action next to a text field.
- Enforcing text case rules.
- Handling F-key shortcuts while the field is focused.

## Main concepts
`ASEnterBox` is provided by `WpfControlsLibrary.dll`.  
It binds its text portion to a script variable using the `FieldName` property.  
The control can define:
- a maximum text length,
- optional button width,
- keyboard traps,
- text case behavior,
- pre/post/text-change events.

## Required namespaces

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:wcl="clr-namespace:WpfControlsLibrary;assembly=WpfControlsLibrary"
    mc:Ignorable="d">
```

## Supported properties

| Property | Type | Description |
|---|---|---|
| `Size` | int | Maximum number of characters allowed |
| `FieldName` | string | Bound script variable name |
| `Text` | string | Initial control text |
| `Name` | string | Base name for event function names |
| `ButtonSize` | int | Width of the optional button; `0` or omitted hides it |
| `KeyTraps` | string | Key-function pairs, e.g. `F2enterbox1clickedF3somethingelse` |
| `Case` | string | `Up`, `Down`, or `Normal` |
| `BorderThickness` | Thickness | Textbox border thickness |
| `BorderBrush` | Brush | Border color |
| `CornerRadius` | float | Corner roundness |
| `ButtonBackground` | Brush | Button background color |
| `BackgroundBrush` | Brush | Textbox background color |
| `ForegroundBrush` | Brush | Textbox foreground color |
| `FontWeight` | FontWeight | Textbox font weight |

## Event naming
If the XAML control `Name` is `enterbox1`, the following event functions may exist:

```internal-script
function enterbox1@clicked
function enterbox1@pre
function enterbox1@post
function enterbox1@TextChange
```

## Event behavior

| Event | When it fires | Notes |
|---|---|---|
| `clicked` | Optional button click | Also can be triggered through key trap |
| `pre` | Before entering the control | Return `false` to block focus |
| `post` | Before leaving the control | Return `false` to block leaving |
| `TextChange` | Text content changed | Use for reactive behavior |

## XAML example

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:wcl="clr-namespace:WpfControlsLibrary;assembly=WpfControlsLibrary"
    mc:Ignorable="d"
    Title="EnterBoxExample"
    Height="300"
    Width="500">

    <Grid>
        <wcl:ASEnterBox
            Name="enterbox1"
            FieldName="myvariable"
            Size="8"
            Text="lalalala"
            ButtonSize="150"
            KeyTraps="F2enterbox1clickedF3somethingelse"
            Case="Normal"
            Height="60"
            Width="400"
            Margin="50,100,0,0"
            HorizontalAlignment="Left"
            VerticalAlignment="Top"
            FontSize="25" />
    </Grid>
</Window>
```

## Script example

```internal-script
DEFINE myvariable TYPE A SIZE 8;
myvariable = "12345678";

CreateWindow(strTrim(tpath()) + "enterBox.xaml");

function enterbox1@clicked
{
    MessageBox(myvariable);
    myvariable = "abcdefgh";
}

function somethingelse
{
    MessageBox("Something else!");
}

a = 2;
function enterbox1@pre
{
    MessageBox("PRE");
    a--;
    if (a % 2 == 0)
    {
        return false;
    }
    else
    {
        return true;
    }
}

b = 2;
function enterbox1@post
{
    MessageBox("POST");
    b--;
    if (b % 2 == 0)
    {
        return false;
    }
    else
    {
        return true;
    }
}

function enterbox1@TextChange
{
    MessageBox("textChanged");
}
```

## Typical workflow
1. Define a string variable with `DEFINE`.
2. Bind it via `FieldName`.
3. Set maximum input size with `Size`.
4. Add `ButtonSize` only if a clickable button is needed.
5. Use `pre` and `post` for validation.
6. Use `TextChange` only for lightweight reactive logic.

## Common mistakes
- `FieldName` does not match an existing script variable.
- `Name` does not match implemented event names.
- `ButtonSize` omitted, but code expects `clicked` behavior from a visible button.
- `Case` value invented instead of using documented values.
- Heavy logic placed in `TextChange`.

## AI notes
- Always define the bound variable explicitly.
- Preserve the exact `Name` → event-function naming pattern.
- If no button is needed, omit `ButtonSize` or set it to `0`.
- Do not invent undocumented EnterBox properties.
- Prefer `pre`/`post` for validation and `clicked` for lookup/helper actions.
```
