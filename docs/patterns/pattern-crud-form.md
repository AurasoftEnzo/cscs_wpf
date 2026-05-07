

```md
---
title: CRUD Form Pattern
module: patterns
topic: crud-form
applies_to: CSCS_WPF
version: 1
---

# CRUD Form Pattern

## Purpose
This document describes a standard CRUD-style form pattern for CSCS_WPF using:
- a window lifecycle,
- input controls,
- optional DataGrid row selection,
- Btrieve table access,
- save/delete actions.

## Use when
- Creating a maintenance form.
- Editing one record at a time.
- Loading a selected record from a table.
- Saving or deleting the current record.

## Main concepts
A typical CRUD form in CSCS_WPF combines:
- `CreateWindow(...)`,
- `OnStart` for initial load,
- `EnterBox` / `NumericBox` for editing,
- Btrieve functions such as `OPENV`, `FINDV`, `RCNGet`, `RCNSet`, `SAVE`, `DEL`, `FLERR`,
- button click event handlers,
- optional `OnClosing` validation.

## Example scenario
A form edits invoice line data:
- code,
- description,
- quantity,
- price.

## Variable definitions

```internal-script
DEFINE invoiceHndl TYPE I;

DEFINE itemCode TYPE A SIZE 20;
DEFINE itemDesc TYPE A SIZE 50;
DEFINE itemQty TYPE N SIZE 12 DEC 2;
DEFINE itemPrice TYPE N SIZE 12 DEC 2;

DEFINE currentId TYPE R;
DEFINE hasChanges TYPE L;
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
    Title="InvoiceLineForm"
    Height="420"
    Width="700"
    WindowStartupLocation="CenterScreen">

    <Grid>
        <wcl:EnterBox
            Name="ebCode"
            FieldName="itemCode"
            Size="20"
            ButtonSize="0"
            Margin="20,20,0,0"
            Width="250"
            Height="40"
            HorizontalAlignment="Left"
            VerticalAlignment="Top" />

        <wcl:EnterBox
            Name="ebDesc"
            FieldName="itemDesc"
            Size="50"
            ButtonSize="0"
            Margin="20,80,0,0"
            Width="350"
            Height="40"
            HorizontalAlignment="Left"
            VerticalAlignment="Top" />

        <wcl:NumericBox
            Name="nbQty"
            FieldName="itemQty"
            Size="12"
            Dec="2"
            MinValue="0"
            MaxValue="999999"
            Thousands="True"
            ButtonSize="0"
            Margin="20,140,0,0"
            Width="180"
            Height="40"
            HorizontalAlignment="Left"
            VerticalAlignment="Top" />

        <wcl:NumericBox
            Name="nbPrice"
            FieldName="itemPrice"
            Size="12"
            Dec="2"
            MinValue="0"
            MaxValue="999999999"
            Thousands="True"
            ButtonSize="0"
            Margin="220,140,0,0"
            Width="180"
            Height="40"
            HorizontalAlignment="Left"
            VerticalAlignment="Top" />

        <Button Name="btnLoad"
                Content="Load"
                Width="100"
                Height="35"
                Margin="20,220,0,0"
                HorizontalAlignment="Left"
                VerticalAlignment="Top" />

        <Button Name="btnSave"
                Content="Save"
                Width="100"
                Height="35"
                Margin="140,220,0,0"
                HorizontalAlignment="Left"
                VerticalAlignment="Top" />

        <Button Name="btnDelete"
                Content="Delete"
                Width="100"
                Height="35"
                Margin="260,220,0,0"
                HorizontalAlignment="Left"
                VerticalAlignment="Top" />
    </Grid>
</Window>
```

## Script example

```internal-script
DEFINE invoiceHndl TYPE I;

DEFINE itemCode TYPE A SIZE 20;
DEFINE itemDesc TYPE A SIZE 50;
DEFINE itemQty TYPE N SIZE 12 DEC 2;
DEFINE itemPrice TYPE N SIZE 12 DEC 2;

DEFINE currentId TYPE R;
DEFINE hasChanges TYPE L;

CreateWindow(strTrim(tpath()) + "invoiceLineForm.xaml");

function invoiceLineFormOnOpen
{
    hasChanges = false;
    currentId = 0;
}

function invoiceLineForm_OnStart
{
    invoiceHndl = OPENV("invoiceLines");
    if (FLERR(invoiceHndl) != 0)
    {
        MessageBox("OPENV failed");
    }
}

function btnLoad@clicked
{
    FINDV(invoiceHndl, "f", 0);
    if (FLERR(invoiceHndl) == 0)
    {
        currentId = RCNGet(invoiceHndl);
        hasChanges = false;
    }
    else
    {
        MessageBox("Record not found");
    }
}

function btnSave@clicked
{
    SAVE(invoiceHndl, true, true);
    if (FLERR(invoiceHndl) == 0)
    {
        currentId = RCNGet(invoiceHndl);
        hasChanges = false;
        MessageBox("Saved");
    }
    else
    {
        MessageBox("Save failed");
    }
}

function btnDelete@clicked
{
    if (currentId > 0)
    {
        DEL(invoiceHndl, true);
        if (FLERR(invoiceHndl) == 0)
        {
            currentId = 0;
            itemCode = "";
            itemDesc = "";
            itemQty = 0;
            itemPrice = 0;
            hasChanges = false;
            MessageBox("Deleted");
        }
        else
        {
            MessageBox("Delete failed");
        }
    }
}

function ebCode@TextChange
{
    hasChanges = true;
}

function ebDesc@TextChange
{
    hasChanges = true;
}

function nbQty@TextChange
{
    hasChanges = true;
}

function nbPrice@TextChange
{
    hasChanges = true;
}

function invoiceLineForm_OnClosing
{
    if (hasChanges)
    {
        result = MessageBox("Unsaved changes exist");
        return true;
    }

    return false;
}

function invoiceLineForm_OnUnload
{
    // Add cleanup here if table closing workflow is used.
}
```

## Optional DataGrid-assisted load pattern
A CRUD form can also be used with a DataGrid selection screen:
1. user selects a row in the grid,
2. script reads selected row or record id,
3. script loads the selected record into the form,
4. user edits and saves.

Useful helper functions in this broader workflow can include:
- `GetSelectedGridRow(...)`,
- `FillBufferFromGridRow(...)`,
- `RCNSet(...)`.

## Typical workflow
1. Open the table in `OnStart`.
2. Load the current record using `FINDV(...)` or `RCNSet(...)`.
3. Let custom controls bind directly to script variables.
4. Mark form dirty when values change.
5. Save with `SAVE(...)`.
6. Delete with `DEL(...)`.
7. Block closing if unsaved changes exist.

## Common mistakes
- Forgetting to open the table before calling `FINDV`, `SAVE`, or `DEL`.
- Assuming save/delete succeeded without checking `FLERR(...)`.
- Not tracking unsaved changes.
- Returning the wrong value in `OnClosing`.
- Mixing grid-oriented edit logic directly into form controls without a clear pattern.

## AI notes
- Prefer `OnStart` for opening tables and first record loading.
- Use explicit `DEFINE` declarations for all bound variables.
- Use one consistent save/delete workflow and check `FLERR(...)` after each database operation.
- If a DataGrid is only a selector, keep editing logic in the form, not in the grid.
- Do not invent undocumented CRUD helper functions when `OPENV`, `FINDV`, `RCNGet`, `RCNSet`, `SAVE`, and `DEL` are sufficient.
```
