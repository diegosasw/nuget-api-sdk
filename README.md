# nuget-api-sdk
Api SDK autogenerated with Kiota

## Pre-Requirements
- .NET 8 SDK
- OpenApi documents available through http in json or yaml format. The generation is out of scope (see `sample-open-api` repository for more details)

## Local Development
This solution follows an approach in which the OpenAPI documents are available at a remote http server and can be used
to generate the client SDK.

It uses `Microsoft.OpenApi.Kiota` dotnet tool installed locally.

When cloning the repository, run the following to restore nuget dependencies
```
dotnet restore
```
and then run the following to restore dotnet tools

```
dotnet tool restore
```

NOTE: This solution includes a `dotnet-tools.json` stating the dotnet tools to restore because a manifest was created
with `dotnet new tool-manifest` and a local dotnet tool was added to the manifest with `dotnet tool install Microsoft.OpenApi.Kiota`.

Compile the solution
```
dotnet build
```

Generate SDK Client
```
dotnet kiota generate -l CSharp -o src/WebApiOne.Client/Sdk -c WebApiOneClient -n WebApiOne.Client.Sdk  -d https://die
gosasw.github.io/sample-open-api/web-api-one.json
```

The arguments mean the following:
- `-l` is the language to generate code for
- `-o` is the output folder to generate code in
- `-c` is the client class name
- `-n` is the namespace
- `-d` is the OpenApi document URL


Sometimes, the OpenApi documents will be updated. There is a `kiota-lock.json` which keeps a reference
to the remote OpenApi document.

Update SDK Client
```
dotnet kiota update -o src/WebApiOne.Client/Sdk
```