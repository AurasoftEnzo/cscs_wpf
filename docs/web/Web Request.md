---
title: WebRequest
module: web
topic: outbound-http
applies_to: CSCS_WPF
version: 1
---

# WebRequest

## Purpose
`WebRequest(...)` sends an HTTP request to an external URL.  
It supports success and failure callbacks, custom headers, configurable content type, timeout and synchronous or asynchronous behavior.

## Syntax
```internal-script
result = WebRequest(method, url, load, trackingId, onSuccess, onFailure, contentType, headers, timeout, justFire);
```

## Arguments
| Argument | Type | Required | Description |
|---|---|---|---|
| `method` | string | yes | HTTP method such as `GET`, `PUT`, `POST`, `DELETE`, `TRACE`, `OPTIONS` |
| `url` | string | yes | Target URL |
| `load` | string | yes | Request payload as string |
| `trackingId` | string | yes | Tracking identifier passed to callbacks |
| `onSuccess` | expression / function | yes | Function executed on success |
| `onFailure` | expression / function | yes | Function executed on failure |
| `contentType` | string | no | Payload content type, for example `application/json` |
| `headers` | dictionary | no | Request headers |
| `timeout` | number | no | Timeout in milliseconds |
| `justFire` | boolean | no | If `true`, fire asynchronously without waiting for response; if `false`, wait and return response message | 

## Callback contract
Both callbacks receive:
- `tracking`
- `responseCode`
- `res` 

Example callback definitions:
```internal-script
function sendSuccessful(tracking, responseCode, res)
{
    MSG("Send Successful", tracking, responseCode, res);
}

function sendFailure(tracking, responseCode, res)
{
    MSG("Send Failure", tracking, responseCode, res);
}
```

## Return behavior
If `justFire = false`, `WebRequest(...)` runs synchronously and returns the response message.  
If `justFire = true`, the function sends the request asynchronously and does not return a response payload.

## Basic POST example
```internal-script
function sendSuccessful(tracking, responseCode, res)
{
    MSG("Send Successful", tracking, responseCode, res);
}

function sendFailure(tracking, responseCode, res)
{
    MSG("Send Failure", tracking, responseCode, res);
}

headers = {};
headers["Authorization"] = "APIKey 123456";

load = "{\"billNumber\":\"12345\",\"billDate\":\"2023-10-01\",\"amount\":150.75}";

result = WebRequest(
    "POST",
    "https://example.test/api/import",
    load,
    "request1",
    sendSuccessful,
    sendFailure,
    "application/json",
    headers,
    15000,
    false
);
```

## Basic GET example
```internal-script
function onSuccess(tracking, responseCode, res)
{
    MSG("OK", tracking, responseCode, res);
}

function onFailure(tracking, responseCode, res)
{
    MSG("ERR", tracking, responseCode, res);
}

headers = {};
headers["Authorization"] = "Bearer token123";

result = WebRequest(
    "GET",
    "https://example.test/api/status",
    "",
    "status-check",
    onSuccess,
    onFailure,
    "application/json",
    headers,
    5000,
    false
);
```

## Asynchronous fire-and-forget example
```internal-script
headers = {};
headers["Content-Type"] = "application/json";

payload = "{\"event\":\"sync-started\"}";

WebRequest(
    "POST",
    "https://example.test/api/log",
    payload,
    "log-1",
    onSuccess,
    onFailure,
    "application/json",
    headers,
    3000,
    true
);
```

## Use when
Use `WebRequest(...)` when:
- calling external REST services
- sending JSON payloads
- integrating with approval, billing or partner APIs
- requiring callback-based success/failure handling
- optionally needing synchronous response retrieval 

## Common mistakes
- Passing non-string payload where string is expected.
- Forgetting to serialize JSON payload before sending.
- Using `justFire = true` and still expecting a return value.
- Forgetting to define callback functions with the correct parameters.
- Omitting required authorization headers.

## Best practices
- Build payload separately before request execution.
- Store headers in a dictionary.
- Use explicit `trackingId` values for logging and debugging.
- Use `justFire = false` when the response must be processed immediately.
- Use `justFire = true` for background notifications or non-blocking integrations.
## AI notes
- Do not invent callback signatures.
- `load` is string payload, not arbitrary object.
- Use serialized JSON string for JSON requests.
- `headers` should be a dictionary.
- `timeout` is in milliseconds.
- `justFire` controls sync versus async behavior and also whether the function returns response text.