This document is crucial for AI report script generation. It must clearly describe the order of work with .repx templates and how the data is filled into the report.WpfCSCS-UPUTE-3.docx
text
---
title: Reporting Lifecycle
module: reporting
topic: lifecycle
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# Reporting Lifecycle

## Purpose

This document describes the basic lifecycle of working with DevExpress reports in CSCS_WPF environment.

Reporting is based on '.repx' templates and functions that:
- I'm going to read the report,
- It's filled with data,
- I'm going to update some of the values,
- I'm going to show a preview,
- export the document,
- They're freeing up resources.

## Main concepts

- Report template is a '.repx' file.
- 'SetupReport' creates a report handle.
- 'OutputReport' adds one report unit of data.
- 'UpdateReport' updates the last report unit.
- 'PrintReport' opens the preview/print.
- 'ExportReport' exports the report.
- 'DisposeReport' frees up resources.
- Subreports are linked via parent handle.

## Standard lifecycle

The typical order of work is:

1. validate the template if necessary,
2. load the report with 'SetupReport',
3. load subreports if necessary,
4. configure the output,
5. set the parameters of the report,
6. prepare data in variables,
7. call 'OutputReport' for each record/unit,
8. use 'UpdateReport' if necessary,
9. display the report with 'PrintReport' or export it with 'ExportReport',
10. Free handles from 'DisposeReport'.

## Function summary

| Function | Purpose | Returns |
|---|---|---|
| `SetupReport(path, optionalParentHandle)` | loads '.repx' and returns handle | int |
| `ConfigureReportOutput(reportHandle, option, value)` | adjusts the output | empty |
| `SetReportParameter(reportHandle, paramName, value)` | sets the report parameter | empty |
| `OutputReport(reportHandle)` | Prints the current values of variables in Report | empty |
| `UpdateReport(reportHandle, variableList)` | Updates part of the last report unit | empty |
| `PrintReport(reportHandle)` | opens preview / print | empty |
| `ExportReport(reportHandle, format, outputPath)` | izvozi report | void |
| `DisposeReport(reportHandle)` | frees up resources | void |

## SetupReport

'SetupReport' loads the '.repx' file and returns the handle report integer.

### Syntax

```internal-script
reportHandle = SetupReport("MainReport.repx");
subHandle = SetupReport("SubReport.repx", reportHandle);
```

### Notes

The trajectory is relative to '.exe'.
- Parent Handle is used to link subreports.

## OutputReport

'OutputReport' takes the current values of the variables and prints them to the corresponding report fields.

### Principle

First, the variables are populated, then 'OutputReport(reportHandle)' is called. For multiple rows or multiple reporting units, the same procedure is repeated.

### Example

```internal-script
invoiceNumber = "INV-001";
invoiceDate = "11.1.2026.";

OutputReport(reportHandle);
```

## UpdateReport

'UpdateReport' updates the last added report unit, but only for the specified variables.

### Example

```internal-script
reportGrandTotal = "GRAND TOTAL 1000.00";
UpdateReport(reportHandle, "reportGrandTotal");
```

## ConfigureReportOutput

This function sets the basic printing options.

### Supported options

- `PageSize`
- `Orientation`
- `Margins`
- `ShowPrintDialog`

### Example

```internal-script
ConfigureReportOutput(reportHandle, "PageSize", "A4");
ConfigureReportOutput(reportHandle, "Orientation", "Portrait");
ConfigureReportOutput(reportHandle, "Margins", "10,10,10,10");
```

## Parameters

Report parameters are set using 'SetReportParameter'.

### Example

```internal-script
reportTitle = "INVOICES 2026";
SetReportParameter(reportHandle, "reportTitle", reportTitle);
```

The parameter must exist in the '.repx' template.

## PrintReport

'PrintReport' opens the preview and print options.

### Example

```internal-script
PrintReport(reportHandle);
```

## ExportReport

The report can be exported in multiple formats.

### Supported formats

- PDF
- XLSX
- DOCX
- HTML
- RTF
- CSV

### Example

```internal-script
ExportReport(reportHandle, "PDF", "D:\\report.pdf");
```

## DisposeReport

When the report is complete, the handle should be released.

If there is a parent report and a subreport, the deepest subreport is released first, followed by the parent.

### Example

```internal-script
DisposeReport(subSubReportHandle);
DisposeReport(subReportHandle);
DisposeReport(reportHandle);
```

## Validation helpers

### ValidateReportTemplate

It checks that all the required fields are present in the script and template.

```internal-script
errors = ValidateReportTemplate(templatePath);

if (Size(errors) > 0) {
    MessageBox("Validation errors found!");
}
```

### GetReportFields

Returns a list of fields found in the template.

```internal-script
myFields = GetReportFields(templatePath);
MessageBox(myFields);
```

## Complete example

```internal-script
tpath = "startersimple6.repx";

errors = ValidateReportTemplate(tpath);
if (Size(errors) > 0) {
    MessageBox("Validation errors found!");
    exit;
}

reportHandle = SetupReport(tpath);
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);

ConfigureReportOutput(reportHandle, "PageSize", "A4");

reportTitle = "INVOICES 2026";
reportDate = "Report date 11.1.2026.";
logoPath = "C:\\logo.png";

SetReportParameter(reportHandle, "reportTitle", reportTitle);
SetReportParameter(reportHandle, "reportDate", reportDate);
SetReportParameter(reportHandle, "logoPath", logoPath);

invoiceNumber = "INVOICE 111111";
invoiceDate = "Invoice date 11.1.2026.";
OutputReport(reportHandle2);

field1 = "PROD-001";
field2 = "Laptop HP ProBook 450";
field3 = "Electronics";
amount = 5499.99;
OutputReport(reportHandle3);

reportGrandTotal = "GRAND TOTAL 5499.99";
UpdateReport(reportHandle, "reportGrandTotal");

ExportReport(reportHandle, "PDF", "D:\\report6.pdf");
PrintReport(reportHandle);

DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

## Common mistakes

- 'SetupReport' is called without the correct path to '.repx'.
- 'SetReportParameter' uses a parameter that does not exist in the template.
- 'OutputReport' is called before the variables are prepared.
- 'UpdateReport' is used before the first 'OutputReport'.
- Subreports are not disposed of in reverse order.
- AI invents field names that are not in the '.repx' template.

## AI notes

- Do not invent report fields, parameters and tag names.
- For each report, first identify which '.repx' template exists.
- For multi-row data, use the repeated form: fill in variables -> 'OutputReport'.
- For final aggregates and one-time values, use 'UpdateReport' or parameters, depending on the role in the template.
- When subreporting, always pay attention to the parent-child order of handles.
- When binding a variable in a .repx report, only the target element must define the Tag property, and its value MUST exactly match the variable name.

