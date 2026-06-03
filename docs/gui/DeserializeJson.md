---
title: DeserializeJson
module: web
topic: json
applies_to: CSCS_WPF
version: 1
---

# DeserializeJson

## Purpose
`DeserializeJson(JSONstring)` deserializes a JSON string into runtime CSCS array or hash-table data.  
It is used when incoming JSON payload must be read and processed inside script logic.

## Syntax
```internal-script
res = DeserializeJson(JSONstring);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `JSONstring` | string | yes | JSON string to be deserialized |

## Returns
Returns array or hash-table with data from JSON string.

## Basic example
```internal-script
JSONdata = DeserializeJson("[{\"key1\":\"value1\",\"key2\":\"value2\"},{\"key1\":\"value3\",\"key2\":\"value4\"}]");

item0 = JSONdata;
item1 = JSONdata;[2]

item0value1 = item0["key1"];
item0value2 = item0["key2"];
item1value1 = item1["key1"];
item1value2 = item1["key2"];
```

The manual example shows that deserialized JSON can be accessed as array items and dictionary entries.

## Example in POST endpoint
```internal-script
CreateEndpoint("POST", "/save-json", "saveJson");

function saveJson
{
    body = requestBody;
    data = DeserializeJson(body);

    code = data["code"];
    name = data["name"];

    result = {};
    result["status"] = "received";
    result["code"] = code;
    result["name"] = name;

    headers = {};
    headers["Content-Type"] = "application/json";

    return Response(headers, SerializeJson(result), 200);
}
```

## When to use
Use `DeserializeJson(...)` when:
- request body is JSON
- external service returns JSON that must be processed
- nested data must be navigated inside script

## When not to use
Do not use it when:
- request body is standard form content, use `GetValueFromForm(...)`
- you only need to return JSON, use `SerializeJson(...)`
- source is SQL result, use `Sql2Json(...)` if direct conversion is enough

## Best practices
- Deserialize once at the beginning of request handling.
- Validate required keys after deserialization.
- Keep clear distinction between arrays and dictionaries in parsed result.
- Re-serialize only the final response object.

## Common mistakes
- Passing form body instead of JSON string.
- Treating string as already parsed object.
- Mixing array indexing and dictionary key access incorrectly.
- Forgetting to validate missing fields after parse.

## AI notes
- `DeserializeJson(...)` converts JSON string into runtime structure.
- Use for JSON request bodies.
- Prefer `GetValueFromForm(...)` for form-encoded bodies.
- Pair naturally with `SerializeJson(...)` in request-response workflows.
