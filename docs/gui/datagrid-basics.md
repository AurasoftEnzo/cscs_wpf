title: DataGrid Basics with Arrays
module: gui
topic: datagrid-arrays
applies_to: CSCS_WPF
version: 1
---

# DataGrid Basics with Arrays

## Purpose
This document describes how to display array-based data in a CSCS_WPF DataGrid.

## Use when
- Showing in-memory arrays in a grid.
- Building read-only list views.
- Selecting a row and copying values into labels or controls.
- Refreshing the visible row after programmatic changes.

## Main concepts
Array-based DataGrid usage is built around:
- `DisplayArraySetup(...)`
- `DisplayArrayRefresh(...)`
- `DisplayArray(...)`
- `GetSelectedGridRow(...)`

In this mode, the DataGrid columns are mapped to arrays through the `Tag` property of controls inside `DataGridTemplateColumn`.

## Requirements
- All arrays used by the DataGrid must be of equal length.
- The DataGrid should usually be `IsReadOnly="True"` for browsing patterns.
- A line counter variable stores the selected row index.
- Separate variables track active row count and max row count.

## Core functions

### `DisplayArraySetup(gridName, lineCounter, activeElements, maxElements)`
Initializes a DataGrid to show array data.

#### Arguments
| Argument | Meaning |
|---|---|
| `gridName` | Name of the DataGrid control |
| `lineCounter` | Integer variable storing selected row index |
| `activeElements` | Integer variable with number of active rows |
| `maxElements` | Integer variable with max number of rows |

### `DisplayArrayRefresh(gridName)`
Programmatically reselects the row whose index is already stored in the line counter variable.

### `DisplayArray(gridName, option)`
Refreshes or repositions the grid using one of the supported options.

#### Options
| Option | Meaning |
|---|---|
| `UpdateCurrent` | Update selected row from array values |
| `RedisplayfromTop` | Move selection to first row |
| `RedisplayFromEnd` | Move selection to last row |
| `RedisplayActive` | Refresh active rows, keep current selection |

### `GetSelectedGridRow(gridName)`
Returns the selected DataGrid row as an array.

## Minimal variable setup

```internal-script
DEFINE linecntr TYPE R;
DEFINE activeelements TYPE R;
DEFINE maxelements TYPE R;

DEFINE invnmarray TYPE A ARRAY 20;
DEFINE pcodearray TYPE A ARRAY 20;
DEFINE locarray TYPE A ARRAY 20;
DEFINE pqtyarray TYPE A ARRAY 20;
DEFINE pextarray TYPE A ARRAY 20;
```

## Minimal XAML example

```xml
<DataGrid Name="Grid1"
          IsReadOnly="True"
          AutoGenerateColumns="False"
          Width="792"
          Height="182">
    <DataGrid.Columns>
        <DataGridTemplateColumn Header="Invnm">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Name="tb0" Tag="invnmarray" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="Pcode">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Name="tb1" Tag="pcodearray" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="Loc">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Name="tb2" Tag="locarray" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="Pqty">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Name="tb3" Tag="pqtyarray" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>

        <DataGridTemplateColumn Header="Pext">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <TextBox Name="tb4" Tag="pextarray" />
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
    </DataGrid.Columns>
</DataGrid>
```

## Script example

```internal-script
DEFINE linecntr TYPE R;
DEFINE activeelements TYPE R;
DEFINE maxelements TYPE R;

DEFINE invnmarray TYPE A ARRAY 20;
DEFINE pcodearray TYPE A ARRAY 20;
DEFINE locarray TYPE A ARRAY 20;
DEFINE pqtyarray TYPE A ARRAY 20;
DEFINE pextarray TYPE A ARRAY 20;

function testGridOnStart
{
    invnmarray = "INV001";
    pcodearray = "P001";
    locarray = "A1";
    pqtyarray = "5";
    pextarray = "100.00";

    invnmarray = "INV002"; [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
    pcodearray = "P002"; [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
    locarray = "B1"; [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
    pqtyarray = "2"; [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
    pextarray = "40.00"; [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)

    activeelements = 2;
    maxelements = 20;

    DisplayArraySetup(Grid1, linecntr, activeelements, maxelements);
}
```

## Programmatic row reselection

```internal-script
linecntr = 1;
DisplayArrayRefresh(Grid1);
```

## Refresh options example

```internal-script
DisplayArray(Grid1, UpdateCurrent);
DisplayArray(Grid1, RedisplayfromTop);
DisplayArray(Grid1, RedisplayFromEnd);
DisplayArray(Grid1, RedisplayActive);
```

## Reading selected row

```internal-script
function Grid1move
{
    dataRow = GetSelectedGridRow(Grid1);

    if (Size(dataRow) > 0)
    {
        SetText(labelInvnm, dataRow);
        SetText(labelPcode, dataRow); [ibm](https://www.ibm.com/think/insights/ai-code-documentation-benefits-top-tips)
        SetText(labelLoc, dataRow); [gist.github](https://gist.github.com/juanpabloaj/d95233b74203d8a7e586723f14d3fb0e)
        SetText(labelQty, dataRow); [mintlify](https://www.mintlify.com/resources/structure-documentation-AI-human-readers)
        SetText(labelExt, dataRow); [news.ycombinator](https://news.ycombinator.com/item?id=44311217)
    }
}
```
## Exmple how to make datagrid cscs

