---
title: Report Logo Picture Box
module: reporting
topic: logo-picture
applies_to: CSCS_WPF
version: 1
---

# Report Logo Picture Box

## Purpose
CSCS_WPF supports displaying a logo or picture in DevExpress reports using a PictureBox control and a report parameter such as `logoPath`.  
The image path is provided from script with `SetReportParameter(...)`, while the `.repx` template binds the PictureBox to that parameter .

## Core concept
Logo images are not typically bound through `Tag` like labels and barcodes.  
Instead, the recommended pattern is:
1. create report parameter
2. configure PictureBox expression to use the parameter
3. set parameter value from script 

## Template setup

### Step 1: create parameter
In report designer:
- open Field List
- right click `Parameters`
- add parameter
- `Name = logoPath`
- `Type = String`
- `Show in parameters panel = false` 

### Step 2: create PictureBox
Place PictureBox on report, typically in `ReportHeader` band .

Recommended settings:
- `Name = companyLogo`
- `Tag = leave empty`
- `Sizing = Zoom Image`
- `Alignment = Top Left` 

### Step 3: bind PictureBox to parameter
In PictureBox configuration:
- `Expression 1 = leave empty`
- `Image Url = leave empty`
- `Expression 2 = Parameters.logoPath` 

## Script usage
```internal-script
logoPath = "C:\\logo.png";
SetReportParameter(reportHandle, "logoPath", logoPath);
```

## Minimal example
```internal-script
reportHandle = SetupReport("MainReport.repx");

logoPath = "C:\\company\\logo.png";
SetReportParameter(reportHandle, "logoPath", logoPath);

OutputReport(reportHandle);
PrintReport(reportHandle);
DisposeReport(reportHandle);
```

## Example with title and date
```internal-script
reportHandle = SetupReport("startersimple6.repx");

reportTitle = "INVOICES 2026";
reportDate = "Report date " + Now("dd.MM.yyyy");
logoPath = "C:\\logo.png";

SetReportParameter(reportHandle, "reportTitle", reportTitle);
SetReportParameter(reportHandle, "reportDate", reportDate);
SetReportParameter(reportHandle, "logoPath", logoPath);

PrintReport(reportHandle);
DisposeReport(reportHandle);
```

## Alternate picture note
The manual also shows an example where a picture box uses a `Tag` property containing a variable name such as `lokacijaSlike`, and the script sets that variable to image path .  
However, the explicit parameter-based pattern with `logoPath` is the clearest documented setup for report-wide logo usage .

## When to use
Use this pattern when:
- one report-wide logo or header image must be shown
- image path is known at runtime
- different companies or environments require different logos
- the image is part of report header branding 

## Common mistakes
- Leaving PictureBox expression unbound to parameter.
- Setting `Tag` when the template is built for parameter-based picture loading.
- Forgetting to set `logoPath` from script.
- Using invalid file path to image .

## Best practices
- Put company logo in `ReportHeader`.
- Use a dedicated report parameter such as `logoPath`.
- Keep PictureBox sizing on `Zoom Image`.
- Treat logo as report-wide parameter, not row-level data. 

## AI notes
- Preferred logo binding pattern is parameter-based.
- `SetReportParameter(reportHandle, "logoPath", logoPath)` is the key runtime step.
- PictureBox should be configured in `.repx`, not created dynamically from script.
- Do not mix row-level label binding rules with report-wide logo parameter binding. 