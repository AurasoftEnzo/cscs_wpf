This document should explain how the subreport is physically placed in .repx and how it logically connects in the script via SetupReport(subreport, parentHandle). The manual clearly emphasizes that subreports are connected via parent handle and that disposal is done in reverse order.


title: Report Subreports
module: reporting
topic: subreports
applies_to: CSCS_WPF
version: 1
source: internal manual



# Report Subreports

## Purpose

This document describes how to use subreports in the DevExpress reporting system within CSCS_WPF scripts.

Subreport enables a hierarchical structure of the report, e.g.:
- glavni report,
- invoice report kao podreport,
- detail lines report kao podreport invoice reporta.

## Main concepts

- The subreport is set in the '.repx' designer as 'XRSubreport'.
- Logical linking is done via 'SetupReport(subReportPath, parentReportHandle)'.
- The parent-child relationship is built through integer handles .
- When shutting down the report, the deepest child reports are disposed of first, and then the parent .

## Designer setup

In the main report:
- add a 'subreport' control,
- set the 'Name',
- Leave 'ReportSourceUrl' blank if the connection will be done by the  script.

## SetupReport for parent and child

### Syntax

```internal-script
mainReportHandle = SetupReport("MainReport.repx");
invoiceReportHandle = SetupReport("InvoiceReport.repx", mainReportHandle);
detailReportHandle = SetupReport("DetailReport.repx", invoiceReportHandle);
```

### Meaning

- the first argument is the '.repx' file of the subreport,
- The second argument is Handle parent report .

## Typical hierarchy

| Level | Example |
|---|---|
| Main report | summary / report shell |
| First subreport | invoice header / invoice container |
| Second subreport | invoice lines / details |

## Data flow concept

A typical pattern is:
1. load the main report,
2. load the first subreport with the parent handle,
3. load the second subreport with the handle of the first subreport,
4. set parameters to the appropriate level,
5. call 'OutputReport' on the handle that represents the data level you are filling in .

## Example hierarchy script

```internal-script
reportHandle = SetupReport("startersimple6.repx");
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);
```

## Example data population

```internal-script
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

field1 = "PROD-002";
field2 = "Monitor Dell 24 inch";
field3 = "Electronics";
amount = 899.00;
OutputReport(reportHandle3);
```

## Parent-child rules

- Main report je root.
- Each child report must be linked to parent handle.
- Data should be sent to the appropriate level of the report.
- Header/global parameters usually go to the main report.
- Detail lines usually go to child or grandchild report .

## Validation before linking

It is recommended to validate the template before full use.

### Example

```internal-script
errors = ValidateReportTemplate("startersimple6detail.repx");

if (Size(errors) > 0) {
    MessageBox("Validation errors found!");
    exit;
}
```

## Dispose order

If there are multiple levels:
1. First of all, the most in-depth sub-report.
2. And then there's his parent.
3. at the end of the root report .

### Example

```internal-script
DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

## Full example

```internal-script
reportHandle = SetupReport("startersimple6.repx");
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

field1 = "PROD-002";
field2 = "Monitor Dell 24 inch";
field3 = "Electronics";
amount = 899.00;
OutputReport(reportHandle3);

PrintReport(reportHandle);

DisposeReport(reportHandle3);
DisposeReport(reportHandle2);
DisposeReport(reportHandle);
```

## Common mistakes

- Creating a child report without parent handle.
- Sending data to the wrong report level.
- The expectation that 'XRSubreport' itself knows where to retrieve data without 'SetupReport'.
- Disposal in the wrong order.
- Mixing root report parameters and detailed child report fields.

## AI notes

- For each subreport, explicitly state the parent handle.
- Do not assume automatic subreport linking without 'SetupReport'.
- For complex reports, first model the hierarchy of handles, and only then generate 'OutputReport' calls.
- If the structure of the '.repx' files is not known, this should be stated as a limitation.



