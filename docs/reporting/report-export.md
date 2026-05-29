---
title: Report Export and Dispose
module: reporting
topic: export-cleanup
applies_to: CSCS_WPF
version: 1
---

# Report Export and Dispose

## Purpose
After report data is prepared, CSCS_WPF supports:
- exporting the report to a file using `ExportReport(...)`
- releasing report resources using `DisposeReport(...)` 

These functions are typically the final phase of the reporting workflow after:
- `SetupReport(...)`
- `SetReportParameter(...)`
- `OutputReport(...)`
- `UpdateReport(...)`
- optional `PrintReport(...)` 

## Functions covered
| Function | Purpose |
|---|---|
| `ExportReport(reportHandle, format, outputPath)` | Exports report to file |
| `DisposeReport(reportHandle)` | Releases report resources | 

## ExportReport

### Purpose
Exports a prepared report to disk .

### Syntax
```internal-script
ExportReport(reportHandle, format, outputPath);
```

### Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `reportHandle` | integer | yes | Handle of the prepared report |
| `format` | string | yes | Export format |
| `outputPath` | string | yes | Destination file path | 

### Supported formats
The manual lists these export formats:
- `PDF`
- `XLSX`
- `DOCX`
- `HTML`
- `RTF`
- `CSV` 

### Example
```internal-script
ExportReport(reportHandle, "PDF", "D:\\report6.pdf");
```

## DisposeReport

### Purpose
Disposes a report after printing or export is complete .

### Syntax
```internal-script
DisposeReport(reportHandle);
```

### Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `reportHandle` | integer | yes | Handle of the report to release | 

### Returns
Returns `void` .

### Example
```internal-script
DisposeReport(reportHandle);
```

## Cleanup order with subreports
If a report hierarchy exists, child reports must be disposed before parent report .

Correct order:
```internal-script
DisposeReport(subsubreportHandle);
DisposeReport(subreportHandle);
DisposeReport(reportHandle);
```

## Typical final workflow
```internal-script
reportHandle = SetupReport("MainReport.repx");
reportHandle2 = SetupReport("InvoiceSubreport.repx", reportHandle);
reportHandle3 = SetupReport("DetailSubreport.repx", reportHandle2);

/* data preparation here */

ExportReport(reportHandle, "PDF", "D:\\invoice-report.pdf");
PrintReport(reportHandle);

DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

## Print versus export
- `PrintReport(...)` opens print preview and printing options.
- `ExportReport(...)` writes the report to a file in requested format.
- Both can be used in the same flow if needed .

## When to export
Use `ExportReport(...)` when:
- report must be archived
- report must be sent to another system
- report must be produced without manual print action
- a final document file is required 

## When to dispose
Use `DisposeReport(...)` when:
- report is no longer needed
- printing/export is completed
- subreport hierarchy must be cleaned up
- script is ending and handles should be released 

## Common mistakes
- Forgetting to dispose report handles after export or print.
- Disposing parent before subreports.
- Exporting before report data is fully prepared.
- Using invalid output path or unsupported format string .

## Best practices
- Export or print only after all `OutputReport(...)` and `UpdateReport(...)` calls are complete.
- Dispose every created report handle.
- In report hierarchies always clean up from deepest child to root.
- Use predictable export naming conventions for generated files. 

## AI notes
- `ExportReport(...)` is a finalization step, not a data-binding function.
- `DisposeReport(...)` should always appear near the end of reporting scripts.
- Child reports must be disposed before parent reports.
- Export and print can coexist in one workflow. 