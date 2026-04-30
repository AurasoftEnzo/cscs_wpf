This document is very important because in reports there is a difference between a label that gets a value by Tag name, a label with an Expression, a label for multiple rows via OutputReport and a label for totals via summary expressions. The attached manual explicitly shows binding via Tag, multi-line binding in the detail band, and summary expressions such as sumSumamount and SumNKPRGLTOTAL with string format 0N2 and Summary Running settings.

---
title: Report Label Binding
module: reporting
topic: label-binding
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# Report Label Binding

## Purpose

This document describes how 'XRLabel' controls are populated with data in DevExpress reports within CSCS_WPF system.

Label binding can be done in several ways:
- via the 'Tag' property that maps the name of the variable,
- via "Expression",
- via 'UpdateReport' for one-time values,
- via summary expressions for sums and aggregates.

## Main concepts

- The 'tag' to 'XRLabel' usually contains the name of the variable from the script.
- 'OutputReport' uses the current variable values to create a single report unit.
- 'UpdateReport' updates the last report unit for certain variables.
- 'Expression' in the designer may be required for calculations and aggregates.
- 'Format String' is used for numerical display, e.g. '0N2'.

## Single value binding

For a one-time value in the footer or header, 'Tag' + 'UpdateReport' can be used.

### . REPX setup

- Add label to `ReportFooter`
- `Name = reportGrandTotal`
- `Tag = reportGrandTotal` 

### Script

```internal-script
grandTotal = 10000;
reportGrandTotal = "GRAND TOTAL" + grandTotal.ToString("N2");
UpdateReport(reportHandle, "reportGrandTotal");
```

## Multi-row value binding

For the values that are printed in each row of the detail band, a 'Tag' is used, and the data is submitted through 'OutputReport'.

### . REPX setup

- Add label to `Detail`
- `Name = invoiceNumber`
- `Tag = invoiceNumber`
- `Expression = invoiceNumber` (not necessarily necessary in a simple case) 

### Script

```internal-script
invoiceNumber = "111111";
OutputReport(reportHandle2);
```

## Field binding by Tag

A typical form for a detail report:

| Label | Tag | Script variable |
|---|---|---|
| `column1` | `field1` | `field1` |
| `column2` | `field2` | `field2` |
| `column3` | `field3` | `field3` |
| `amount`  | `amount` | `amount` |

The documentation shows an example where 'Label column1' has 'expression field1' and 'tag field1', and 'amount' is a numeric field in the detail band .

## Summary labels

For the sums of all invoices or all rows, 'Expression' and 'Summary Running` are used.

### Examples from report designer

- `SumNKPRGLTOTAL`
- `sumSumamount`
- `sumSumNKPRLNPOREZ`
- `sumSumNKPRLNPPRCE` 

### Required settings

- `Expression` = corresponding summary expression
- `Format String` = e.g. '0N2'
- `Summary Running` = 'Report' or appropriate group 

## Example: invoice detail labels

### . REPX setup

```text
Label column1:
- Tag = field1
- Expression = field1

Label totalAmount:
- Expression = sumSumamount
- String format = 0N2
- Summary Running = Report
```

### Script

```internal-script
field1 = "PROD-001";
field2 = "Laptop HP ProBook 450";
field3 = "Electronics";
amount = 5499.99;

OutputReport(reportHandle3);
```

## Labels vs parameters

| Use case | Mechanism |
|---|---|
| detail row values | label `Tag` + `OutputReport` |
| footer one-time text | label `Tag` + `UpdateReport` |
| report header config text | parameter |
| report sums | `Expression` + summary settings |

## Formatting

For numeric values, it is recommended to use format string as '0N2' when two decimal places are needed .

## Advanced visual properties

The documentation also lists additional label settings such as:
- font color,
Left/top position.
- width/height,
- font size,
- font name .

These properties are the layout/visual part, but they do not change the underlying data-binding mechanism.

## Common mistakes

- Using a 'Tag' name that does not match a variable from the script.
- Expecting 'UpdateReport' to create a new detail row, even though it is used to update the last report unit .
- Using 'OutputReport' for footer aggregate instead of summary or 'UpdateReport' expressions.
- Mixing parameters and detail fields.
- AI invents 'Expression' syntax that is not validated by the template.

## AI notes

- If it's a detail line, first try the 'Tag = variableName' + 'OutputReport' model.
- If it is an aggregate, use the 'Expression' + summary settings.
- If it is a one-time final value, use the 'UpdateReport' or parameter, depending on the role in the template.
- Don't assume that every label needs both a 'Tag' and an 'Expression'; simple case often works with 'Tag' mapping .
