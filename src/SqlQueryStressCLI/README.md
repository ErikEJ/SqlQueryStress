# sqlstresscmd

Cross platform SQL query stress simulator command line tool (early preview)

## Getting started guide

1. Install/update the tool

```bash
dotnet tool install -g sqlstresscmd
```

```bash
dotnet tool update -g sqlstresscmd
```

You can also download directly from [NuGet](https://www.nuget.org/packages/sqlstresscmd) and unzip the package locally.

2. Create a json config file similar to [this one](https://github.com/ErikEJ/SqlQueryStress/blob/master/src/SqlQueryStressCLI/sample.json)

You can run this command to get a sample:

```bash
sqlstresscmd -x
```

3. Run the tool, create the load, and view the summary

```bash
sqlstresscmd -s sample.json -t 1
```

![Sample screenshot](https://raw.githubusercontent.com/ErikEJ/SqlQueryStress/refs/heads/master/src/SqlQueryStressCLI/sample.png)

To get help, run

```bash
sqlstresscmd help
```

## Contributing

Any and all contributions are welcome! Please see the full [contributing guide](CONTRIBUTING.md) for more details.  
