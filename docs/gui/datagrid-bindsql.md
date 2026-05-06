title: DataGrid with SQL Sources
module: gui
topic: datagrid-sql
applies_to: CSCS_WPF
version: 1
---

# DataGrid with SQL Sources

## Purpose
This document describes how to bind SQL data to a CSCS_WPF DataGrid using SQL/grid helper functions.

## Use when
- Displaying rows from a SQL table or SQL query in a DataGrid.
- Building read-only grids from SQL results.
- Building editable grids over a table.
- Switching between grid select mode and edit mode.

## Main concepts
SQL/DataGrid usage in CSCS_WPF is based on:
- `BindSql(...)`
- `NewBindSql(...)`
- `DisplayTableSetup(...)`
- `DisplayTableSetupWhere(...)`
- `DisplayTable(...)`
- `DataGrid(...)`
- `SetWidgetOptions(grid, SelectMode/EditMode)`

Different patterns are used for:
- read-only SQL result display,
- editable table display,
- filtered display,
- row add/insert/delete actions.

## SQL type limitations
Documented accepted SQL column types for grid scenarios include:
- `int`
- `nvarchar`
- `float`
- `bit`
- `date`

A documented limitation is that `time` is not accepted in some grid scenarios.  
In some `NewBindSql(...)` scenarios, the `id` column should be renamed, for example to `id2`.

## Pattern A: `NewBindSql(...)` for read-only SQL result

### Purpose
Use `NewBindSql(...)` to execute a query and directly display the result in a DataGrid.

### Example script
```internal-script
DEFINE rowCount TYPE R;

function testdatagridnewbindsqlOnDisplay
{
    rowCount = NewBindSql(Grid1,
        "select id as id2, name, gender, amount, bitcolumn, datein from tblstudents2");

    MessageBox(rowCount);
}
```

### Selected row retrieval
```internal-script
function Grid1Move
{
    dataRow = GetSelectedGridRow(Grid1);

    if (Size(dataRow) > 0)
    {
        SetText(labelid, dataRow);
        SetText(labelname, dataRow); [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
        SetText(labelgender, dataRow); [gist.github](https://gist.github.com/juanpabloaj/d95233b74203d8a7e586723f14d3fb0e)
        SetText(labelamount, dataRow); [mintlify](https://www.mintlify.com/resources/structure-documentation-AI-human-readers)
        SetText(labelbitcolumn, dataRow); [news.ycombinator](https://news.ycombinator.com/item?id=44311217)
        SetText(labeldatein, dataRow); [docuwriter](https://www.docuwriter.ai/posts/code-documentation-best-practices-building-better-software)
    }
}
```

### XAML column mapping
For this pattern, the visible columns are typically defined in XAML and mapped by `Tag`.

```xml
<DataGrid Name="Grid1"
          AutoGenerateColumns="False"
          Width="380"
          Height="225">
    <DataGrid.Columns>
        <DataGridTemplateColumn Header="head0">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Tag="id2" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="head1">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Tag="name" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="head2">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Tag="gender" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
    </DataGrid.Columns>
</DataGrid>
```

## Pattern B: `DisplayTableSetup(...)` for editable table display

### Purpose
Use table-display setup functions when the grid is directly working with database table records and editing is expected.

### Notes
- This pattern is used for editable grids.
- Some examples show add/edit/delete workflows.
- Joined tables have limitations; editing is intended for the main table, not the joined table.

### XAML example
Editable table grids may use `FieldName` instead of `Tag`:

```xml
<DataGridTemplateColumn Header="Invoice Number">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <wcl:ASGridCell FieldName="invoiceNumber" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>

<DataGridTemplateColumn Header="Amount">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <wcl:ASGridCell FieldName="amount" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>

<DataGridTemplateColumn Header="Date Column">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <wcl:ASGridCell FieldName="dateColumn" EditLength="10" />
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>
```

## Grid display refresh options

### `DisplayTable(gridName, option)`
Supported options:
- `UpdateCurrent`
- `RedisplayfromTop`
- `RedisplayFromEnd`
- `RedisplayActive`

Example:
```internal-script
DisplayTable(Grid1, RedisplayActive);
```

## Add/insert/delete row actions

### `DataGrid(gridName, option)`
Supported options:
- `AddRow`
- `InsertRow`
- `DeleteRow`
- `Clear` (documented as not implemented)

Examples:
```internal-script
DataGrid(Grid1, AddRow);
DataGrid(Grid1, InsertRow);
DataGrid(Grid1, DeleteRow);
```

## Grid mode switching

DataGrid supports:
- `SelectMode`
- `EditMode`

Example:
```internal-script
SetWidgetOptions(Grid1, SelectMode);
SetWidgetOptions(Grid1, EditMode);
```

In select mode, row selection is active.  
In edit mode, grid cells can be edited.

## Pattern C: SQL query into arrays + grid

Another documented pattern is:
1. Fetch SQL with `SqlQuery(...)`,
2. copy result values into arrays,
3. show arrays in the DataGrid.

This is useful when:
- the displayed data needs preprocessing,
- arrays are also reused elsewhere,
- the grid should remain read-only and controlled by script logic.

## Typical workflow for read-only SQL grid
1. Define DataGrid columns in XAML.
2. Use `NewBindSql(...)` or `BindSql(...)` to load query results.
3. Rename `id` to `id2` if required by the pattern.
4. Use `GridNameMove` to read selected row values.
5. Keep the grid in select mode unless editing is required.

## Typical workflow for editable grid
1. Configure DataGrid columns for table fields.
2. Switch grid to `EditMode` when editing is needed.
3. Use `DataGrid(grid, AddRow/InsertRow/DeleteRow)` for row operations.
4. Save/update through the documented table workflow.
5. Refresh grid with `DisplayTable(grid, RedisplayActive)`.

## Common mistakes
- Using `time` columns in a grid pattern that does not support them.
- Forgetting to rename `id` to `id2` in `NewBindSql(...)` scenarios where required.
- Mixing `Tag`-based and `FieldName`-based patterns without a clear reason.
- Expecting joined-table columns to behave like editable main-table columns.
- Using edit mode when a read-only selection grid is intended.
- Assuming `Clear` is implemented for `DataGrid(...)`.

## AI notes
- Choose one grid pattern first: query-result grid, table-edit grid, or array grid.
- For read-only selection screens, prefer `NewBindSql(...)` or query-to-array patterns.
- For editable grids, use documented table-oriented setup and `EditMode`.
- Do not invent unsupported SQL column types for grid binding.
- When examples show `id as id2`, preserve that pattern.
```

