---
title: Pattern Master Detail Report
module: patterns
topic: reporting
applies_to: CSCS_WPF
version: 1
---

# Pattern Master Detail Report

## Purpose
This pattern describes how to build a master-detail report in CSCS_WPF using DevExpress reports and linked subreports.  
The typical use case is invoice header -> invoice lines, or document header -> item details .

## Pattern structure
A master-detail report usually has:
- main report
- subreport for master row block
- optional sub-subreport for detail rows 

Example chain:
- main report: report title, date, logo, footer totals
- invoice subreport: invoice number and invoice date
- detail subreport: product rows and line totals 

## Typical file set
```text
MainReport.repx
InvoiceSubreport.repx
InvoiceDetailSubreport.repx
```

## Setup sequence
Create reports from parent to child using `SetupReport(...)` .

```internal-script
reportHandle = SetupReport("startersimple6.repx");
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);
```

## Main report responsibilities
Main report typically contains:
- picture box for company logo
- labels for report title and report date
- subreport control for invoice subreport
- grand total label in footer
- print date and page number in page footer 

## Invoice subreport responsibilities
Invoice subreport typically contains:
- invoice number
- invoice date
- subreport control for detail report 

## Detail subreport responsibilities
Detail subreport typically contains:
- column headers
- fields such as `field1`, `field2`, `field3`, `amount`
- per-group or per-report totals 

## Validation pattern
Before generating output, validate template fields .

```internal-script
errors = ValidateReportTemplate(tpath, "startersimple6invoice.repx", "invoiceNumber,invoiceDate");
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

## Parameter setup
Main report parameters are usually set once:
```internal-script
reportTitle = "INVOICES 2026";
reportDate = "Report date " + Now("dd.MM.yyyy");
logoPath = "C:\\logo.png";

SetReportParameter(reportHandle, "reportTitle", reportTitle);
SetReportParameter(reportHandle, "reportDate", reportDate);
SetReportParameter(reportHandle, "logoPath", logoPath);
```

## Master-detail output pattern
The usual output order is:
1. output one main/master section
2. output one invoice block
3. output all lines for that invoice
4. repeat for next invoice
5. update summary labels
6. print or export 

## Simplified example
```internal-script
reportHandle = SetupReport("startersimple6.repx");
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);

ConfigureReportOutput(reportHandle, "PageSize", "A4");
SetReportParameter(reportHandle, "reportTitle", "INVOICES 2026");
SetReportParameter(reportHandle, "reportDate", "29.05.2026");
SetReportParameter(reportHandle, "logoPath", "C:\\logo.png");

grandTotal = 0;

invoiceNumber = "INVOICE 111111";
invoiceDate = "Invoice date 11.1.2026";
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

invoiceNumber = "INVOICE 222222";
invoiceDate = "Invoice date 22.2.2026";
OutputReport(reportHandle2);

field1 = "PROD-003";
field2 = "Office Chair Ergonomic";
field3 = "Furniture";
amount = 1299.00;
grandTotal = grandTotal + amount;
OutputReport(reportHandle3);

reportGrandTotal = "GRAND TOTAL " + grandTotal.ToString("N2");
UpdateReport(reportHandle, "reportGrandTotal");

PrintReport(reportHandle);

DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

## Using subreport controls in .repx
To connect reports structurally:
- place subreport control on parent report
- keep `ReportSourceURL` empty
- link child report through `SetupReport(child, parentHandle)` 

## Cleanup order
Dispose child reports before parent report .

Correct order:
```internal-script
DisposeReport(subsubreportHandle);
DisposeReport(subreportHandle);
DisposeReport(reportHandle);
```

## Common mistakes
- Creating child report without parent handle.
- Outputting detail rows before parent invoice block exists.
- Disposing parent before subreports.
- Forgetting validation of expected fields .

## Best practices
- Keep one clear responsibility per report level.
- Put global parameters on main report.
- Put repeating header data on subreport.
- Put row-level data on detail subreport.
- Dispose reports in reverse order of creation. 

## AI notes
- Master-detail reports are built by chaining `SetupReport(...)` with parent handles.
- Output order matters: parent first, child rows after.
- Footer totals are typically updated on main report with `UpdateReport(...)`.
- Always dispose deepest child report first. 