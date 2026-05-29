---
title: Report Label Binding
module: reporting
topic: labels
applies_to: CSCS_WPF
version: 1
---

# Report Label Binding

## Purpose
CSCS_WPF binds report label values in DevExpress reports primarily through XRLabel controls.  
The standard rule is that the XRLabel `Tag` property contains the variable name whose value should be shown in the report .

## Core binding rule
For XRLabel controls:
- place label in `.repx`
- set `Tag` to variable name
- fill the corresponding variable in script
- use `OutputReport(...)` or `UpdateReport(...)` depending on scenario 

## Binding modes
There are two common label binding modes:
- single value binding
- multi-row value binding 

## Single value binding

### Use case
A value that appears once at report level, such as grand total in report footer .

### Template setup
In main report `ReportFooter` band:
- put label
- `Name = reportGrandTotal`
- `Tag = reportGrandTotal` 

### Script example
```internal-script
grandTotal = 10000;
reportGrandTotal = "GRAND TOTAL " + grandTotal.ToString("N2");
UpdateReport(reportHandle, "reportGrandTotal");
```

### Why UpdateReport
Single report-level values are often updated after rows have already been output, so `UpdateReport(...)` is used to update the last report unit for the listed variable .

## Multi-row value binding

### Use case
Values that repeat for each row or detail unit, such as invoice number in invoice subreport or line fields in detail report .

### Template setup
In invoice report `Detail` band:
- `Name = invoiceNumber`
- `Tag = invoiceNumber`
- `Expression = invoiceNumber` is optional and not required 

### Script example
```internal-script
invoiceNumber = "111111";
OutputReport(reportHandle2);
```

Each `OutputReport(...)` call creates one report unit using current variable values .

## Field label example in detail report
```internal-script
field1 = "PROD-001";
field2 = "Laptop HP ProBook 450";
field3 = "Electronics";
amount = 5499.99;

OutputReport(reportHandle3);
```

If the detail report contains labels whose tags are `field1`, `field2`, `field3`, and `amount`, those values are rendered into one detail unit .

## Relationship to report bands
Typical mapping:
- `ReportHeader` labels for report-wide values
- `Detail` labels for repeating row values
- `ReportFooter` or `GroupFooter` labels for totals and summaries 

## Label formatting notes
The manual shows examples of formatting and summary behavior in the report designer, for example:
- expression such as `sumSumamount`
- format string such as `0N2`
- summary running mode set on footer totals 

This means label rendering can combine:
- runtime script values through `Tag`
- designer expressions
- designer formatting and summary settings 

## Example full pattern
```internal-script
reportHandle = SetupReport("MainReport.repx");
reportHandle2 = SetupReport("InvoiceSubreport.repx", reportHandle);
reportHandle3 = SetupReport("DetailSubreport.repx", reportHandle2);

grandTotal = 0;

invoiceNumber = "INVOICE 111111";
invoiceDate = "11.1.2026";
OutputReport(reportHandle2);

field1 = "PROD-001";
field2 = "Laptop HP ProBook 450";
field3 = "Electronics";
amount = 5499.99;
grandTotal = grandTotal + amount;
OutputReport(reportHandle3);

field1 = "PROD-002";
field2 = "Monitor Dell 24 inch";
field3 = "Electronics";
amount = 899.00;
grandTotal = grandTotal + amount;
OutputReport(reportHandle3);

reportGrandTotal = "GRAND TOTAL " + grandTotal.ToString("N2");
UpdateReport(reportHandle, "reportGrandTotal");
```

## Style/property note
The manual also mentions label-related property-style patterns such as:
- `label1Fc` for font color
- `label1Cl` for another color-related property
- `label1Lt` for left margin
- `label1Top` for top margin
- `label1Wd` for width
- `label1Ht` for height
- `label1Fs` for font size
- `label1Fn` for font name 

These indicate that label appearance can also be influenced through variables or conventions used by the report/template setup .

## Common mistakes
- Forgetting to set XRLabel `Tag`.
- Using `UpdateReport(...)` for row creation instead of `OutputReport(...)`.
- Expecting `OutputReport(...)` to modify already-rendered footer value.
- Mixing report-wide values and row-level values in the wrong report band .

## Best practices
- Use `Tag` consistently for label binding.
- Use `OutputReport(...)` for repeating units.
- Use `UpdateReport(...)` for late updates of summary or final labels.
- Keep naming between label tags and script variables identical.
- Put totals in footer bands and transactional values in detail bands. 

## AI notes
- XRLabel binding is primarily based on `Tag = variableName`.
- `OutputReport(...)` creates units; `UpdateReport(...)` updates last created unit.
- Report band placement matters for how values repeat.
- If a label is a footer summary or final text, `UpdateReport(...)` is often the correct operation. 