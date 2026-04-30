This document should clearly explain to the AI how to open a table, how to read a record, how to check for an error, and how to close a handle. In the attached manual, OPENV returns the integer handle, FINDV performs fetch according to the options f/l/n/p/m/g, CLOSEV closes the work with the table, and FLERR returns the status of the last Btrie operation.WpfCSCS-UPUTE-3.docx
text
---
title: Btrieve Open, Find, Close
module: data
topic: btrieve-basics
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# Btrieve Open, Find, Close

## Purpose

This document describes the basic Btrieve-style functions for working with tables in CSCS_WPF:

- `OPENV`
- `FINDV`
- `CLOSEV`
- `FLERR`

These functions are used to open the table, retrieve the record to the memory buffer, close the table, and check for an error after the operation.

## Main concepts

- Working with a table starts with 'OPENV'.
- 'OPENV' returns the integer handle used in all subsequent Btrieve functions.
- 'FINDV' loads the record into the memory buffer or just checks if the record exists.
- 'FLERR' restores the status of the last operation.
- 'CLOSEV' finishes working with the table handle.

## OPENV

'OPENV' initializes the table for further Btrieve mode.

### Syntax

```internal-script
tableHandle = OPENV(tableName);
tableHandle = OPENV(tableName, dbName);
```

### Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `tableName` | string | yes | naziv tablice |
| `dbName` | string | well | short name of the database if the default base from the configuration is not used |

### Returns

Returns the integer handle for the open table [file:104].

### Example

```internal-script
DEFINE tableHndl type i;
tableHndl = OPENV("NKMKVEZM", "BD1");
```

## FINDV

'FINDV' retrieves a record from a table to a buffer or checks if a record exists, depending on the [file:104] arguments.

### Syntax

```internal-script
FINDV(handle, option, keyNameOrKeyNum);
FINDV(handle, option, keyNameOrKeyNum, matchExactValue);
FINDV(handle, option, keyNameOrKeyNum, matchExactValue, forStringSQL);
FINDV(handle, option, keyNameOrKeyNum, matchExactValue, forStringSQL, columnsToSelect);
FINDV(handle, option, keyNameOrKeyNum, matchExactValue, forStringSQL, columnsToSelect, "keyo");
```

### Supported options

| Option | Meaning |
|---|---|
| `f` | first record |
| `l` | last record |
| `n` | next record |
| `p` | previous record |
| `m` | Exact Match |
| `g` | exact or greater match |

### Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `handle` | int | yes | Handle Open Tables |
| `option` | string | yes | 'f', 'l', 'n', 'p', 'm', 'g' |
| `keyNameOrKeyNum` | string | yes | Key name or ordinal key number, e.g. 'usercode' or '2' |
| `matchExactValue` | string | no, but required for 'm' and 'g' | Value or set of values for match |
| `forStringSQL` | string | well | SQL-like filter that skips results |
| `columnsToSelect` | string | well | Load Column List |
| `keyo` | string | well | If it is '"keyo"', it only checks for the existence of no buffer filling |

### Notes

- For 'm' and 'g', 'matchExactValue' must be given [file:104].
- If 'keyo' is the default, the result is checked via 'FLERR', but the record is not populated in buffer [file:104].
- '0' as a key number means sorting by 'ID' column [file:104].

### Examples

```internal-script
FINDV(tableHndl, "f", "usercode");
FINDV(tableHndl, "n");
FINDV(tableHndl, "p");
FINDV(tableHndl, "m", "usercode", "ALBERT21");
FINDV(tableHndl, "g", "usercode", "ALBERT21");
```

## FLERR

'FLERR' returns the integer error code of the last Btrie function, globally or for a specific handle [file:104].

### Syntax

```internal-script
err = FLERR();
err = FLERR(tableHndl);
```

### Returns

- '0' means there is no error [file:104].
- The other numbers represent different errors [file:104].

### Examples

```internal-script
if (FLERR() != 0) {
    MessageBox(FLERR());
    exit;
}
```

```internal-script
if (FLERR(tableHndl) != 0) {
    MessageBox("Error on table handle");
    MessageBox(FLERR(tableHndl));
    exit;
}
```

## CLOSEV

'CLOSEV' closes the table and finishes working with its resources [file:104].

### Syntax

```internal-script
CLOSEV(tableHndl);
```

### Example

```internal-script
CLOSEV(tableHndl);
```

## Typical usage pattern

```internal-script
DEFINE tableHndl type i;

tableHndl = OPENV("NKMKVEZM", "BD1");

FINDV(tableHndl, "m", "usercode", "ALBERT21");

if (FLERR(tableHndl) != 0) {
    MessageBox("Record not found or Btrieve error");
    CLOSEV(tableHndl);
    exit;
}

/* Buffer is still used */

CLOSEV(tableHndl);
```

## Common mistakes

- Using 'FINDV' without first 'OPENV'.
- Using 'm' or 'g' without 'matchExactValue'.
- Relying on data from the buffer after an unsuccessful 'FINDV' operation.
- Forgotten 'CLOSEV'.
- Expecting 'FINDV(..., "keyo")' to fill the buffer, even though it is only used to verify the existence of [file:104].

## AI notes

- Do not invent additional 'FINDV' options.
- Use 'm' for accurate reach, 'f/n/p/l' for navigation.
- After 'FINDV' always consider 'FLERR'.
- The handle from 'OPENV' must be used consistently in all Btrieve calls.
