<h1 align="center">YAFC: Community Edition</h1>
<p align="center"><IMG style="width:50px; height:auto;" src="Yafc/image.ico" alt="yafc_icon.png"></p>
<p align="center"><a href="README.md">English</a> | <a href="README.ko.md">한국어</a></p>

### Why new repo?
The [original](https://github.com/ShadowTheAge/yafc) YAFC repository was inactive for a long time. Bugfixes piled up, but there was no one to merge them.  
This repository addresses that by providing continuous support to the development.

### Have you talked with the author?
Yes, we have their approval.
<details>
<summary>Expand to see the screenshot</summary>
<IMG src="/Docs/Media/yafc_author_approval.png"  alt="yafc_author_approval.png"/>
</details>

## What is YAFC?
Yet Another Factorio Calculator or YAFC is a planner and analyzer. The main goal of YAFC is to help with heavily modded Factorio games.

<details>
<summary>Expand to see what YAFC can do</summary>
<IMG src="/Docs/Media/Main.gif"  alt="Main.gif"/>
</details>

YAFC is more than just a calculator. It uses multiple algorithms to understand what is going on in your modpack to the point of calculating the whole late-game base. It knows what items are more important and what recipes are more efficient.

It was created as an answer to recursive [Pyanodon](https://mods.factorio.com/user/pyanodon) recipes that tools like Helmod could not handle. YAFC uses Google's [OrTools](https://developers.google.com/optimization) as a model solver to handle them extremely well.

YAFC also has its own Never Enough Items, which is FNEI on steroids. In addition to showing the recipes, it shows which ones you want to use and how much.

## Getting started

YAFC is a desktop app. The Windows build is the most tested, but macOS and Linux are supported too. See [Linux and macOS installation instructions](/Docs/LinuxOsxInstall.md) for platform-specific dependencies and troubleshooting.

1. Navigate to the [YAFC releases](https://github.com/Yafc-CE/yafc-ce/releases).
1. Download the archive for your OS and CPU architecture:
    - Windows: `Yafc-CE-Windows-<version>.zip`, or the self-contained Windows archive if you do not want to install .NET separately.
    - macOS Apple Silicon: `Yafc-CE-OSX-arm64-<version>.tar.gz`.
    - macOS Intel: `Yafc-CE-OSX-intel-<version>.tar.gz`.
    - Linux: `Yafc-CE-Linux-self-contained-<version>.tar.gz` for the simplest setup, or `Yafc-CE-Linux-<version>.tar.gz` if you already have the .NET 10 runtime installed.
1. Install the required runtime dependencies:
    - Windows: install the latest [VC Redist](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170), which is needed for Google OrTools.
    - macOS and Linux: follow [Linux and macOS installation instructions](/Docs/LinuxOsxInstall.md).
1. Extract the archive to your preferred location.
1. Run either `./Yafc` or `./Yafc.exe` depending on your OS.
1. Once YAFC is opened, locate your Factorio data and mod folders. Refer to the [Factorio application directory wiki](https://wiki.factorio.com/Application_directory#Locations) for OS-specific paths.

We also have the following materials to improve your Yafc experience:
* [Gifs](/Docs/Gifs.md) for the examples of different use cases, but beware that Gifs are traffic-heavy.  
* [Tips and Tricks](/Docs/TipsAndTricks.md) and the [in-built tips](https://github.com/Yafc-CE/yafc-ce/blob/master/Yafc/Data/Tips.txt) for useful info.
* [Shortcuts](/Docs/Shortcuts.md) for quality of life.

If you want to build YAFC from source, install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). You can run `./build.sh` from the repository root to build all release targets, or run one `dotnet publish` command for your platform:

```sh
dotnet publish Yafc/Yafc.csproj -r linux-x64 -c Release -o Build/Linux
dotnet publish Yafc/Yafc.csproj -r osx-arm64 -c Release -o Build/OSX-arm64
dotnet publish Yafc/Yafc.csproj -r osx-x64 -c Release -o Build/OSX
dotnet publish Yafc/Yafc.csproj -r win-x64 -c Release -o Build/Windows
```

See [How to build YAFC from sources](/Docs/HowToBuild.md) for more detail.

## Project features
- Works with any combination of mods for Factorio 2.0+. The most recent version that supports Factorio 1.1 is 0.9.1.
- Multiple pages, the Undo button (Ctrl+Z).
- Dependency Explorer that allows to see which objects are needed for what.
- Never Enough Items that helps to find out how to produce any item, and which option YAFC considers optimal.
- Main calculator sheet:
    - Links: YAFC will try to balance production/consumption only for linked goods. Unlinked goods are calculated but not balanced. It is a core difference from Helmod, which attempts to balance everything and breaks on deeply recursive recipes.
    - Nested tables: You can attach a nested table to any recipe. When a table is collapsed, you will see a summary for all recipes in it. Nested tables have their own set of links. For example, if a nested table has copper cables that are linked inside, then these cables will be calculated only inside the nested table.
    - Auto modules: You can add modules to recipes by using a single slider. It will add modules based on your milestones and will prioritize modules in buildings that benefit most. <details><summary>Expand to see it in action</summary><IMG src="/Docs/Media/AutoModules.gif"  alt="AutoModules.gif"/></details>
    - Fluid temperatures, without mixing.
    - Fuel and electricity. You can even add exactly enough energy for your sheet. However, inserters are not included.
- Multiple analyses:
    - Accessibility analysis shows inaccessible objects. Mods often hide objects, and Factorio has a bunch of hidden ones too. However, it is impossible to find objects that are spawned by mods or map scripts. This analysis may fail for modpacks like Seablock, but you can mark some objects as accessible manually.
    - Milestone analysis: You can add anything as a milestone. YAFC will display that milestone icon on every object that is locked behind it, directly or indirectly. Science packs are natural milestones, and so they are added by default.
    - Automation analysis finds objects that cannot be fully automated. For example, wood in a vanilla.
    - Cost analysis assigns a cost to each object. The cost is a sum of logistic actions you need to perform to get that object when using optimal recipes. It helps to compare which recipe is better.
    - Flow analysis calculates a base that produces enough science packs for all non-infinite research.
- Load projects from the [command line](/Docs/CLI.md).

## Possible incompatibilities

- For Seablock, please check [this](https://github.com/ShadowTheAge/yafc/issues/31) issue that has a list of things to enable at first.
- For other mods with scripted progression, YAFC might also think that items are inaccessible. There is no silver bullet against that, but you can open Dependency Explorer and manually mark a bunch of items or technologies as accessible.

For mod authors: You can add a mod-specific fix to Yafc. Here is a [guide](/Yafc/Data/Mod-fixes/README.md) about it. You can also detect YAFC by checking the `data.data_crawler` variable during the data stage. It will be equal to `yafc a.b.c.d` where `a.b.c.d` is yafc version. For instance, `yafc 2.5.6.0`.

YAFC loads mods in environment that is not completely compatible with Factorio. If you notice any bugs, please report them in the [issues](https://github.com/Yafc-CE/yafc-ce/issues).


## Contributing

Do you want to discuss things about YAFC? Feel free to join our [channel](https://discord.gg/b5VergGq75) on the Pyanodons Discord!  
Do you want to make a Pull Request to YAFC? Great! Please check out the [Contributor's Guide](Docs/CONTRIBUTING.md) to make sure the review process of your PR is smooth.

## License
- [GNU GPL 3.0](/LICENSE)
- Copyright 2020 © ShadowTheAge
- This readme contains gifs featuring Factorio icons. All Factorio icons are copyright of Wube Software.
- Powered by free software: .NET core, SDL2, Google Or-Tools, Lua and others (see [full list](/licenses.txt)).
