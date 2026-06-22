# Linux and macOS installation

[English](LinuxOsxInstall.md) | [한국어](LinuxOsxInstall.ko.md)

## Release archives

Download the latest archive from the [YAFC releases page](https://github.com/Yafc-CE/yafc-ce/releases).

- Linux:
  - `Yafc-CE-Linux-self-contained-<version>.tar.gz` includes the .NET runtime and is the easiest option.
  - `Yafc-CE-Linux-<version>.tar.gz` requires the .NET 10 runtime to be installed separately.
- macOS Apple Silicon:
  - Use `Yafc-CE-OSX-arm64-<version>.tar.gz`.
- macOS Intel:
  - Use `Yafc-CE-OSX-intel-<version>.tar.gz`.

After extracting the archive, run the `Yafc` executable from the extracted folder:

```sh
chmod +x Yafc
./Yafc
```

## Linux

### Arch

There is an AUR package for YAFC-CE: [`factorio-yafc-ce-git`](https://aur.archlinux.org/packages/factorio-yafc-ce-git). Once installed, it can be run with `factorio-yafc`.

The current source tree targets .NET 10. If the AUR package metadata lags behind the repository, use the release archive or build from source with the .NET 10 SDK.

### Debian-based distributions

For the framework-dependent Linux archive, install the .NET 10 runtime from Microsoft:

- [Install .NET on Debian](https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian)
- [Download .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Install the native SDL/OpenGL dependencies:

```sh
sudo apt-get update
sudo apt-get install libsdl2-2.0-0 libsdl2-image-2.0-0 libsdl2-ttf-2.0-0 libgl1
```

For reference, YAFC needs these shared libraries at runtime:

- `SDL2-2.0.so.0`
- `SDL2_ttf-2.0.so.0`
- `SDL2_image-2.0.so.0`

If you use the self-contained Linux archive, .NET is bundled, but the SDL/OpenGL libraries are still system dependencies.

## macOS

Install the .NET 10 runtime if you use a release archive, or the .NET 10 SDK if you build from source:

- [Download .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- Or, with Homebrew:

```sh
brew install dotnet
dotnet --list-runtimes
```

Homebrew installs .NET under its own prefix. If `dotnet` works in your shell but YAFC cannot find the runtime, set `DOTNET_ROOT`:

```sh
export DOTNET_ROOT="$(brew --prefix dotnet)/libexec"
```

For Apple Silicon (`osx-arm64`), the repository includes the SDL2, SDL2_image, SDL2_ttf, and Lua native libraries used by the release build.

For Intel macOS (`osx-x64`), install SDL through Homebrew if the release archive you are using does not already include the SDL dylibs:

```sh
brew install sdl2 sdl2_image sdl2_ttf
```

Run YAFC from the extracted folder:

```sh
chmod +x Yafc
./Yafc
```

You can also launch it through Launch Services:

```sh
open -n ./Yafc
```

If macOS blocks the app because it was downloaded from the internet and is not notarized, either allow it in System Settings > Privacy & Security, or remove the quarantine attribute from the extracted YAFC folder:

```sh
xattr -dr com.apple.quarantine /path/to/Yafc-folder
```

## Building from source

Install the .NET 10 SDK, then publish the target you need from the repository root:

```sh
dotnet restore FactorioCalc.sln
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r linux-x64 --self-contained -c Release -o Build/Linux-self-contained
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
```

To build all release targets, run:

```sh
./build.sh
```

On macOS, the system `grep` may not support the `-P` option used by `build.sh`. If you hit `grep: invalid option -- P`, either run a single `dotnet publish` command directly, or install GNU grep and adjust the script to use `ggrep`:

```sh
brew install grep
```

You normally do not need to rebuild Lua manually. Only use the Lua build scripts when changing the bundled native Lua library; see [`lua/README.md`](/lua/README.md).

## Flathub

The [version available on Flathub](https://flathub.org/apps/details/com.github.petebuffon.yafc) is not the Community Edition. Its repository is https://github.com/petebuffon/yafc.

## General checklist

- .NET 10 runtime for framework-dependent release archives, or .NET 10 SDK for source builds.
- SDL2, SDL2_image, SDL2_ttf, and OpenGL on Linux.
- On macOS Intel, SDL2 libraries may need to be installed through Homebrew.
- Run the `Yafc` executable from the extracted or published folder.