function KB_RefreshList() {
    if (!chatSqlEnabled) { return; }
    try {
        kbRes = SQL_ListKnowledgeBases();
        kbListCount = Size(kbRes) - 1;
        if (kbListCount < 0) { kbListCount = 0; }
        // Initialize arrays
        for (i = 0; i < kbListCount; i++) {
            row = kbRes[i + 1];
            kbListId[i]       = int(row[0]);
            kbListName[i]     = row[1];
            kbListDocCount[i] = int(row[3]);
            kbListDate[i]     = row[4];
        }
        // Reset counterFld tracker before setup (grid manages it from 0)
        kbLineCntr = 0;
        DisplayArray("datagridKB", "close");
        SetWidgetOptions("lblKBStatus", "Text", "Nema dostupnih baza znanja. Kreirajte novu.");
        if(kbListCount > 0){

            
        DisplayArraySetup("datagridKB", counterFld: kbLineCntr,
                        activeElements: kbListCount, maxElements: 50);
  
        }
        if (kbListCount == 0) {
            SetWidgetOptions("lblKBDetailTitle", "Text", "Nema baza znanja. Kreirajte novu.");
        }
    } catch (ex) {
        MSG("KB_RefreshList error: " + ex);
    }
}
## Example how to make datagrid xaml

<DataGrid Grid.Row="1" Name="datagridKB"
          Background="Transparent"
          IsReadOnly="True"
          AutoGenerateColumns="False"
          HeadersVisibility="Column"
          BorderThickness="0"
          RowHeight="28"
          FontSize="11.5"
          FontFamily="Segoe UI"
          Foreground="#C8D0DA"
          RowBackground="#1E2F3D"
          AlternatingRowBackground="#243444"
          GridLinesVisibility="Horizontal"
          HorizontalGridLinesBrush="#2D4057"
          CanUserAddRows="False"
          CanUserDeleteRows="False"
          CanUserResizeRows="False"
          CanUserResizeColumns="True"
          SelectionMode="Single"
          HorizontalScrollBarVisibility="Disabled"
          VerticalScrollBarVisibility="Auto">
    <DataGrid.Resources>
        <Style TargetType="{x:Type DataGridColumnHeader}">
            <Setter Property="Background" Value="#1A2535"/>
            <Setter Property="Foreground" Value="#E67E22"/>
            <Setter Property="FontWeight" Value="SemiBold"/>
            <Setter Property="FontSize" Value="11"/>
            <Setter Property="BorderBrush" Value="#2D4057"/>
        </Style>
    </DataGrid.Resources>
    <DataGrid.Columns>
        <DataGridTemplateColumn Header="Naziv" Width="*" MinWidth="160">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <wcl:ASGridCell FieldName="kbListName"
                                    Editor="edDefault"
                                    EditLength="0"
                                    IsReadOnly="True"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
        <DataGridTemplateColumn Header="Dok." Width="48" MinWidth="40">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <wcl:ASGridCell FieldName="kbListDocCount"
                                    Editor="edDefault"
                                    EditLength="0"
                                    IsReadOnly="True"
                                    HorizontalContentAlignment="Center"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
        <DataGridTemplateColumn Header="Ažurirano" Width="90" MinWidth="70">
            <DataGridTemplateColumn.CellTemplate>
                <DataTemplate>
                    <wcl:ASGridCell FieldName="kbListDate"
                                    Editor="edDefault"
                                    EditLength="0"
                                    IsReadOnly="True"/>
                </DataTemplate>
            </DataGridTemplateColumn.CellTemplate>
        </DataGridTemplateColumn>
    </DataGrid.Columns>
</DataGrid>

## Typical workflow
1. Define arrays and row counters.
2. Prepare equal-length arrays.
3. Map each grid column to an array using `Tag`.
4. Call `DisplayArraySetup(...)`.
5. Use `GridNameMove` event to react to row changes.
6. Use `DisplayArrayRefresh(...)` or `DisplayArray(...)` when array values change.

## Common mistakes
- Arrays are not equal length.
- `Tag` names do not match actual array variable names.
- Row count variables are not initialized correctly.
- Expecting editable database behavior from array mode.
- Using `DisplayTable(...)` functions in array-based setup.

## AI notes
- In array mode, each grid column usually maps to one array via `Tag`.
- Prefer read-only grid usage unless there is a documented editable pattern.
- Use `GetSelectedGridRow(...)` for label/detail display after row selection.
- Keep `linecntr`, `activeelements`, and `maxelements` explicit and synchronized.
- Do not mix array-grid patterns with SQL-grid patterns in the same example unless clearly documented.
```
