title: Widget Events
module: gui
topic: widget-events
applies_to: CSCS_WPF
version: 1
---

# Widget Events

## Purpose
This document describes how widget events are mapped from XAML controls to CSCS_WPF script functions.

## Use when
- Handling button clicks.
- Validating focus entry/exit on text fields.
- Reacting to tab changes, navigator actions, or DataGrid selection.
- Implementing header-click behavior in a DataGrid.

## Naming rules

### Event functions without parameters
```internal-script
function <controlName>@<eventName>
```

### Event functions with parameters
```internal-script
function <controlName>@<eventName>(sender, args)
```

The actual event function name depends on the control `Name` property in XAML.

## Supported events

| Event | Trigger | Notes |
|---|---|---|
| `button@clicked` | Button is clicked | Basic click event |
| `textBox@pre` | TextBox is about to get focus | Return `false` to block focus |
| `textBox@post` | TextBox is about to lose focus | Return `false` to block leaving |
| `tabControl@change` | User changes tab | Return `false` to prevent change |
| `navigator@change` | Navigator button click before navigation | Return `false` to block navigation |
| `navigator@afterChange` | Navigator navigation completed | Fires after successful navigation |
| `dataGrid@move` | Selected row changes in DataGrid | DataGrid must use row selection |
| `dataGrid@select` | Cell is double-clicked in DataGrid | Used for row/cell activation |
| `controlName@header` | User clicks DataGrid column header | Bound to control name inside column template |

## Button click

### XAML
```xml
<Button Name="button1"
        Content="Button text"
        HorizontalAlignment="Left"
        VerticalAlignment="Top"
        Margin="50,50,0,0"
        Width="100"
        Height="50" />
```

### Script
```internal-script
function button1@clicked
{
    MSG("CLICKED");
}
```

## TextBox pre-focus

### Behavior
This event is fired when the `TextBox` is about to get focus.  
If it returns `false`, focus is blocked and the previously focused control keeps focus.

### XAML
```xml
<TextBox Name="textBox1"
         VerticalAlignment="Top"
         HorizontalAlignment="Left"
         Margin="10,50,0,0"
         Height="35"
         Width="100"
         Text="ABC" />
```

### Script
```internal-script
function textBox1@pre
{
    MSG("PRE");
    return true;
}
```

## TextBox post-focus

### Behavior
This event is fired when the `TextBox` is about to lose focus.  
If it returns `false`, focus remains on that `TextBox`.

### Script
```internal-script
function textBox1@post
{
    MSG("POST");
    return true;
}
```

## TabControl change

### Behavior
This event is fired when the user clicks a different tab than the current tab.  
The script can inspect or set tab selection with `SetWidgetOptions(...)` and `GetWidgetOptions(...)`.

### XAML
```xml
<TabControl Name="tabControl">
    <TabItem Header="tab1" Name="tabItem1" />
    <TabItem Header="tab2" Name="tabItem2" />
    <TabItem Header="tab3" Name="tabItem3" />
</TabControl>
```

### Script
```internal-script
DEFINE i TYPE R INIT 0;
DEFINE index TYPE R;

function button1@clicked
{
    i++;
    SetWidgetOptions(tabControl, SelectedIndex, i);
}

function tabControl@change(sender, load)
{
    index = GetWidgetOptions(tabControl, SelectedIndex, int);
    // Index is zero-based.
    return true;
}
```

## Navigator events

### `navigatorchange`
This event runs before navigation is accepted.  
If it returns `false`, navigation does not happen.

```internal-script
function navigator1@change(sender, args)
{
    return true;
}
```

### `navigatorafterChange`
This event runs only after successful navigation.

```internal-script
function navigator1@afterChange(sender, args)
{
    MSG("Navigation completed");
}
```

## DataGrid move

### Behavior
This event runs after the selected row has changed.  
The DataGrid must use row selection.

```internal-script
function grid1@move
{
    dataRow = GetSelectedGridRow(grid1);
    if (Size(dataRow) > 0)
    {
        SetText(labelId, dataRow);
        SetText(labelName, dataRow); [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
    }
}
```

## DataGrid select

### Behavior
This event runs when a cell in the DataGrid is double-clicked.

```internal-script
function grid1@select
{
    MSG("Cell double-clicked");
}
```

## Header click event in DataGrid

Header click event name is based on the control name inside the `DataGridTemplateColumn`.

### XAML
```xml
<DataGridTemplateColumn Header="Attach">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <TextBox Name="tb2" Tag="Attach" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

### Script
```internal-script
function tb2@header
{
    MessageBox("Header clicked");
}
```

## Typical workflow
1. Name each XAML control clearly.
2. Implement event functions using the exact control name.
3. Use `return false` only where event logic supports blocking behavior.
4. Use `GetSelectedGridRow(...)` after `dataGridmove` when row data is needed.
5. Keep widget event handlers short and delegate larger logic into helper functions.

## Common mistakes
- Using the wrong case or wrong control name in event function names.
- Forgetting that some events support `return false` and some do not.
- Handling row-selection logic in `dataGridselect` when `dataGridmove` is a better fit.
- Expecting `dataGridmove` to work correctly when the grid is not using row selection.
- Misnaming header events; they are based on the inner control name, not the grid name.

## AI notes
- Never invent event names that are not documented.
- Match event function names exactly to the XAML `Name`.
- Use `textBoxpre` and `textBoxpost` for validation boundaries.
- Use `dataGridmove` for row selection updates and `dataGridselect` for double-click actions.
- For header click logic, derive the event from the inner control name in the column template.
```
