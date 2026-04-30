This document should explain to the AI how EnterBox is declared in XAML, how it binds to a variable, and what events are allowed.WpfCSCS-UPUTE-3.docx
text
---
title: EnterBox Control
module: gui
topic: custom-control
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# EnterBox Control

## Purpose

'EnterBox' is a custom WPF control for entering text values. It consists of a text section and an optional button.

The control is intended for the input and validation of text data, with the ability to link to CSCS_WPF variable and define events.

## Use when

You need a text value.
- the textbox should be linked to the script variable,
- react to clicking a button, entering the control, exiting the control or changing the text,
- The length and/or casing of the entry should be limited.

## XAML control type

In XAML, a control from a custom control library is used, e.g. 'wcl:ASEnterBox'.

## Attached properties

| Property | Type | Required | Description |
|---|---|---|---|
| `Size` | int | well | Maximum number of characters in a textbox |
| `FieldName` | string | yes | name CSCS_WPF variable for binding |
| `Text` | string | well | Initial value on loading |
| `Name` | string | yes | control name, used for function names |
| `ButtonSize` | int | well | button width; '0' or omitted means no function button |
| `KeyTraps` | string | well | Pairs of keys and functions that are called by pressing the button |
| `Case` | string | well | 'Up', 'Down' or 'Normal' |
| `BorderThickness` | Thickness | no | Border Thickness |
| `BorderBrush` | Brush | no | Border color|
| `CornerRadius` | float | no | Corner radius of textbox and button |
| `ButtonBackground` | Brush | no | Button Background |
| `BackgroundBrush` | Brush | no | Background color |
| `ForegroundBrush` | Brush | no | Foreground color |
| `FontWeight` | FontWeight | no | Font Weight |

## Event naming

If 'Name="enterbox1"', the expected events are:

- `enterbox1@clicked`
- `enterbox1@pre`
- `enterbox1@post`
- `enterbox1@TextChange`

The names must exactly match the name of the control.

## Event meaning

| Event | When fired | Typical use | Return value |
|---|---|---|---|
| `clicked` | Click on the optional button | auxiliary action, lookup, selection | none |
| `before` | when entering the control | Preparation of validation, input requirement | 'true/false' |
| `fast` | out of control | Post-Entry Validation | 'true/false' |
| `TextChange` | when changing the text | Live Reaction to Intake | none |

## XAML example

```xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:wcl="clr-namespace:WpfControlsLibrary; assembly=WpfControlsLibrary"
    mc:Ignorable="d"
    Title="EnterBox Example"
    Height="300"
    Width="500">

    <Grid>
        <wcl:EnterBox
            Name="enterbox1"
            FieldName="myvariable"
            Text="lalalala"
            Size="8"
            ButtonSize="150"
            Case="Normal"
            KeyTraps="F2 enterbox1clicked F3 somethingelse"
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
DEFINE myvariable type a size 8;
myvariable = "12345678";


CreateWindow(strTrim(tpath()) + "enterBox.xaml");

function enterbox1@clicked() {
    MessageBox(myvariable);
    myvariable = "abcdefgh";
}

function somethingelse() {
    MessageBox("Something else!");
}

function enterbox1@pre() {
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

function enterbox1@post() {
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

function enterbox1@TextChange() {
    MessageBox("text changed");
}
```

## Behavioral notes

- 'FieldName' must match an existing variable.
- If the 'ButtonSize' is set to '0', the button is practically non-existent.
- 'Case' determines the automatic conversion of the entered text.
- 'KeyTraps' connects keys to functions in the script.

## Common mistakes

- 'FieldName' does not match the actual variable.
- The name of the event function does not match the 'Name' property of the control.
- The use of non-existent event names.
- Assuming that 'pre' and 'post' have no return value.
- Relying on an implicit variable without 'DEFINE', although explicit definition is recommended.

## AI notes

- Do not invent additional control properties.
- Do not assume WPF events outside of documented CSCS_WPF forms.
- For 'EnterBox' use only documented events 'clicked', 'pre', 'post', 'TextChange'.
- 'FieldName' should be bound to CSCS_WPF variable, not to an arbitrary C# property.
