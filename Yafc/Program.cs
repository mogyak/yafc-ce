using System;
using System.IO;
using System.Runtime.InteropServices;
using Serilog;
using Yafc.I18n;
using Yafc.Model;
using Yafc.Parser;
using Yafc.UI;

namespace Yafc;

public static class Program {
    private static readonly ILogger logger = Logging.GetLogger(typeof(Program));
    internal static bool hasOverriddenFont { get; private set; }

    private static void Main(string[] args) {
        SetWorkingDirectoryForAppBundle(args);

        YafcLib.RegisterDefaultAnalysis();

        // Wire up the UI-aware undo batch scheduler so that undo batches are committed on gesture-finish
        // (mouse-up) rather than immediately. This must be set before any Project is loaded or created,
        // including any that Ui.Start() might trigger during initialization.
        UndoSystem.DefaultScheduler = new GestureFinishUndoBatchScheduler();

        try {
            Ui.Start();
        }
        catch (Exception ex) {
            logger.Fatal(ex, "Failed to initialize the UI");
            throw;
        }

        // This must happen before Preferences.Instance, where we load the prefs file and the requested translation.
        FactorioDataSource.LoadYafcLocale("en");

        string? overrideFont = Preferences.Instance.overrideFont;
        FontFile? overriddenFontFile = null;

        try {
            if (!string.IsNullOrEmpty(overrideFont) && File.Exists(overrideFont)) {
                overriddenFontFile = new FontFile(overrideFont);
            }
        }
        catch (Exception ex) {
            Console.Error.WriteException(ex);
        }

        string baseFileName = "Roboto";
        if (WelcomeScreen.languageMapping.TryGetValue(Preferences.Instance.language, out LanguageInfo? language)) {
            if (Font.FilesExist(language.BaseFontName)) {
                baseFileName = language.BaseFontName;
            }
        }

        hasOverriddenFont = overriddenFontFile != null;
        Font.header = new Font(overriddenFontFile ?? new FontFile($"Data/{baseFileName}-Light.ttf"), 2f);
        var regular = overriddenFontFile ?? new FontFile($"Data/{baseFileName}-Regular.ttf");
        Font.subheader = new Font(regular, 1.5f);
        Font.productionTableHeader = new Font(regular, 1.23f);
        Font.text = new Font(regular, 1f);

        ProjectDefinition? cliProject = CommandLineParser.ParseArgs(args);

        if (CommandLineParser.errorOccured || CommandLineParser.helpRequested) {
            Console.WriteLine(LSs.YafcWithVersion.L(YafcLib.version.ToString(3)));
            Console.WriteLine();

            if (CommandLineParser.errorOccured) {
                Console.WriteLine(LSs.CommandLineError.L(CommandLineParser.lastError));
                Console.WriteLine();
                Environment.ExitCode = 1;
            }

            CommandLineParser.PrintHelp();
        }
        else {
            _ = new WelcomeScreen(cliProject);
            Ui.MainLoop();
        }
    }

    private static void SetWorkingDirectoryForAppBundle(string[] args) {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            return;
        }

        string baseDirectory = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
        string appBundleMacOsDirectory = $"{Path.DirectorySeparatorChar}Contents{Path.DirectorySeparatorChar}MacOS";
        if (baseDirectory.EndsWith(appBundleMacOsDirectory, StringComparison.Ordinal)
            && Directory.Exists(Path.Combine(baseDirectory, "Data"))) {
            string launchDirectory = Environment.CurrentDirectory;
            for (int i = 0; i < args.Length; i++) {
                if (!args[i].StartsWith('-') && !Path.IsPathRooted(args[i])) {
                    args[i] = Path.GetFullPath(args[i], launchDirectory);
                }
            }

            Directory.SetCurrentDirectory(baseDirectory);
        }
    }
}
