---
title: Sql2Json
module: web
topic: json
applies_to: CSCS_WPF
version: 1
---

# Sql2Json

## Purpose
`Sql2Json(2Darray)` converts a two-dimensional SQL result array into a JSON string.  
It is intended for turning `SqlQuery(...)` output into JSON that can be returned from a web endpoint or logged for debugging .

## Syntax
```internal-script
res = Sql2Json(2Darray);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `2Darray` | 2D array | yes | Result returned by `SqlQuery(...)` | 

## Returns
Returns `res` as JSON string .

## Basic example
```internal-script
SQLConnectionString = ReadConfig("SqlConnectionString");
sqlString = "SELECT name FROM master.sys.databases";
sqlResult = SqlQuery(sqlString);

print(Sql2Json(sqlResult));
json = Sql2Json(sqlResult);
```

This is the documented manual pattern: query SQL data, convert the 2D result set into JSON string, then print or return it .

## Typical endpoint example
```internal-script
CreateEndpoint("GET", "/databases", "getDatabases");

function getDatabases
{
    SQLConnectionString = ReadConfig("SqlConnectionString");
    sqlString = "SELECT TOP 5 name FROM master.sys.databases";
    sqlResult = SqlQuery(sqlString);

    jsonString = Sql2Json(sqlResult);

    headers = {};
    headers["Content-Type"] = "application/json";

    return Response(headers, jsonString, 200);
}
```

## Example with application table
```internal-script
function getProducts
{
    SQLConnectionString = ReadConfig("SqlConnectionString");
    sqlString = "SELECT TOP 10 productNumber, productName, price FROM products";
    sqlResult = SqlQuery(sqlString);

    jsonString = Sql2Json(sqlResult);

    headers = {};
    headers["Content-Type"] = "application/json";

    return Response(headers, jsonString, 200);
}
```

## Relationship to other JSON functions
- `Sql2Json(...)` converts SQL 2D result array into JSON string.
- `SerializeJson(...)` serializes a hash-table or array into JSON string.
- `DeserializeJson(...)` parses JSON string into runtime array or hash-table .

## When to use
Use `Sql2Json(...)` when:
- source data already comes from `SqlQuery(...)`
- you want a direct SQL-result-to-JSON conversion
- endpoint should return query result without manually rebuilding response object 

## When not to use
Do not use `Sql2Json(...)` when:
- data is not a 2D SQL result
- you already have a hash-table or array ready for `SerializeJson(...)`
- you need custom reshaping before JSON output 

## Common mistakes
- Passing non-SQL array into `Sql2Json(...)`.
- Expecting a runtime object instead of a JSON string.
- Forgetting to set `Content-Type` to `application/json` when returning result.
- Using `Sql2Json(...)` where custom object serialization would be clearer .

## Best practices
- Use `ReadConfig(...)` to obtain connection string instead of hardcoding it.
- Keep SQL query explicit and predictable.
- Return resulting JSON through `Response(...)` in web handlers.
- Use `Sql2Json(...)` for fast read-only endpoint output from SQL result sets. 

## AI notes
- `Sql2Json(...)` takes the direct result of `SqlQuery(...)`.
- It returns JSON string, not parsed object.
- It is best suited for API-style read endpoints.
- For custom nested JSON output, prefer manual structure plus `SerializeJson(...)`. 
