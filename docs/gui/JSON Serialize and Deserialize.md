---
title: JSON Serialize and Deserialize
module: web
topic: json
applies_to: CSCS_WPF
version: 1
---

# JSON Serialize and Deserialize

## Purpose
CSCS_WPF supports conversion between internal data structures and JSON strings through:
- `SerializeJson(object)`
- `DeserializeJson(jsonString)` 

These functions are used when:
- returning JSON from web endpoints
- consuming JSON from external services
- preparing request payloads for `WebRequest(...)`
- converting parsed JSON into arrays or hash-tables for further processing 

## Functions covered
| Function | Purpose |
|---|---|
| `SerializeJson(object)` | Converts a hash-table or array into JSON string |
| `DeserializeJson(jsonString)` | Converts JSON string into array or hash-table | 

## SerializeJson

### Purpose
`SerializeJson(object)` serializes a CSCS_WPF object into JSON string.  
The source object can be a hash-table, array, or nested structure .

### Syntax
```internal-script
jsonString = SerializeJson(object);
```

### Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `object` | hash-table or array | yes | Object to serialize into JSON string | 

### Returns
Returns JSON string .

### Basic example
```internal-script
payload = {};
payload["status-code"] = 200;
payload["status-text"] = "OK";

headers = {};
headers["Content-Type"] = "application/json";
headers["Cache-Control"] = "no-cache";
headers["X-Custom-Header"] = "CustomValue";

payload["headers"] = headers;

jsonString = SerializeJson(payload);
print(jsonString);
```

### Array example
```internal-script
arr = {};
arr.Add("A");
arr.Add("B");
arr.Add("C");

jsonString = SerializeJson(arr);
```

### Nested object example
```internal-script
customer = {};
customer["code"] = "C001";
customer["name"] = "Test Customer";

invoice = {};
invoice["number"] = "INV-1";
invoice["customer"] = customer;
invoice["total"] = 150.75;

jsonString = SerializeJson(invoice);
```

## DeserializeJson

### Purpose
`DeserializeJson(jsonString)` parses JSON string and converts it into CSCS_WPF array or hash-table .

### Syntax
```internal-script
result = DeserializeJson(jsonString);
```

### Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `jsonString` | string | yes | JSON string to deserialize | 

### Returns
Returns array or hash-table with parsed data .

### Basic example
```internal-script
jsonData = DeserializeJson("[{\"key1\":\"value1\",\"key2\":\"value2\"},{\"key1\":\"value3\",\"key2\":\"value4\"}]");

item0 = jsonData;
item1 = jsonData;[2]

item0value1 = item0["key1"];
item0value2 = item0["key2"];
item1value1 = item1["key1"];
item1value2 = item1["key2"];
```

### Object example
```internal-script
jsonString = "{\"status\":\"ok\",\"message\":\"done\"}";
obj = DeserializeJson(jsonString);

status = obj["status"];
message = obj["message"];
```

## Typical endpoint pattern
Use `SerializeJson(...)` before returning JSON from an endpoint .

```internal-script
function getStatus
{
    payload = {};
    payload["status"] = "ok";
    payload["time"] = "2026-05-28 10:00:00";

    jsonString = SerializeJson(payload);

    headers = {};
    headers["Content-Type"] = "application/json";
    headers["Access-Control-Allow-Origin"] = "*";

    return Response(headers, jsonString, 200);
}
```

## Typical outbound request pattern
Use `SerializeJson(...)` to prepare request body before calling `WebRequest(...)` .

```internal-script
payload = {};
payload["billNumber"] = "12345";
payload["amount"] = 150.75;
payload["currency"] = "EUR";

body = SerializeJson(payload);

headers = {};
headers["Authorization"] = "Bearer token123";

res = WebRequest(
    "POST",
    "https://example.test/api/bills",
    body,
    "req-1",
    onSuccess,
    onFailure,
    "application/json",
    headers,
    15000,
    false
);
```

## Common mistakes
- Passing non-JSON string into `DeserializeJson(...)`.
- Returning a hash-table directly from endpoint without serializing it first.
- Building JSON manually with string concatenation instead of using `SerializeJson(...)`.
- Assuming deserialized result is always array; it can also be hash-table .

## Best practices
- Prefer `SerializeJson(...)` over manual string assembly.
- After `DeserializeJson(...)`, inspect whether the result is an array or hash-table before accessing values.
- Use explicit keys for API payloads.
- Keep JSON property naming consistent across requests and responses. 

## AI notes
- `SerializeJson(...)` expects array or hash-table, not arbitrary raw code.
- `DeserializeJson(...)` returns runtime structures, not plain text.
- For JSON endpoints always combine `SerializeJson(...)` with `Response(...)` and `Content-Type = application/json`.
- Do not assume that JSON output is automatically pretty-printed or schema-validated. 