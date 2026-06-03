---
title: ReadConfig
module: web
topic: configuration
applies_to: CSCS_WPF
version: 1
---

# ReadConfig

## Purpose
`ReadConfig(configKey)` retrieves configuration values from `appsettings.json`.  
It is used in CSCS_WPF web scripts to access settings such as template directories, SQL connection strings, and other application configuration values .

## Syntax
```internal-script
res = ReadConfig(configKey);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `configKey` | string | yes | Key of the configuration property to retrieve | 

## Returns
Returns `res` as string containing the configuration property value .

## Basic example
```internal-script
templatesDirectory = ReadConfig("TemplatesDirectory");
```

The manual explicitly shows this usage for reading template directory from configuration .

## SQL connection example
```internal-script
SQLConnectionString = ReadConfig("SqlConnectionString");
sqlString = "SELECT name FROM master.sys.databases";
sqlResult = SqlQuery(sqlString);
```

This same pattern is also shown in the `Sql2Json(...)` example, where SQL connection settings are read from configuration instead of hardcoding them .

## Example for template path
```internal-script
templatesDirectory = ReadConfig("TemplatesDirectory");
templatePath = templatesDirectory + "\\home.html";

templateHndl = LoadTemplate(templatePath);
htmlString = RenderHtml(templateHndl);
```

## Endpoint example
```internal-script
CreateEndpoint("GET", "/config-test", "configTest");

function configTest
{
    templatesDirectory = ReadConfig("TemplatesDirectory");

    result = {};
    result["templatesDirectory"] = templatesDirectory;

    headers = {};
    headers["Content-Type"] = "application/json";

    return Response(headers, SerializeJson(result), 200);
}
```

## Typical use cases
Use `ReadConfig(...)` for:
- SQL connection strings
- template root directories
- server-related settings
- configurable paths and environment-specific values 

## Relationship to other web functions
`ReadConfig(...)` is often used together with:
- `SqlQuery(...)`
- `Sql2Json(...)`
- `LoadTemplate(...)`
- `RenderHtml(...)`
- `CreateEndpoint(...)` 

## Common mistakes
- Using wrong configuration key name.
- Expecting non-string return type.
- Hardcoding environment values instead of placing them in `appsettings.json`.
- Assuming `ReadConfig(...)` validates the returned content automatically .

## Best practices
- Keep connection strings and directories in configuration, not in scripts.
- Use descriptive config keys such as `SqlConnectionString` and `TemplatesDirectory`.
- Read config once per handler when possible and reuse the value.
- Validate or inspect returned values before using them in critical operations. 

## AI notes
- `ReadConfig(...)` always takes one key name.
- It returns string configuration value.
- It is the preferred way to retrieve environment-dependent settings in web scripts.
- Use it instead of hardcoded paths and connection strings whenever possible. 

