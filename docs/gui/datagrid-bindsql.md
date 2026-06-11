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


## Create datagrid with look up list
### scrpit.cscs
include(strTrim(tpath()) + "F2ListLib.cscs");

function ceSkladiste@clicked()
{
    F2ListQueryString = "WITH skladista AS("
    + " SELECT DISTINCT nkpr_ln_loc, pkmk_skl_naziv"
    + " FROM  " + trenutnaGodinaBaza + ".[dbo].[nkprglnr] "
    + " LEFT JOIN " + trenutnaGodinaBaza + ".[dbo].[nkprlnnr] ON nkpr_gl_sonum = nkpr_ln_invnm "
    + " LEFT JOIN " + trenutnaGodinaBaza + ".[dbo].[pkmkskla] ON pkmk_skl_code = nkpr_ln_loc"
    + " WHERE nkpr_ln_loc LIKE '{var1}%' AND pkmk_skl_naziv LIKE '%{var2}%'"
    + " AND YEAR(nkpr_gl_orddte) = " + ovagod_h
    + ")"
    + " SELECT ROW_NUMBER() OVER(ORDER BY nkpr_ln_loc ASC) AS RowNumber, nkpr_ln_loc, pkmk_skl_naziv"
    + " FROM skladista ORDER BY RowNumber";

    skladiste = "";
    sifraFieldPointer -> skladiste;
    NazivFieldPointer -> SkladisteNaziv; 
    windowTitle = "F2 lista skladišta";
    headersArray = {"R.br.", "Skladište", "Naziv skladišta"};
    ShowF2List();--> calls fuction from F2ListLib.cscs
    ```
### F2ListLib.cscs

function ShowF2List(){
    ModalWindow(strTrim(tpath()) + "F2List.xaml");
}

function F2List_onDisplay(){
    FormatF2ListDataGrid("dgF2List", windowTitle, headersArray); /// -> gets columns and data 
    searchSifra = &sifraFieldPointer;
    search();
}

### script.xaml

<DataGrid AlternatingRowBackground="LightGray"
                  GridLinesVisibility="None"
                  HorizontalAlignment="Left"
                  VerticalAlignment="Top"
                  Height="320"
                  Width="624"
                  Margin="0,0,0,0"
                  IsReadOnly="true"
                  Foreground="Black"
                  Name="dgF2List"
                  HorizontalScrollBarVisibility="Auto"
                  VerticalScrollBarVisibility="Auto"
                  Background="White"
                  TabIndex="2"
                  IsTabStop="True"
                  SelectionMode="Single"
                  SelectionUnit="FullRow"
                  >
            <DataGrid.Resources>
                <Style TargetType="{x:Type DataGridColumnHeader}">
                    <Setter Property="Background"
                            Value="#FFE7E7CF" />
                    <Setter Property="Foreground"
                            Value="black" />
                </Style>
            </DataGrid.Resources>
            <DataGrid.Columns>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column1Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column2Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column3Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column4Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column5Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column6Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column7Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
                <DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column8Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
				<DataGridTemplateColumn Header=""
                                        >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox Tag="column9Tag" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
            </DataGrid.Columns>
        </DataGrid>

### FormatF2ListDatagrid

Description:
Function for changing “dgF2List” datagrid’s column headers and it’s window’s title.

Usage:
FormatF2ListDataGrid( {windowTitle}, {headersArray});

Argument details:
{windowTitle} -> required string
{headersArray} -> required array

Returns:
Variable.EmptyInstance

called on display in ShowF2List() and SelectedViewList()

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

