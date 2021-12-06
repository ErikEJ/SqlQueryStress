# sqlstresscmd

Cross platform SQL query stress simulator command line tool (early preview) 

## Getting started guide

1. Download and extract the .zip file with the tool - future plan is to make a dotnet global tool. See download link below.

2. Create a json config file similar to [this one](https://github.com/ErikEJ/SqlQueryStress/blob/master/src/SqlQueryStressCLI/sample.json)  

3. Run the tool, create the load, and view the summary - future plan is to show progress while running.

```bash
sqlstresscmd -s sample.json -t 100
```
To get help, run

```bash
sqlstresscmd help
```

## Download

You can download a .zip file with the [latest daily build from GitHub releases](https://github.com/ErikEJ/SqlQueryStress/releases)

Notice that you must have the [.NET Core 3.1 runtime](https://dotnet.microsoft.com/download) installed.

[Release notes](https://github.com/ErikEJ/SqlQueryStress/wiki/Release-notes)

## Contributing

Any and all contributions are welcome! Please see the full [contributing guide](CONTRIBUTING.md) for more details.  
