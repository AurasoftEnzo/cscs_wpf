---
title: SerializeJson
module: web
topic: json
applies_to: CSCS_WPF
version: 1
---

# SerializeJson

## Purpose
`SerializeJson(object)` serializes a hash-table or array into a JSON string.  
It is used when endpoint output must be returned as JSON response body.

## Syntax
```internal-script
string = SerializeJson(object);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `object` | hash-table or array | yes | Object to serialize into JSON |

## Returns
Returns JSON string.

## Important capability
The manual states that a hash-table can have any depth, so nested structures are supported during serialization.

## Basic example
```internal-script
object = {};
object["status-code"] = 200;
object["status-text"] = "OK";

headers = {};
headers["Content-Type"] = "application/json";
headers["Cache-Control"] = "no-cache";

object["headers"] = headers;

string = SerializeJson(object);
print(string);
```

## Array example
```internal-script
arr = {};
arr.Add(1);
arr.Add(2);
arr.Add(3);

jsonString = SerializeJson(arr);
```

## Endpoint example
```internal-script
CreateEndpoint("GET", "/info", "getInfo");

function getInfo
{
    result = {};
    result["application"] = "CSCSWEB";
    result["version"] = "1.0";
    result["ok"] = true;

    headers = {};
    headers["Content-Type"] = "application/json";

    return Response(headers, SerializeJson(result), 200);
}
```

## When to use
Use `SerializeJson(...)` when:
- you already have runtime dictionary or array data
- endpoint returns API response
- custom JSON structure must be created in code

## When not to use
Do not use it when:
- source is raw SQL 2D result and direct `Sql2Json(...)` is sufficient
- you need parsed object rather than output string
- response body should be HTML rather than JSON

## Best practices
- Build full response object first, then serialize once.
- Return serialized string through `Response(...)`.
- Use nested dictionaries for structured API output.
- Keep keys explicit and stable for API consumers.

## Common mistakes
- Forgetting to serialize before sending JSON response.
- Assuming result is runtime object instead of string.
- Mixing HTML body with JSON content type.
- Passing unsupported ad-hoc structure instead of array/hash-table.

## AI notes
- Preferred JSON serializer for arrays and dictionaries.
- Returns string.
- Works well with `Response(...)`.
- Use `Sql2Json(...)` instead only for direct SQL-result conversion.
