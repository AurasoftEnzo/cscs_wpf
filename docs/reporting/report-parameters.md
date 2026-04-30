This document should separate the report parameters from ordinary field binding. In the manual, it is clearly shown that the parameter is created in .repx, then used in the Parameters.someName expression, and from the script it is filled via SetReportParameter; retrieval goes via GetReportParameter.

---
title: Report Parameters
module: reporting
topic: parameters
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# Report Parameters

## Purpose

This document describes how to use report parameters in DevExpress '.repx' templates and how they are populated from CSCS_WPF script.

Report parameters are used for one-time or global values such as:
- the title of the report,
- the date of printing,
- the user who prints,
- path to the logo,
- Configuration values that are not part of the repeated rows.

## Main concepts

- The parameter is first defined in the '.repx' template.
- In the report designer, it is used through the expression 'Parameters.parameterName'.
- In the script, it is populated via 'SetReportParameter'.
- The parameter can also be read via 'GetReportParameter' [file:104].

## When to use parameters

It uses parameters when the value:
- It's not tied to every detail
- belongs to the header, footer or configuration,
- should be set once for the entire report,
- is not naturally bound to 'OutputReport' rows.

## Create parameter in .repx

In the report designer:
1. open **Field List**,
2. Right-click on **Parameters**,
3. add a new parameter,
4. Give a name and type,
5. set 'Show in parameters panel = false' [file:104] if necessary.

## Use parameter in .repx

For a control such as 'XRLabel':
- "Tag" can be left empty,
- the control expression uses 'Parameters.parameterName' [file:104].

### Example expression

```text
Parameters.reportPrintingPerson
```

## SetReportParameter

'SetReportParameter' sets the value of the existing report parameter [file:104].

### Syntax

```internal-script
SetReportParameter(reportHandle, parameterName, value);
```

### Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `reportHandle` | int | yes | handle reporta |
| `parameterName` | string | yes | parameter name defined in '.repx' |
| `value` | any | yes | parameter value |

### Example

```internal-script
reportPrintingPerson = "Dalibor";
SetReportParameter(reportHandle, "reportPrintingPerson", reportPrintingPerson);
```

## GetReportParameter

'GetReportParameter' retrieves the current value of the report parameter [file:104].

### Syntax

```internal-script
value = GetReportParameter(reportHandle, parameterName);
```

### Example

```internal-script
value = GetReportParameter(reportHandle, "reportPrintingPerson");
MessageBox(value);
```

## Common usage patterns

### Header text

```internal-script
reportTitle = "INVOICES 2026";
SetReportParameter(reportHandle, "reportTitle", reportTitle);
```

### Print date

```internal-script
reportDate = "Report date " + Now("dd.MM.yyyy");
SetReportParameter(reportHandle, "reportDate", reportDate);
```

### Logo path

```internal-script
logoPath = "C:\\logo.png";
SetReportParameter(reportHandle, "logoPath", logoPath);
```

## Example: label bound to parameter

### . REPX setup

- Add parameter: `reportPrintingPerson`
- Type: `String`
- `Show in parameters panel = false`
- Add label to `ReportHeader`
- Set label expression to `Parameters.reportPrintingPerson` [file:104]

### CSCS_WPF script

```internal-script
reportPrintingPerson = "Dalibor";
SetReportParameter(reportHandle, "reportPrintingPerson", reportPrintingPerson);
```

## Example: picture box bound to parameter

### . REPX setup

- Add parameter: `logoPath`
- Add `PictureBox`
- Set expression to `Parameters.logoPath` [file:104]

### CSCS_WPF script

```internal-script
logoPath = "C:\\logo.png";
SetReportParameter(reportHandle, "logoPath", logoPath);
```

## Parameters vs output fields

| Use case | Preferred mechanism |
|---|---|
| global title | parameter |
| print date | parameter |
| company logo path | parameter |
| invoice number in each row | output field |
| item code in detail row | output field |
| total label in footer | parameter or `UpdateReport`, depending on template |

## Common mistakes

- Using 'SetReportParameter' for a parameter that is not defined in '.repx'.
- Expecting the parameter to automatically fill in the detail rows to be generated via 'OutputReport'.
- Mixing 'Tag' field bindings and 'Parameters.*' expressions.
- AI invents parameters that don't exist in template [file:104].

## AI notes

- Do not use parameters as a substitute for all report fields.
- Parameters are best for header, footer, logo, and one-time values.
- The parameter name must be identical to the one in '.repx'.
- If it is not known whether a parameter exists, this should be explicitly marked as an assumption.
- When binding a variable in a .repx report, only the target element must define the Tag property, and its value MUST exactly     match the variable name.
