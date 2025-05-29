# Financial KPI Dashboard with cscs_wpf

## Overview
This dashboard reads financial KPIs from General Ledger table `NKGKPROM` using Croatian Chart of Accounts logic.

## Features
- Multi-year comparison
- Period filter (from month – to month)
- Trend indicators ? ? ? (color-coded)
- Tooltips on KPI names
- Responsive layout
- Drill-down modals showing GL transactions
- Charts (Revenue vs COGS)

## Requirements
- [cscs_wpf](https://github.com/vassilych/cscs_wpf) installed
- SQL Server database with tables `NKGKPROM`, `NKGKARTI`
- Connection string in `App.config`

## How to Use
1. Copy all scripts into your `Scripts/` folder.
2. Run `main_dashboard.cscs`.
3. Select year and period to calculate KPIs.
4. Click on any KPI to open transaction details.

## KPI Definitions
See `KPI_Accounts.txt` for account codes per KPI.

## Functions Used
- `GetSumFromGL()` – fetches values from GL
- `GetTrendIcon()` – returns ?, ?, ? with color
- `DrawKPICharts()` – uses LiveCharts
- `ShowKPIDetails()` – opens modal with GL rows