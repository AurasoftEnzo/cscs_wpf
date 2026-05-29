---
title: Report Barcodes and QR Codes
module: reporting
topic: barcode-qrcode
applies_to: CSCS_WPF
version: 1
---

# Report Barcodes and QR Codes

## Purpose
CSCS_WPF supports barcode and QR code rendering in DevExpress reports through XRBarCode controls.  
The encoded value is supplied from script through the report control `Tag` property and then pushed into the report using standard report output functions .

## Core binding rule
For XRBarCode controls:
- place barcode control in the report template
- set its `Tag` property to the script variable name that contains the encoded value 

Example:
```text
Tag = invoiceQRcode
```

Then in script:
```internal-script
invoiceQRcode = "HTTPS://WWW.AURASOFT.HR-.01234 56789";
```

## QR code example

### Template setup
In `.repx`:
- put Bar Code control on report
- `Name = barCode1`
- `Tag = invoiceQRcode`
- `Symbology = QRCode`
- `ShowText = True`
- `AutoModule = True` 

### Script example
```internal-script
invoiceQRcode = "HTTPS://WWW.AURASOFT.HR-.01234 56789";
UpdateReport(reportHandle2, "invoiceQRcode");
```

The QR code value is taken from variable `invoiceQRcode` and rendered into the control whose `Tag` matches that variable name .

## Standard barcode example

### Main report barcode
In `.repx`:
- place barcode in main report Detail band
- choose desired symbology such as `Code128`
- set `Tag = NKPRGLNUM` 

### Script example
```internal-script
NKPRGLNUM = "INV-2026-0001";
OutputReport(reportHandle);
```

## Subreport line barcode example
In line-detail subreport:
- place barcode in Detail band
- choose `Code39`
- set `Tag = NKPRLNPCODE` 

### Script example
```internal-script
NKPRLNPCODE = "PROD-001";
OutputReport(detailReportHandle);
```

## How binding works
Barcode and QR controls behave similarly to labels:
- `Tag` identifies the variable to use
- script sets variable value
- `OutputReport(...)` outputs a new report row/unit
- `UpdateReport(...)` updates already rendered unit for selected variables 

## Typical workflow
1. create report using `SetupReport(...)`
2. define barcode or QR value variables
3. call `OutputReport(...)` or `UpdateReport(...)`
4. print or export report 

## Example with invoice QR code
```internal-script
reportHandle = SetupReport("InvoiceReport.repx");

invoiceNumber = "INV-2026-001";
invoiceQRcode = "https://example.test/pay/INV-2026-001";

OutputReport(reportHandle);
UpdateReport(reportHandle, "invoiceQRcode");

PrintReport(reportHandle);
DisposeReport(reportHandle);
```

## Example with main and detail barcodes
```internal-script
mainReport = SetupReport("InvoiceMain.repx");
detailReport = SetupReport("InvoiceLines.repx", mainReport);

NKPRGLNUM = "INV-2026-001";
OutputReport(mainReport);

NKPRLNPCODE = "PROD-001";
OutputReport(detailReport);

NKPRLNPCODE = "PROD-002";
OutputReport(detailReport);
```

## Supported use cases
- invoice number barcode
- product code barcode
- payment QR code
- document tracking code
- logistics label values 

## Common mistakes
- Setting barcode value in script but leaving report control `Tag` empty.
- Using wrong variable name in `Tag`.
- Forgetting to call `OutputReport(...)` or `UpdateReport(...)`.
- Changing variable value after output without updating report unit .

## Best practices
- Use clear variable names such as `invoiceQRcode`, `documentBarcode`, `itemBarcode`.
- Keep barcode symbology consistent with business requirement.
- Use `UpdateReport(...)` only when updating already created unit.
- Put one clear responsibility on each barcode field. 

## AI notes
- XRBarCode uses `Tag` to bind to script variable.
- QR code is configured by report designer symbology, not by a special script function.
- For new data rows use `OutputReport(...)`; for last rendered unit use `UpdateReport(...)`.
- Ensure variable names in script and `Tag` names in `.repx` match exactly. 