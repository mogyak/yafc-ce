# How to build Yafc from sources

[English](HowToBuild.md) | [한국어](HowToBuild.ko.md)

- Download the sources,
- Install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0),
- Run `./build.sh` from the repository root to build all release targets,
- Your built copies are located under `Build`.

To build only one target, run the matching `dotnet publish` command from the repository root:

```sh
dotnet publish Yafc/Yafc.csproj -r win-x64 -c Release -o Build/Windows
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
```

If you want to run YAFC after that, please refer to the installation instructions in the README or [Linux and macOS installation guide](LinuxOsxInstall.md).
