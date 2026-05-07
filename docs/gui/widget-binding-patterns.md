```md
---
title: Widget Binding Patterns
module: gui
topic: binding-patterns
applies_to: CSCS_WPF
version: 1
---

# Widget Binding Patterns

## Purpose
This document summarizes the most common binding and naming patterns used between XAML widgets and CSCS_WPF script variables.

## Use when
- Creating new forms.
- Deciding whether to use `FieldName`, `DataContext`, `Tag`, or `Name`.
- Building DataGrid columns.
- Connecting widgets to script variables and event handlers.

## Main concepts
In CSCS_WPF, widget integration usually happens through:
- `Name` for event binding,
- `FieldName` for custom control variable binding,
- `DataContext` for standard control variable binding,
- `Tag` for DataGrid array/query column mapping [file:104].

## Binding overview

| XAML attribute | Main use | Typical widgets |
|---|---|---|
| `Name` | Event function naming and widget reference | All controls |
| `FieldName` | Binding custom controls to script variables | `EnterBox`, `NumericBox`, `ASGridCell` |
| `DataContext` | Binding standard widgets to variables | `TextBox`, `Label`, `CheckBox`, other simple controls |
| `Tag` | Mapping DataGrid template content to arrays or query column names | `TextBox`, `CheckBox`, date/time controls inside grid templates |

## Pattern 1: `Name`
`Name` is used to:
- reference a control from script functions,
- derive event handler names.

### Example
```xml
<Button Name="buttonUpdater" Content="Updater" />
```

```internal-script
function buttonUpdaterClickedsender, load
{
    SetText(textArea, "Hello, world");
}
```

## Pattern 2: `FieldName`
Use `FieldName` for custom controls that bind directly to a script variable.

### Example
```xml
<wcl:EnterBox
    Name="enterbox1"
    FieldName="customerCode"
    Size="10" />
```

```internal-script
DEFINE customerCode TYPE A SIZE 10;
```

Another example:
```xml
<wcl:NumericBox
    Name="numboxAmount"
    FieldName="invoiceAmount"
    Size="12"
    Dec="2" />
```

```internal-script
DEFINE invoiceAmount TYPE N SIZE 12 DEC 2;
```

## Pattern 3: `DataContext`
Use `DataContext` for simple widget binding in many standard controls.

### Example
```xml
<TextBox Name="tb1"
         DataContext="invoiceNumber"
         Width="60" />
```

```internal-script
DEFINE invoiceNumber TYPE R;
```

Example from labels:
```xml
<Label Name="labelPrice" DataContext="price" />
```

## Pattern 4: `Tag` in DataGrid
Use `Tag` inside `DataGridTemplateColumn.CellTemplate` to map displayed values.

### Array grid example
```xml
<DataGridTemplateColumn Header="Pcode">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <TextBox Name="tb1" Tag="pcodearray" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

### Query/result grid example
```xml
<DataGridTemplateColumn Header="Name">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <TextBox Tag="name" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

## Pattern 5: `FieldName` in editable grid cells
Some editable DataGrid patterns use custom cells with `FieldName`.

### Example
```xml
<DataGridTemplateColumn Header="Amount">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <wcl:ASGridCell FieldName="amount" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

## Choosing the right binding strategy

| Scenario | Prefer |
|---|---|
| Custom text input control | `FieldName` |
| Custom numeric input control | `FieldName` |
| Standard widget bound to one variable | `DataContext` |
| DataGrid with arrays | `Tag` |
| DataGrid with query result columns | `Tag` |
| Editable DataGrid over table fields | `FieldName` |
| Event handler naming | `Name` |

## Naming rules
- Keep `Name` stable and descriptive.
- Match event function names exactly to control names.
- `FieldName` should reference an actually defined script variable.
- `Tag` values in grid templates must match real array names or returned column aliases.
- If a SQL result uses `id as id2`, the XAML `Tag` should also use `id2` [file:104].

## Typical workflow
1. Decide whether the control is standard or custom.
2. Use `FieldName` for custom input controls.
3. Use `DataContext` for simple standard controls.
4. Use `Tag` inside DataGrid templates.
5. Use `Name` for all event-driven controls.

## Common mistakes
- Using `FieldName` on standard controls where the documented pattern expects `DataContext`.
- Using `DataContext` where a custom control expects `FieldName`.
- Mismatch between SQL alias and `Tag`.
- Mismatch between variable name and `FieldName`.
- Assuming `Name` itself binds data; it does not.

## AI notes
- Treat `Name`, `FieldName`, `DataContext`, and `Tag` as different mechanisms with different purposes.
- In generated XAML, do not substitute one binding style for another unless the pattern explicitly supports it.
- Prefer explicit variable definitions that match XAML binding names.
- In DataGrid examples, preserve alias names exactly as exposed by SQL or arrays.
```