NumericBox is similar to EnterBox, but has more rules around the numeric format, number of decimals, and value limits. That is why it is useful to document it separately.

---
title: NumericBox Control
module: gui
topic: custom-control
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# NumericBox Control

## Purpose

'NumericBox' is a custom WPF control for entering numeric values. It consists of a numeric textbox and an optional button.

The control supports length limit, number of decimals, range of allowed values, and display of thousands.

## Use when

You need a numerical value.
- Range validation is required,
- need to display thousands after losing focus,
- needs a reaction to a button click or a change in value.

## Attached properties

| Property | Type | Required | Description |
|---|---|---|---|
| `Thousands` | bool | well | Display thousands after losing focus |
| `Size` | int | yes | Maximum number of characters including minus sign and decimal separator |
| `Dec` | int | well | Maximum number of decimals |
| `MinValue` | double | well | Minimum permissible value |
| `MaxValue` | double | well | maximum allowed value |
| `FieldName` | string | yes | name of variable for binding |
| `Text` | string | well | Initial value |
| `Name` | string | yes | Name of the Event Function Mapping Control |
| `ButtonSize` | int | no | button width |
| `KeyTraps` | string | well | Keys & Functions |
| `FontWeight` | FontWeight | no |  Font weigth |
| `BackgroundBrush` | Brush | no | background color |
| `ForegroundBrush` | Brush | no | foreground color |
| `BorderThickness` | Thickness | no | Border thickness |
| `BorderBrush` | Brush | no | Border color |
| `CornerRadius` | float | no | corner radius |
| `ButtonBackground` | Brush | no | Button background |

## Validation behavior

- 'MinValue' and 'MaxValue' are checked when exiting the control.
- If the entered value exceeds the allowed limits, the value can be set to '0'.
- 'Thousands=True' specifies the formatted display after loss of focus.

## Event naming

If 'Name="numbox1"', the expected events are:

- `numbox1@clicked`
- `numbox1@pre`
- `numbox1@post`
- `numbox1@TextChange`

## Event meaning

| Event | When fired | Typical use | Return value |
|---|---|---|---|
| `clicked` | Click the button | auxiliary numerical action | none |
| `before` | Entry into control | Preparation or prohibition of entry | 'true/false' |
| `fast` | Out of Control | Numerical Value Validation | 'true/false' |
| `TextChange` | Change Text | Live Input Processing | none |

## XAML example

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:wcl="clr-namespace:WpfControlsLibrary; assembly=WpfControlsLibrary"
    mc:Ignorable="d"
    Title="NumericBox Example"
    Height="300"
    Width="500">

    <Grid>
        <wcl:NumericBox
            Name="numbox1"
            FieldName="variable1"
            Text="123353.22"
            Size="9"
            Dec="2"
            MinValue="-100"
            MaxValue="100"
            ButtonSize="150"
            Thousands="True"
            KeyTraps="F2 numbox1clicked F3 somethingelse"
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
DEFINE variable1 type n size 9 dec 2;
variable1 = 91919;

CreateWindow(strTrim(tpath()) + "numericBox.xaml");

function numbox1@clicked() {
    MessageBox(variable1);
    variable1 = 55555;
}

function somethingelse() {
    MessageBox("Something else!");
}

function numbox1@pre() {
    a = 2;
    MessageBox("PRE");
    MessageBox(a);

    if (a % 2 == 0) {
        return false;
    }
    else {
        return true;
    }
}

function numbox1@post() {
    b = 2;
    MessageBox("POST");
    MessageBox(b);

    if (b % 2 == 0) {
        return false;
    }
    else {
        return true;
    }
}

function numbox1@TextChange() {
    MessageBox("textChanged");
}
```

## Behavioral notes

- 'FieldName' must refer to a numeric variable.
- 'Size' and 'Dec' together define the allowed format.
- 'Thousands' affects the view, but does not change the business logic of the value.
- 'MinValue' and 'MaxValue' are used to limit the range.
- The button is optional and tied to 'ButtonSize'.

## Common mistakes

- Binding to a variable of the wrong type.
- Inconsistency between 'Size', 'Dec' and expected business range.
- Assuming that validation is done immediately at each character, although limits are typically checked at the exit of the control.
- Wrong name of the event handler.
- Inventing additional attached properties.

## AI notes

- For numeric input, prefer 'NumericBox' instead of 'EnterBox'.
- Do not use undocumented property names.
- Do not assume that 'Text' carries a final validated value without 'post' logic.
- Use 'FieldName' + 'DEFINE' as the standard form.
