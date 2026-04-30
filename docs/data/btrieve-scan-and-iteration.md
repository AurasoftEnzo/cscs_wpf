SCAN is important because AI often makes mistakes about start values, while conditions, for filters, and scope limits. The documentation clearly states that SCAN fills the buffer at each iteration, uses the order key, can filter via for stringSQL, stop via while stringC, and limit the number of records with scope n 20; Break and continue are supported.

---
title: Btrieve Scan and Iteration
module: data
topic: btrieve-iteration
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# Btrieve Scan and Iteration

## Purpose

This document describes how to use 'SCAN' to iterate through table records in CSCS_WPF Btrieve model.

'SCAN' is a loop that fills the memory buffer for each record found and executes a block of commands. It is used when multiple records need to be passed sorted by key, with the possibility of additional filtering and limiting the number of lines read.

## Main concepts

- 'SCAN' works on an already open table handle.
- Each iteration fills the buffer with the current record.
- The order is specified by 'keyName' or 'keyNum'.
- 'start' specifies which key value the iteration starts from .
- 'while stringC' stops the iteration when the condition is no longer met.
- 'for stringSQL' filters records to skip.
- 'scope' limits the number of records read, e.g. 'n 20'.
- 'break' breaks the loop, 'continue' skips one iteration.

## Syntax

```internal-script
SCAN handle, keyNameOrKeyNum, startString, whileCondition, forStringSQL, scope {
    statements;
}
```

## Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `handle` | int | yes | Handle Open Tables |
| `keyNameOrKeyNum` | string | yes | Key name or number |
| `startString` | string | yes | Key Segment Initial Value |
| `whileCondition` | string/C expression | yes | Condition to continue iteration |
| `forStringSQL` | string | yes | SQL-like Record Skip Filter |
| `scope` | string | yes | Range limit, e.g. 'n 20' |

## Usage pattern

Typical pattern:

1. open the table with 'OPENV',
2. Select the key for sorting,
3. set the initial value,
4. set a 'while' condition,
5. If necessary, set the 'for' filter,
6. Limit the number of records with 'scope',
7. Process the data from the buffer in the loop body.

## Example from canonical pattern

```internal-script
function forFilter() {
    return VEZMSIRINA > 130;
}

SCAN(tableHndl, "3", "1081620 00", "true", "VEZMRNALOG != 108162", "n 1") {
    MessageBox(VEZMVEZNIBROJ);
}
```

## More explicit example

```internal-script
DEFINE tableHndl type i;

tableHndl = OPENV("invoiceLines", "DB1");

SCAN(tableHndl, "InvoiceLineinvoiceLineNumber", "1,1", "invoiceNumber == 1", "amount > 0", "n 20") {
    MessageBox(invoiceNumber);
    MessageBox(invoiceLineNumber);
    MessageBox(amount);
}
```

## Buffer behavior

During each iteration:
- the buffer is filled with the current record,
- the script can read columns as variables/buffer fields,
- 'break' and 'continue' work as in other loops.

## Using break and continue

```internal-script
SCAN(tableHndl, "InvoiceLineinvoiceLineNumber", "1,1", "true", "", "n 50") {
    if (amount == 0) {
        continue;
    }

    if (invoiceNumber > 1000) {
        break;
    }

    MessageBox(amount);
}
```

## Scope

'scope' limits how many records will be read. Example 'n 20' means "next 20 records".

## While vs for filter

| Mechanism | Role |
|---|---|
| `while string(C#)` | stops iteration when condition becomes false |
| `for string(SQL)` | Skips records that don't meet the filter |

This means that 'while' controls the iteration boundary, and 'for' controls the additional inclusion criterion.

## Related iteration forms

In addition to 'SCAN', there are also standard control structures in the language:
- `for(init, condition, step) { ... }`,
- `for item array { ... }`,
- `while(condition) { ... }`,
And then there is the need for a statement (104).

## Common mistakes

- Using 'SCAN' without 'OPENV'.
- Incorrect number of segments in 'startString'.
- Mixing the role of 'while' and 'for' filters.
- Expecting 'SCAN' not to fill the buffer, even though it fills it with each iteration.
- Forgotten 'break' in cases where logic needs a manual break.
- Assuming that some arguments are optional, even though the document states that all the constituent parts of the form are essential.

## AI notes

- To pass through multiple DB records, prefer 'SCAN' instead of manual 'FINDV(..., "n")' when the target is a span loop.
- Do not invent new scope formats.
- Always explicitly define key, start, while, filter and scope.
- When generating examples, clearly separate the Btrieve iteration from the standard 'for' loops of the language.
