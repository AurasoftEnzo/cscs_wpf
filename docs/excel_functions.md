# CSCS Excel Functions Reference

This document describes all Excel-related functions available in the **CSCS WPF scripting language**. These functions provide programmatic creation, reading, and manipulation of Excel (.xlsx) files using the **EPPlus** library (OfficeOpenXml) as the primary engine, with **MiniExcel** used for the `SqlToXlsx` function and **Spire.XLS** for post-processing.

---

## Table of Contents

1. [Overview](#overview)
2. [File Operations](#file-operations)
3. [Sheet (Worksheet) Operations](#sheet-worksheet-operations)
4. [Cell Writing](#cell-writing)
5. [Cell Reading](#cell-reading)
6. [Cell Utilities](#cell-utilities)
7. [Search / Find Operations](#search--find-operations)
8. [Copy Operations](#copy-operations)
9. [Row / Column Operations](#row--column-operations)
10. [Formatting & Styling](#formatting--styling)
11. [Table & Named Range Operations](#table--named-range-operations)
12. [Header / Footer](#header--footer)
13. [Pivot Table](#pivot-table)
14. [SQL to XLSX](#sql-to-xlsx)
15. [Error Handling](#error-handling)
16. [Error Codes Reference](#error-codes-reference)

---

## Overview

All Excel functions are registered in the `Excel.Init()` method and are callable from CSCS scripts. The global static object `Excel.document` (an `ExcelPackage` instance) holds the currently opened workbook. Functions require a document to be open before any sheet or cell operations can be performed.

**Internal library used:** EPPlus (OfficeOpenXml)

### ⚠️ Critical Pattern: `XFileNew` → `XFileOpen`

`XFileNew` creates the file on disk and saves a blank workbook, but it does **not** set the global `Excel.document` variable. You **must** call `XFileOpen` immediately after to load it into memory:

```csharp
XFileNew("file.xlsx");
XFileOpen("file.xlsx");   // ← Required! Sets Excel.document
// ... cell operations ...
XFileSave();
```

Without `XFileOpen`, all subsequent cell/sheet operations will silently fail.

---

## File Operations

### `XFileNew(fileName)`
Creates a new Excel workbook file with a default worksheet named `"Sheet1"`. If the file already exists, it is deleted and recreated.

| Parameter | Type | Description |
|-----------|------|-------------|
| `fileName` | string | Full path to the new .xlsx file |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFileNew("C:\Reports\report.xlsx");
```

---

### `XFileOpen(fileName)`
Opens an existing Excel workbook into the global `Excel.document` object.

| Parameter | Type | Description |
|-----------|------|-------------|
| `fileName` | string | Full path to an existing .xlsx file |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `2` if file does not exist.

**Example:**
```
XFileOpen("C:\Reports\report.xlsx");
```

---

### `XFileSave()`
Saves the currently open workbook to its current path (same as the file used in `XFileNew` or `XFileOpen`).

**Parameters:** None

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFileSave();
```

---

### `XFileSaveAs(fileNamePath)`
Saves the currently open workbook to a new file path.

| Parameter | Type | Description |
|-----------|------|-------------|
| `fileNamePath` | string | Full destination path for the .xlsx file |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFileSaveAs("C:\Reports\backup.xlsx");
```

---

### `XFileDelete(fileName)`
Deletes an Excel file from disk.

| Parameter | Type | Description |
|-----------|------|-------------|
| `fileName` | string | Full path to the .xlsx file to delete |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `2` if file does not exist.

**Example:**
```
XFileDelete("C:\Reports\temp.xlsx");
```

---

## Sheet (Worksheet) Operations

### `XSheetAdd(sheetName)`
Adds a new worksheet to the current workbook.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetName` | string | Name for the new worksheet |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XSheetAdd("DataSheet");
```

---

### `XSheetDelete(sheetIndex)`
Deletes a worksheet by its 1-based index.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetIndex` | int | 1-based index of the worksheet to delete |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XSheetDelete(2);
```

---

### `XSheetCount()`
Returns the number of worksheets in the current workbook.

**Parameters:** None

**Returns:** Integer count of worksheets.

**Example:**
```
count = XSheetCount();
```

---

### `XSheetClear(sheetIndex)`
Clears all content from a worksheet by deleting it and recreating a new one with the same name.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetIndex` | int | 1-based index of the worksheet to clear |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `3` if sheet index does not exist.

**Example:**
```
XSheetClear(1);
```

---

### `XSheetRename(sheetIndex, newName)`
Renames an existing worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetIndex` | int | 1-based index of the worksheet |
| `newName` | string | New name for the worksheet |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XSheetRename(1, "Summary");
```

---

## Cell Writing

### `XCellWriteString(sheet, column, row, value)`
Writes a string value into a cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `value` | string | String value to write |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellWriteString(1, 1, 1, "Hello");
```

---

### `XCellWriteInteger(sheet, column, row, value)`
Writes an integer value into a cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `value` | int | Integer value to write |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellWriteInteger(1, 2, 1, 42);
```

---

### `XCellWriteDouble(sheet, column, row, value, [format])`
Writes a double/decimal value into a cell with optional number format.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `value` | double | Numeric value to write |
| `format` | string (optional) | Number format string (e.g., `"#,##0.00"`, `"General"`) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellWriteDouble(1, 3, 1, 1234.56, "#,##0.00");
```

---

### `XCellWriteBoolean(sheet, column, row, value)`
Writes a boolean value into a cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `value` | bool | Boolean value (`true`/`false`) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellWriteBoolean(1, 4, 1, true);
```

---

### `XCellWriteTime(sheet, column, row, timeVariable)`
Writes a time value from a CSCS time variable into a cell (formatted as `hh:mm:ss`).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `timeVariable` | string | Name of a CSCS time/date variable |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `4` if variable not found.

**Example:**
```
timeVar = Time(14, 30, 0);
XCellWriteTime(1, 5, 1, "timeVar");
```

---

### `XCellWriteDate(sheet, column, row, dateVariable)`
Writes a date value from a CSCS date variable into a cell (formatted as `dd-mm-yyyy`).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `dateVariable` | string | Name of a CSCS date variable |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `4` if variable not found.

**Example:**
```
dateVar = Date(2025, 12, 31);
XCellWriteDate(1, 6, 1, "dateVar");
```

---

### `XCellWriteDateTime(sheet, column, row, dateVariable, timeVariable)`
Writes a combined date+time value from two CSCS variables into a cell (formatted as `dd-mm-yyyy hh:mm:ss`).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `dateVariable` | string | Name of a CSCS date variable |
| `timeVariable` | string | Name of a CSCS time variable |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `4` if either variable not found.

**Example:**
```
dateVar = Date(2025, 12, 31);
timeVar = Time(14, 30, 0);
XCellWriteDateTime(1, 7, 1, "dateVar", "timeVar");
```

---

### `XCellWriteFormula(sheet, column, row, formula)`
Writes an Excel formula into a cell. Formulas are written as strings and evaluated by Excel when the file is opened.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `formula` | string | Excel formula (e.g., `"SUM(A1:A10)"`) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellWriteFormula(1, 8, 1, "SUM(B1:B10)");
```

---

## Cell Reading

### `XCellReadString(sheet, column, row)`
Reads a cell value as a string.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** String value of the cell (empty string on failure).

**Example:**
```
val = XCellReadString(1, 1, 1);
```

---

### `XCellReadInteger(sheet, column, row)`
Reads a cell value as an integer.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Integer value (`0` on failure).

**Example:**
```
val = XCellReadInteger(1, 2, 1);
```

---

### `XCellReadDouble(sheet, column, row)`
Reads a cell value as a double.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Double value (`0.0` on failure).

**Example:**
```
val = XCellReadDouble(1, 3, 1);
```

---

### `XCellReadBoolean(sheet, column, row)`
Reads a cell value as a boolean.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Boolean value (`false` on failure).

**Example:**
```
val = XCellReadBoolean(1, 4, 1);
```

---

### `XCellReadTime(sheet, column, row)`
Reads a cell value as a `TimeSpan` (time).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Time value (default `00:00:00` on failure).

**Example:**
```
val = XCellReadTime(1, 5, 1);
```

---

### `XCellReadDate(sheet, column, row)`
Reads a cell value as a date string in `yyyy-MM-dd` format.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Date string in `"yyyy-MM-dd"` format (returns `"1900-01-01"` on failure).

**Example:**
```
val = XCellReadDate(1, 6, 1);
```

---

### `XCellReadFormula(sheet, column, row)`
Reads the formula string from a cell (not the computed value).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |

**Returns:** Formula string (empty string on failure).

**Example:**
```
formula = XCellReadFormula(1, 8, 1);
```

---

## Cell Utilities

### `XCellEmpty(sheet, column, row, type)`
Clears a cell's content, style, or both.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `row` | int | Row number (1-based) |
| `type` | int | Operation type: `1` = clear all (value + style), `2` = clear value only, `3` = clear style only |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCellEmpty(1, 1, 1, 1);  // Clear both value and style
```

---

## Search / Find Operations

### `XFindSheet(sheetName)`
Finds a worksheet by name and returns its 1-based index.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetName` | string | Name of the worksheet to find |

**Returns:** 1-based index of the worksheet, or `0` if not found.

**Example:**
```
idx = XFindSheet("DataSheet");
```

---

### `XFindColumn(sheet, row, columnName)`
Searches for a column header name in a specific row and returns its column number.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `row` | int | Row number to search in (typically header row) |
| `columnName` | string | Column header text to find |

**Returns:** 1-based column number, or `0` if not found.

**Error codes:** `5` if search returns nothing.

**Example:**
```
col = XFindColumn(1, 1, "Total");
```

---

### `XFindRow(sheet, column, rowName)`
Searches for a row label in a specific column and returns its row number.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number to search in |
| `rowName` | string | Row label text to find |

**Returns:** 1-based row number, or `0` if not found.

**Error codes:** `5` if search returns nothing.

**Example:**
```
row = XFindRow(1, 1, "Product A");
```

---

## Copy Operations

### `XCopyCell(sheet, column, row, sheetDest, columnDest, rowDest)`
Copies a single cell to another location (can be across different worksheets).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | Source worksheet index |
| `column` | int | Source column number |
| `row` | int | Source row number |
| `sheetDest` | int | Destination worksheet index |
| `columnDest` | int | Destination column number |
| `rowDest` | int | Destination row number |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCopyCell(1, 1, 1, 1, 5, 1);  // Copy A1 to E1
```

---

### `XCopyRow(sheet, row, numRowsCopy)`
Copies one or more rows downward within the same worksheet (inserts below the source row).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | Worksheet index |
| `row` | int | Source row number to copy from |
| `numRowsCopy` | int | Number of rows to copy |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCopyRow(1, 1, 3);  // Copy row 1 down 3 times (rows 2, 3, 4)
```

---

### `XCopyRowToRow(sheet, row, sheetDest, rowDest)`
Copies an entire row to a specific destination row (can be across different worksheets).

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | Source worksheet index |
| `row` | int | Source row number |
| `sheetDest` | int | Destination worksheet index |
| `rowDest` | int | Destination row number |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCopyRowToRow(1, 1, 2, 1);  // Copy row 1 from Sheet1 to row 1 of Sheet2
```

---

### `XCopyColumn(sheet, column, numColumnsCopy)`
Copies one or more columns to the right within the same worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | Worksheet index |
| `column` | int | Source column number to copy from |
| `numColumnsCopy` | int | Number of columns to copy |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XCopyColumn(1, 1, 2);  // Copy column A two columns to the right (to B and C)
```

---

## Row / Column Operations

### `XLastRow(sheet)`
Returns the last used row number in a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |

**Returns:** Integer — last row number with content, or `0` on failure.

**Example:**
```
lastRow = XLastRow(1);
```

---

### `XLastColumn(sheet)`
Returns the last used column number in a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |

**Returns:** Integer — last column number with content, or `0` on failure.

**Example:**
```
lastCol = XLastColumn(1);
```

---

### `XLastAddress(sheet)`
Returns the address (e.g., `"E15"`) of the last used cell in a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |

**Returns:** String address of the last cell, or empty string on failure.

**Example:**
```
addr = XLastAddress(1);
```

---

### `XInsertRows(sheet, row, numOfRows)`
Inserts blank rows at a specified position.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `row` | int | Row number where insertion starts |
| `numOfRows` | int | Number of rows to insert |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XInsertRows(1, 3, 5);  // Insert 5 rows starting at row 3
```

---

### `XDeleteRow(sheet, row)`
Deletes a single row from a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `row` | int | Row number to delete |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XDeleteRow(1, 5);
```

---

### `XColumnsAutoFit(sheet)`
Auto-fits all columns in a worksheet to their content width.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XColumnsAutoFit(1);
```

---

### `XFormatColumn(sheet, column, format)`
Applies a number format to an entire column.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number (1-based) |
| `format` | string | Excel number format string (e.g., `"#,##0.00"`, `"dd-mm-yyyy"`). Pass empty string to clear. |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFormatColumn(1, 3, "#,##0.00");
```

---

## Formatting & Styling

### `XFontName(sheet, column, row, fontName)`
Sets the font name (typeface) for a specific cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `fontName` | string | Font name (e.g., `"Arial"`, `"Calibri"`) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFontName(1, 1, 1, "Arial");
```

---

### `XFontSize(sheet, column, row, fontSize)`
Sets the font size for a specific cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `fontSize` | int | Font size in points |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFontSize(1, 1, 1, 12);
```

---

### `XFontColor(sheet, column, row, red, green, blue)`
Sets the font color for a specific cell using RGB values.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `red` | int | Red component (0–255) |
| `green` | int | Green component (0–255) |
| `blue` | int | Blue component (0–255) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFontColor(1, 1, 1, 255, 0, 0);  // Red font
```

---

### `XBackgroundColor(sheet, column, row, red, green, blue)`
Sets the background (fill) color for a specific cell using RGB values. Uses solid fill pattern.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `red` | int | Red component (0–255) |
| `green` | int | Green component (0–255) |
| `blue` | int | Blue component (0–255) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XBackgroundColor(1, 1, 1, 255, 255, 0);  // Yellow background
```

---

### `XAlign(sheet, column, row, horizontal, vertical)`
Sets both horizontal and vertical alignment for a specific cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `horizontal` | int | Horizontal alignment code (see table below) |
| `vertical` | int | Vertical alignment code (see table below) |

**Horizontal alignment values:**

| Value | Alignment |
|-------|-----------|
| `1` | Left |
| `2` | Center |
| `3` | Right |
| `4` | Justify |

**Vertical alignment values:**

| Value | Alignment |
|-------|-----------|
| `1` | Top |
| `2` | Center |
| `3` | Bottom |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XAlign(1, 1, 1, 2, 2);  // Center horizontally and vertically
```

---

### `XFontFormat(sheet, column, row, bold, italic, underline, strikethrough, superscript, subscript)`
Applies font formatting options (bold, italic, etc.) to a specific cell.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `bold` | bool | Bold text |
| `italic` | bool | Italic text |
| `underline` | bool | Underline text |
| `strikethrough` | bool | Strikethrough text |
| `superscript` | bool | Superscript text (cannot be combined with subscript) |
| `subscript` | bool | Subscript text (cannot be combined with superscript) |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFontFormat(1, 1, 1, true, false, true, false, false, false);  // Bold + Underline
```

---

### `XBorder(sheet, column, row, left, right, top, bottom, weightLeft, weightRight, weightTop, weightBottom)`
Applies borders to a specific cell on any combination of sides.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `column` | int | Column number |
| `row` | int | Row number |
| `left` | bool | Apply left border |
| `right` | bool | Apply right border |
| `top` | bool | Apply top border |
| `bottom` | bool | Apply bottom border |
| `weightLeft` | int | Left border weight: `1`=Thin, `2`=Medium, `3`=Thick |
| `weightRight` | int | Right border weight: `1`=Thin, `2`=Medium, `3`=Thick |
| `weightTop` | int | Top border weight: `1`=Thin, `2`=Medium, `3`=Thick |
| `weightBottom` | int | Bottom border weight: `1`=Thin, `2`=Medium, `3`=Thick |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XBorder(1, 1, 1, true, true, true, true, 2, 2, 2, 2);  // All borders, medium weight
```

---

## Table & Named Range Operations

### `XSetTable(sheet, firstRow, firstCol, lastRow, lastCol, tableName)`
Creates an Excel table (ListObject) over a specified range.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `firstRow` | int | First row of the range |
| `firstCol` | int | First column of the range |
| `lastRow` | int | Last row of the range |
| `lastCol` | int | Last column of the range |
| `tableName` | string | Name for the table |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XSetTable(1, 1, 1, 10, 5, "MyTable");
```

---

### `XNamedCellPosition(varNameSheet, varNameColumn, varNameRow, cellName)`
Looks up a named range by name and writes its position (sheet index, column, row) into three CSCS variables.

| Parameter | Type | Description |
|-----------|------|-------------|
| `varNameSheet` | string | Name of the CSCS variable to receive the sheet index |
| `varNameColumn` | string | Name of the CSCS variable to receive the column number |
| `varNameRow` | string | Name of the CSCS variable to receive the row number |
| `cellName` | string | Name of the named range to look up |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `4` if any of the three output variables are not found in DEFINES.

**Example:**
```
sheetVar = 0;
colVar = 0;
rowVar = 0;
XNamedCellPosition("sheetVar", "colVar", "rowVar", "TotalCell");
```

---

### `XNamedRangeAdd(rangeName, rangeDefinition)`
Adds a named range to the workbook. The range definition must follow the format `SheetName!CellRange`.

| Parameter | Type | Description |
|-----------|------|-------------|
| `rangeName` | string | Name for the new named range |
| `rangeDefinition` | string | Range reference in `"SheetName!A1:B10"` format |

**Returns:** `true` on success, `false` on failure.

**Error codes:** `5` if failed to parse or add the named range.

**Example:**
```
XNamedRangeAdd("MyRange", "Sheet1!A1:B10");
```

---

## Header / Footer

### `XHeader(sheet, position, headerText)`
Sets the header text for the first page of a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `position` | int | Position: `1`=Left, `2`=Center, `3`=Right |
| `headerText` | string | Header text content |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XHeader(1, 2, "Monthly Report");  // Centered header
```

---

### `XFooter(sheet, position, footerText)`
Sets the footer text for the first page of a worksheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheet` | int | 1-based worksheet index |
| `position` | int | Position: `1`=Left, `2`=Center, `3`=Right |
| `footerText` | string | Footer text content |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XFooter(1, 1, "Confidential");  // Left-aligned footer
```

---

## Pivot Table

### `XPivotTableRefresh(sheetPivot, sheetData)`
Refreshes a pivot table's source data range from the data worksheet. Updates the first pivot table on the pivot sheet to match the current data range of the data sheet.

| Parameter | Type | Description |
|-----------|------|-------------|
| `sheetPivot` | int | Index of the worksheet containing the pivot table |
| `sheetData` | int | Index of the worksheet containing the source data |

**Returns:** `true` on success, `false` on failure.

**Example:**
```
XPivotTableRefresh(2, 1);  // Refresh pivot table on Sheet2 using data from Sheet1
```

---

## SQL to XLSX

### `SqlToXlsx(dbShortName, query, xlsxFileName)`
Executes a SQL query against a database and exports the result set directly to an XLSX file using **MiniExcel**. Automatically adds version numbers to the filename if the file already exists. Post-processes with **Spire.XLS** to freeze the header row and auto-fit columns.

| Parameter | Type | Description |
|-----------|------|-------------|
| `dbShortName` | string | Database short name (looked up in `SY_DATABASESList`) |
| `query` | string | SQL query to execute |
| `xlsxFileName` | string | Output .xlsx file path (`.xlsx` extension appended if missing; version number added if file exists) |

**Returns:** `Empty` on success. On error, the exception is caught silently (no error number set in the main flow).

**Behavior:**
- The `Id` column is automatically excluded from the output.
- Date columns are converted to short date string format.
- The `USE [dbFullName]` statement is prepended to the query.
- If the output file already exists, a version number is appended: `file(1).xlsx`, `file(2).xlsx`, etc.
- After export: row 1 is frozen as header, and all columns are auto-fitted.

**Example:**
```
SqlToXlsx("MyDB", "SELECT Name, Value FROM Settings", "SettingsExport.xlsx");
```

---

## Error Handling

### `XErr()`
Returns the last error number from any Excel function. This is useful for checking whether the last operation succeeded.

**Parameters:** None

**Returns:** Integer error code (`0` means no error).

**Example:**
```
result = XFileOpen("missing.xlsx");
err = XErr();
if (err != 0) {
    ShowMessage("Error opening file, code: " + err);
}
```

---

## Error Codes Reference

| Code | Description |
|------|-------------|
| `0` | No error — operation completed successfully |
| `1` | General exception — an unexpected error occurred (see message box for details) |
| `2` | File not found — the specified file does not exist (used by `XFileOpen`, `XFileDelete`) |
| `3` | Sheet index out of range — the worksheet index exceeds the sheet count (used by `XSheetClear`) |
| `4` | CSCS variable not found — a referenced define variable does not exist in the context (used by time/date write functions and `XNamedCellPosition`) |
| `5` | Search/range not found — a lookup or named range operation returned no results (used by `XFindColumn`, `XFindRow`, `XNamedRangeAdd`) |
| `6` | Invalid parameter value — an alignment, position, or border weight value is out of the allowed range (used by `XHeader`, `XFooter`, `XAlign`, `XBorder`) |

---

## General Notes

- **Column and row indices are 1-based** throughout all functions.
- All cell write functions that accept a `sheet` parameter expect a **1-based worksheet index**.
- The global `Excel.document` object must be initialized via `XFileNew` or `XFileOpen` before any sheet or cell operations.
- The `Excel.document` is a static (global) object — only one workbook can be open at a time.
- Formatting functions apply to individual cells; for whole-column formatting use `XFormatColumn`.
- The `XFontFormat` function does not allow both `superscript` and `subscript` to be `true` simultaneously (a warning is shown but no error code is set).
- The `SqlToXlsx` function uses **MiniExcel** for fast CSV-style export and **Spire.XLS** for post-processing (freeze panes, auto-fit).
- All other functions use **EPPlus (OfficeOpenXml)** for full Excel file manipulation.
