This document serves as an "entry point" for AI and developers. It should be briefly explained what CSCS_WPF is, how it is conceived and what are the main functional units of the language.WpfCSCS-UPUTE-3.docx
text
---
title: CSCS_WPF Overview
module: language
topic: overview
applies_to: CSCS_WPF
version: 1
source: internal manual
---

# CSCS_WPF Overview

## Purpose

CSCS_WPF is an in-house scripting language and framework for building applications, based on the concepts of C, JavaScript and Python, with integration with WPF/XAML for GUI and DevExpress Reporting for prints and reports.

Language is event-driven. In a typical program, the entry point is not the classic 'main' function, but the start window and its events.

## Main characteristics

- The syntax is a mixture of C, JavaScript, and Python approaches.
- All commands end with a semicolon ';'.
- Indenting lines is not part of the syntax.
- Blocks are defined by curly brackets '{ ... }`.
- The language supports implicit and explicit variable definitions.
- The recommended mode is to explicitly define variables via 'DEFINE'.
- It is integrated with the WPF/XAML interface.
- Supports basic data access, including Btrieve-style functions and SQL functions.
- Supports generating and displaying reports through DevExpress Reporting.

## Entry point

A program usually starts in one of two ways:

1. By calling 'CreateWindow(fullXamlPath)' --- > Preferred option!!!
2. 'WINFORM myWindow' directive

If there are more than one 'WINFORM' directives, the start window is listed first. Other XAML windows can also be used to check XAML code.

## Code allowed outside events

CSCS_WPF is predominantly an event-driven language. Outside of event functions, only certain elements are allowed.

Rule of thumb:
- 'WINFORM' and 'MAIMENU' are special directives that may stand outside the event.
- Functions and 'DEFINE' declarations are processed in the first pass of the script.
- Other GUI and business steps should be within the functions or event handler.

## Execution model

The script is executed in two passes:

1. In the first pass, 'DEFINE' declarations and functions are processed.
2. In the second pass, the script is executed.

This is important when generating code, as AI must not assume a classic single-pass execution model.

## Main documentation areas

The documentation is divided into these sections:

- language syntax and execution rules,
- GUI to WPF/XAML integration,
- widgets and window events,
- custom WPF controls,
- access to data and Btrieve's functions,
The SQL function.
- reporting and DevExpress '.repx' templates,
- XML and auxiliary functions,
- Forms and verified examples.

## Recommended AI usage

When AI generates CSCS_WPF code, these principles should be adhered to:

- use only documented syntax,
- prefer 'DEFINE' over implicit declarations,
- po tovati event-driven model,
- do not invent event names, controls and report fields,
- Use existing forms for windows, database and reporting.

## AI notes

- Do not assume that CSCS_WPF works as pure C#, JavaScript, or Python.
- Do not assume the existence of the 'main()' function.
- Do not place arbitrary execution code outside of permitted directives and definitions.
- For GUI scenarios, always check the window lifecycle and the name of the event functions.
