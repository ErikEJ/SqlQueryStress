# SqlQueryStress

![screenshot](https://raw.githubusercontent.com/ErikEJ/SqlQueryStress/refs/heads/master/images/figure1.png)

SQL query stress simulator [created by Adam Machanic](https://dataeducation.com/sqlquerystress-the-source-code/).

## Installation

The tool runs on any Windows machine with the .NET 8.0 runtime installed.

Get the latest version from [GitHub releases](https://github.com/ErikEJ/SqlQueryStress/releases)

[Release notes](https://github.com/ErikEJ/SqlQueryStress/wiki/Release-notes)

## Getting started guide

[Introduction to SQL Query Stress](https://github.com/ErikEJ/SqlQueryStress/wiki)

## sqlstresscmd

A cross platform command line tool using the same load engine is also available, [see dedicated readme](https://github.com/ErikEJ/SqlQueryStress/blob/master/src/SqlQueryStressCLI/README.md)

## Connection Settings

SQL Query Stress automatically applies SQL Server connection settings similar to SQL Server Management Studio (SSMS) to ensure consistent query execution behavior. These settings are read from the `querysettings.sql` file located in the application directory.

### Default Settings

By default, the following SSMS-compatible settings are applied to every connection:

- `SET QUOTED_IDENTIFIER ON;`
- `SET ANSI_NULL_DFLT_ON ON;`
- `SET ANSI_PADDING ON;`
- `SET ANSI_WARNINGS ON;`
- `SET ANSI_NULLS ON;`
- `SET ARITHABORT ON;`
- `SET CONCAT_NULL_YIELDS_NULL ON;`

These settings match the default SSMS configuration and ensure that query execution behavior is consistent between SSMS and SQL Query Stress.

### Customizing Connection Settings

You can customize the connection settings by editing the `querysettings.sql` file in the application directory. Any valid T-SQL `SET` commands can be added to this file, and they will be automatically executed when each connection is opened.

Example customizations:
```sql
SET NOCOUNT ON;
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
```

**Note:** Settings are applied automatically when connections are opened, so there's no need to modify your test queries.

## Contributing

Any and all contributions are welcome! Please see the full [contributing guide](CONTRIBUTING.md) for more details.  
