# Command Line Interface

[English](CLI.md) | [한국어](CLI.ko.md)

YAFC can be invoked from a command line:

```sh
./Yafc
./Yafc --help
./Yafc path/to/project.yafc
./Yafc path/to/Factorio/data --mods-path path/to/mods --project-file path/to/project.yafc
```

For the current built-in help text, run:

```sh
./Yafc --help
```

YAFC can be started without arguments. This opens the welcome screen.

A single non-option argument is treated as a project file, not as a Factorio data directory:

```sh
./Yafc path/to/project.yafc
```

If that project has been opened before, YAFC uses its saved Factorio data and mods paths. If it has not been opened before, YAFC uses the start settings from the most recently opened project.

When starting from Factorio data directly, the data directory must be the first argument and at least one option must follow it. The following options may be supplied:

- `<data-path>`: Factorio `data` directory.
- `--mods-path <path>`: Factorio mods directory.
- `--project-file <path>`: YAFC project file to load or create.
- `--net-production`: only suggest recipes with net production or net consumption for the selected item or fluid.
- `--help`: print the built-in help and exit.

Provided directories must exist. For `--project-file`, the containing directory must exist; the project file itself may be new.
