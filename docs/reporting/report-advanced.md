---
title: Report Advanced Examples
module: reporting
topic: advanced-examples
applies_to: CSCS_WPF
version: 1
---

# Report Advanced Examples

## Purpose
This document collects more advanced reporting patterns in CSCS_WPF, especially:
- subreport chains
- chart-enabled reports
- barcode and QR usage
- logo picture binding
- validation and export flow 

## Example 1: Main report with parameters and logo

### Template design
Main report contains:
- picture box `companyLogo` in ReportHeader
- labels for `reportTitle` and `reportDate`
- footer label for `reportGrandTotal`
- subreport for invoice blocks 

### Script pattern
```internal-script
reportHandle = SetupReport("startersimple6.repx");

reportTitle = "INVOICES 2026";
reportDate = "Report date " + Now("dd.MM.yyyy");
logoPath = "C:\\logo.png";

SetReportParameter(reportHandle, "reportTitle", reportTitle);
SetReportParameter(reportHandle, "reportDate", reportDate);
SetReportParameter(reportHandle, "logoPath", logoPath);
```

## Example 2: Subreport chain with detail rows
```internal-script
reportHandle = SetupReport("startersimple6.repx");
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);

invoiceNumber = "INVOICE 111111";
invoiceDate = "Invoice date 11.1.2026";
OutputReport(reportHandle2);

field1 = "PROD-001";
field2 = "Laptop HP ProBook 450";
field3 = "Electronics";
amount = 5499.99;
OutputReport(reportHandle3);

field1 = "PROD-002";
field2 = "Monitor Dell 24 inch";
field3 = "Electronics";
amount = 899.00;
OutputReport(reportHandle3);
```

## Example 3: QR code in invoice report

### Template setup
In `.repx`:
- place XRBarCode in Detail band
- `Tag = invoiceQRcode`
- `Symbology = QRCode`
- `ShowText = True`
- `AutoModule = True` 

### Script
```internal-script
invoiceQRcode = "HTTPS://WWW.AURASOFT.HR-.01234 56789";
UpdateReport(reportHandle2, "invoiceQRcode");
```

## Example 4: Main and line barcodes
```internal-script
NKPRGLNUM = "INV-2026-0001";
OutputReport(reportHandle);

NKPRLNPCODE = "PROD-001";
OutputReport(reportHandle3);
```

The main report barcode can use one field such as document number, while detail report barcode can encode item code .

## Example 5: Chart-enabled report
For chart reports:
- place chart on report
- set chart `Tag` to array prefix
- define matching arrays in script 

```internal-script
subreportChartVal1
subreportChartVal1.Add(100)
subreportChartVal1.Add(100)
subreportChartVal1.Add(130)

subreportChartArgs
subreportChartArgs.Add("a")
subreportChartArgs.Add("b")
subreportChartArgs.Add("c")

OutputReport(reportHandle2);
```

## Example 6: Validation before report creation
```internal-script
errors = ValidateReportTemplate(tpath, "startersimple6detail.repx", "field1,field2,field3,amount");
if (Size(errors) > 0)
{
    MSG("Validation errors found!");
    for (i = 0; i < Size(errors); i++)
    {
        MSG("- " + errors[i]);
    }
    exit;
}
```

Use validation before printing or export when reports are maintained independently from script logic .

## Example 7: Export and print flow
```internal-script
ExportReport(reportHandle, "PDF", "D:\\report6.pdf");
PrintReport(reportHandle);
```

Supported export formats include:
- PDF
- XLSX
- DOCX
- HTML
- RTF
- CSV 

## Example 8: Full cleanup sequence
```internal-script
DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

When there is a parent-child report hierarchy, disposal should go from deepest child to top parent .

## Example 9: Getting report metadata
You can inspect or validate report definitions using:
- `GetReportFields(templatePath)`
- `GetReportParameter(reportHandle, parameterName)` 

```internal-script
myFields = GetReportFields(tpath + "startersimple5invoice.repx");
value = GetReportParameter(reportHandle, "reportPrintingPerson");
```

## Advanced design notes
Advanced reports often combine several features:
- logo in header
- parameters for title/date/printing person
- subreports for nesting
- barcodes in document rows
- charts in summary or subreport sections
- footer totals and page info 

## Common mistakes
- Combining too many responsibilities into one `.repx`.
- Forgetting that subreports must be connected through `SetupReport(child, parent)`.
- Using chart arrays with wrong prefix compared to report `Tag`.
- Updating values after output without calling `UpdateReport(...)`.
- Disposing reports in wrong order .

## Best practices
- Keep templates layered: main, subreport, detail.
- Validate templates before runtime output.
- Use parameters for report-wide values and variables for row data.
- Use `ExportReport(...)` for generated documents and `PrintReport(...)` for preview/printing.
- Clean up every created report handle. 

## AI notes
- Advanced reports in CSCS_WPF are built by combining existing primitives, not special hidden APIs.
- Typical building blocks are `SetupReport`, `SetReportParameter`, `OutputReport`, `UpdateReport`, `ExportReport`, `PrintReport`, and `DisposeReport`.
- Charts, barcodes, pictures, and subreports all depend on correct `.repx` design-time configuration.
- Prefer explicit, layered report flows over monolithic report scripts. 