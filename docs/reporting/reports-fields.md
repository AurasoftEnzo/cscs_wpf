---
title: Report Validation and Fields
module: reporting
topic: validation-fields
applies_to: CSCS_WPF
version: 1
---

# Report Validation and Fields

## Purpose
CSCS_WPF provides two helper functions for report template inspection:
- `ValidateReportTemplate(...)`
- `GetReportFields(...)` 

These functions are useful before runtime output because they help confirm that the `.repx` template matches the expected script variables and report fields .

## Functions covered
| Function | Purpose |
|---|---|
| `ValidateReportTemplate(templatePath)` | Validates report template and returns errors |
| `GetReportFields(templatePath)` | Returns array of field names found in template | 

## ValidateReportTemplate

### Purpose
Performs detailed validation of a report template .

### Syntax
```internal-script
errors = ValidateReportTemplate(templatePath);
```

### Returns
Returns array of error messages, or empty array if template is valid .

### Basic example
```internal-script
errors = ValidateReportTemplate(tpath);
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

### Example with specific template
```internal-script
errors = ValidateReportTemplate(tpath + "startersimple6invoice.repx");
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

## GetReportFields

### Purpose
Returns field names found in a report template .

### Syntax
```internal-script
myFields = GetReportFields(templatePath);
```

### Returns
Returns array of strings .

### Example
```internal-script
myFields = GetReportFields(tpath + "startersimple5invoice.repx");
msg(myFields);
```

## Why validation matters
Validation is especially useful when:
- `.repx` files are edited independently from scripts
- multiple developers maintain reports
- report structure is complex
- master-detail templates depend on precise field names 

## Typical validation workflow
1. determine template path
2. call `ValidateReportTemplate(...)`
3. stop processing if validation errors exist
4. optionally inspect fields using `GetReportFields(...)`
5. continue with `SetupReport(...)` only when template is acceptable 

## Example integrated into report startup
```internal-script
templatePath = tpath + "startersimple6detail.repx";

errors = ValidateReportTemplate(templatePath);
if (Size(errors) > 0)
{
    MSG("Validation errors found!");
    for (i = 0; i < Size(errors); i++)
    {
        MSG("- " + errors[i]);
    }
    exit;
}

fields = GetReportFields(templatePath);
msg(fields);

reportHandle = SetupReport(templatePath);
```

## Relationship to runtime data
- `ValidateReportTemplate(...)` checks template consistency.
- `GetReportFields(...)` helps inspect what fields the template expects or contains.
- These functions do not print, export, or populate the report themselves .

## Common mistakes
- Skipping validation when templates change often.
- Assuming `GetReportFields(...)` also validates report correctness.
- Continuing to generate output even when validation returned errors.
- Using wrong template path .

## Best practices
- Validate complex templates before production use.
- Use `GetReportFields(...)` during template development and debugging.
- Place validation near the beginning of report script.
- Fail fast when validation returns errors. 

## AI notes
- `ValidateReportTemplate(...)` returns error array, not boolean.
- `GetReportFields(...)` is an inspection helper, not a validation substitute.
- These functions are most useful before `SetupReport(...)` and data output.
- Use them in report generation scripts that must be robust against `.repx` changes. 