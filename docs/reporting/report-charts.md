---
title: Report Charts
module: reporting
topic: charts
applies_to: CSCS_WPF
version: 1
---

# Report Charts

## Purpose
CSCS_WPF supports chart rendering inside DevExpress reports through XRChart controls defined in `.repx` templates.  
Charts are populated from script variables using the chart control `Tag` property as a prefix for related data arrays .

## Supported chart concepts
Reports support:
- line charts
- bar charts
- pie charts
- doughnut charts 

Line and bar charts can contain multiple series.  
Pie and doughnut charts can contain only one series defined in the report designer .

## Core binding rule
In the report designer, set the XRChart `Tag` property to a prefix name such as:
```text
chartArray
```

Then the script must define arrays using that same prefix:
- `chartArrayVal1`
- `chartArrayVal2`
- `chartArrayVal3`
- `chartArrayArgs` 

## Required arrays

### Shared arguments array
The horizontal axis labels are stored in:
```internal-script
chartArrayArgs
```

### Value arrays
Series values are stored in:
```internal-script
chartArrayVal1
chartArrayVal2
chartArrayVal3
```

For one-series charts use only `Val1`.  
For multi-series line or bar charts define as many `ValN` arrays as needed .

## Line chart example

### Template setup
In `.repx`:
- put chart control on the report
- choose line chart type
- add required series in designer
- set chart `Tag = chartArray` 

### Script example
```internal-script
chartArrayVal1
chartArrayVal1.Add(5)
chartArrayVal1.Add(50)
chartArrayVal1.Add(35)

chartArrayVal2
chartArrayVal2.Add(1)
chartArrayVal2.Add(2)
chartArrayVal2.Add(5)

chartArrayArgs
chartArrayArgs.Add("a")
chartArrayArgs.Add("b")
chartArrayArgs.Add("x")
```

## Bar chart example
```internal-script
salesByMonthVal1
salesByMonthVal1.Add(1200)
salesByMonthVal1.Add(1500)
salesByMonthVal1.Add(1800)

salesByMonthArgs
salesByMonthArgs.Add("Jan")
salesByMonthArgs.Add("Feb")
salesByMonthArgs.Add("Mar")
```

If chart `Tag` in `.repx` is `salesByMonth`, the chart binds to:
- `salesByMonthVal1`
- `salesByMonthArgs` 

## Pie chart example
Pie charts support one series only .

### Template setup
In `.repx`:
- choose pie or doughnut chart
- define one series only
- set `Tag = productShare` 

### Script example
```internal-script
productShareVal1
productShareVal1.Add(45)
productShareVal1.Add(30)
productShareVal1.Add(25)

productShareArgs
productShareArgs.Add("Electronics")
productShareArgs.Add("Furniture")
productShareArgs.Add("Plants")
```

## Pie label pattern
For pie charts, configure label pattern in the report designer on:
`Chart -> Series1 -> Label -> TextPattern` 

Recommended pattern:
```text
{A}: {V} ({VP:P0})
```

The manual describes it conceptually as:
- `A` = argument
- `V` = value
- `VP` = value percentage
- `VPP0` = formatted percentage 

## Chart usage inside report workflow
Charts are populated through variables, then output occurs with normal report functions:
1. `SetupReport(...)`
2. fill arrays and variables
3. `OutputReport(...)`
4. optionally `UpdateReport(...)`
5. `PrintReport(...)` or `ExportReport(...)` 

## Example with main report chart
```internal-script
reportHandle = SetupReport("SalesOverview.repx");

salesChartVal1
salesChartVal1.Add(100)
salesChartVal1.Add(130)
salesChartVal1.Add(170)

salesChartArgs
salesChartArgs.Add("Q1")
salesChartArgs.Add("Q2")
salesChartArgs.Add("Q3")

OutputReport(reportHandle);
PrintReport(reportHandle);
DisposeReport(reportHandle);
```

## Example with subreport chart
Charts can also be used inside subreports if the subreport template contains chart control with its own `Tag` prefix .

```internal-script
report1hndl = SetupReport("MainReport.repx");
report2hndl = SetupReport("InvoiceSubreport.repx", report1hndl);

subreportChartVal1
subreportChartVal1.Add(100)
subreportChartVal1.Add(100)
subreportChartVal1.Add(130)

subreportChartArgs
subreportChartArgs.Add("a")
subreportChartArgs.Add("b")
subreportChartArgs.Add("c")

OutputReport(report2hndl);
```

## Validation rules
- `Args` array and each `ValN` array should have the same number of members.
- The `Tag` value in report designer must match the script prefix exactly.
- Pie and doughnut charts must use only one value series.
- Multi-series charts require series to be defined in the designer in advance .

## Common mistakes
- Using a chart `Tag` that does not match script variable prefixes.
- Defining `Args` with different item count than value arrays.
- Trying to use multiple series in pie chart.
- Forgetting to define chart series in the report designer before binding data .

## Best practices
- Use short and descriptive chart tag prefixes such as `salesChart`, `vatChart`, `invoiceLinesChart`.
- Keep arguments and values aligned by index.
- Use one naming convention consistently across all reports.
- Prefer charts for summaries and trends, not dense transactional detail. 

## AI notes
- Report chart binding is convention-based through `Tag`.
- If `Tag = chartArray`, use `chartArrayVal1`, `chartArrayVal2`, `chartArrayArgs`.
- Pie and doughnut charts allow only one series.
- Designer configuration and script variable names must match exactly. 