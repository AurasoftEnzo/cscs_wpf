---
title: Report Data Source
module: reporting
topic: data-source
applies_to: CSCS_WPF
version: 1
---

# Report Data Source

## Purpose
`SetReportDataSource(reportHandle, sourceType, connectionOrData, query)` replaces the existing table data in a report with external structured data.  
This mechanism supports report binding from SQL, JSON, CSV, or XML sources .

## Syntax
```internal-script
SetReportDataSource(reportHandle, sourceType, connectionOrData, query);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `reportHandle` | integer | yes | Handle of the target report |
| `sourceType` | string | yes | Data source type: `SQL`, `JSON`, `CSV`, or `XML` |
| `connectionOrData` | string | yes | Connection string or source payload/path depending on source type |
| `query` | string | yes | SQL query or source-specific selection expression | 

## Returns
Returns `Variable.EmptyInstance` .

## Special table requirements
The data table should contain extra columns:
- `rowid`
- `parentid` 

These are used for row organization and parent-child grouping in report structures .

## SQL example
```internal-script
sourceType = "SQL";
connectionOrData = "Server=localhost;User Id=sa;Password=mypass;Initial Catalog=database1;";

query = "SELECT 
    STR(productNumber) as field1,
    productName as field2,
    STR(price) as field3,
    price as amount,
    1 as rowid,
    CASE WHEN productNumber % 2 = 0 THEN 1 ELSE 2 END AS parentid
FROM products";

SetReportDataSource(reportHandle3, sourceType, connectionOrData, query);
```

## Purpose of rowid and parentid
- `rowid` identifies the row
- `parentid` links child rows to parent report items or sections 

This is especially useful in:
- invoice and invoice-line layouts
- grouped master-detail reports
- nested detail tables 

## Typical workflow
1. validate template if needed
2. create report using `SetupReport(...)`
3. optionally create subreports
4. set parameters
5. inject external data using `SetReportDataSource(...)`
6. print or export report 

## Validation example
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

## Example with report and subreports
```internal-script
reportHandle = SetupReport("startersimple6.repx");
reportHandle2 = SetupReport("startersimple6invoice.repx", reportHandle);
reportHandle3 = SetupReport("startersimple6detail.repx", reportHandle2);

ConfigureReportOutput(reportHandle, "PageSize", "A4");

SetReportParameter(reportHandle, "reportTitle", reportTitle);
SetReportParameter(reportHandle, "reportDate", reportDate);
SetReportParameter(reportHandle, "logoPath", logoPath);

sourceType = "SQL";
connectionOrData = SQLConnectionString;
query = "SELECT
    invoiceNumber as field1,
    itemName as field2,
    itemType as field3,
    amount,
    rowid,
    parentid
FROM somePreparedView";

SetReportDataSource(reportHandle3, sourceType, connectionOrData, query);

PrintReport(reportHandle);
```

## Supported source types
The manual lists these `sourceType` values:
- `SQL`
- `JSON`
- `CSV`
- `XML` 

## Relationship to classic OutputReport
`SetReportDataSource(...)` is an alternative data loading mechanism for structured tabular data.  
Classic `OutputReport(...)` is still used when values are pushed manually through script variables row by row .

## When to prefer SetReportDataSource
Use `SetReportDataSource(...)` when:
- data already exists in queryable/tabular form
- report contains many rows
- master-detail grouping can be expressed through `rowid` and `parentid`
- external data format is already SQL, JSON, CSV, or XML 

## Common mistakes
- Omitting `rowid` and `parentid` columns.
- Using source type string not supported by the function.
- Expecting `SetReportDataSource(...)` to automatically create report controls.
- Mixing report field names and data source columns inconsistently .

## Best practices
- Match source column names to report field expressions clearly.
- Always validate report template before binding large structured data.
- Use prepared SQL views or query aliases for stable field naming.
- Keep parent-child linking explicit through `parentid`. 

## AI notes
- `SetReportDataSource(...)` is for structured external data injection.
- `rowid` and `parentid` are important parts of the expected table structure.
- It complements, but does not replace, `SetupReport(...)`.
- For manual per-row scripting use `OutputReport(...)`; for tabular bulk binding use `SetReportDataSource(...)`. 