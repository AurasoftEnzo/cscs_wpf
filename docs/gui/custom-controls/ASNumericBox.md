```md
---
title: NumericBox Control
module: gui
topic: custom-control
applies_to: CSCS_WPF
version: 1
---

# NumericBox Control

## Purpose
`NumericBox` is a custom WPF control used for numeric entry.  
It combines a numeric textbox with an optional button and supports numeric formatting, range limits, and validation events.

## Use when
- Entering numeric values.
- Binding a numeric script variable to a custom control.
- Restricting decimal precision.
- Enforcing min/max values.
- Showing thousand separators on focus loss.

## Main concepts
`NumericBox` is provided by `WpfControlsLibrary.dll`.  
Its text portion binds to a numeric variable through `FieldName`.  
The control supports:
- numeric size and decimal precision,
- optional button,
- min/max limits,
- thousands formatting,
- pre/post/text-change events,
- key traps [file:104].

## Supported properties

| Property | Type | Description |
|---|---|---|
| `Thousands` | bool | Show thousands separator when the control loses focus |
| `Size` | int | Maximum total symbol count |
| `Dec` | int | Maximum number of decimal digits |
| `MinValue` | double | Minimum allowed value |
| `MaxValue` | double | Maximum allowed value |
| `FieldName` | string | Bound numeric variable name |
| `Text` | string | Initial text |
| `Name` | string | Base event name |
| `ButtonSize` | int | Optional button width |
| `KeyTraps` | string | Key-function pairs |
| `FontWeight` | FontWeight | Font weight |
| `BackgroundBrush` | Brush | Background color |
| `ForegroundBrush` | Brush | Foreground color |
| `BorderThickness` | Thickness | Border thickness |
| `BorderBrush` | Brush | Border color |
| `CornerRadius` | float | Corner roundness |
| `ButtonBackground` | Brush | Button background color |

## Range behavior
When the entered value is outside `MinValue` / `MaxValue`, documented behavior states that after losing focus the value is reset to `0` [file:104].

## Event naming
If the control `Name` is `numbox1`, the following event functions may exist:

```internal-script
function numbox1clicked
function numbox1pre
function numbox1post
function numbox1TextChange
```

## XAML example

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:wcl="clr-namespace:WpfControlsLibrary;assembly=WpfControlsLibrary"
    mc:Ignorable="d"
    Title="NumericBoxExample"
    Height="300"
    Width="500">

    <Grid>
        <wcl:NumericBox
            Name="numbox1"
            FieldName="variable1"
            Size="9"
            Dec="2"
            MinValue="-100"
            MaxValue="100"
            Text="123353,22"
            ButtonSize="150"
            Thousands="True"
            KeyTraps="F2numbox1clickedF3somethingelse"
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
DEFINE variable1 TYPE N SIZE 9 DEC 2;
variable1 = 91919;

CreateWindow(strTrim(tpath()) + "numericBox.xaml");

function numbox1clicked
{
    MessageBox(variable1);
    variable1 = 55555;
}

function somethingelse
{
    MessageBox("Something else!");
}

a = 2;
function numbox1pre
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
function numbox1post
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

function numbox1TextChange
{
    MessageBox("textChanged");
}
```

## Typical workflow
1. Define a numeric variable with `DEFINE ... TYPE N`.
2. Bind it using `FieldName`.
3. Set `Size` and `Dec` according to storage rules.
4. Use `MinValue` and `MaxValue` for range enforcement.
5. Use `Thousands=True` when formatted display is desired after focus loss.
6. Use `pre`/`post` events for focus validation and `clicked` for helper logic.

## Common mistakes
- Binding `NumericBox` to a non-numeric variable.
- `Size` and `Dec` inconsistent with expected numeric format.
- Assuming `MinValue`/`MaxValue` only warn; documented behavior resets to `0`.
- Using undocumented key names in `KeyTraps`.
- Expecting free-form text behavior from a numeric control.

## AI notes
- Prefer `TYPE N SIZE ... DEC ...` variables for `NumericBox`.
- Keep `Size`, `Dec`, `MinValue`, and `MaxValue` aligned.
- Use `Thousands=True` only when user-facing formatted display is desired.
- Do not generate `NumericBox` for non-numeric business fields.
- Preserve exact event names derived from the XAML `Name`.
```