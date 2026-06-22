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
    private const string MacKoreanSystemFont = "/System/Library/Fonts/AppleSDGothicNeo.ttc";
    private const int MacKoreanSystemFontRegularFace = 0;
    private const int MacKoreanSystemFontLightFace = 8;
    internal static bool hasOverriddenFont { get; private set; }

    private static void Main(string[] args) {
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

        Preferences preferences = Preferences.Instance;
        Ui.SetInterfaceScale(preferences.interfaceScale);

        string? overrideFont = preferences.overrideFont;
        FontFile? overriddenFontFile = null;

        try {
            if (!string.IsNullOrEmpty(overrideFont) && File.Exists(overrideFont)) {
                overriddenFontFile = new FontFile(overrideFont);
            }
        }
        catch (Exception ex) {
            Console.Error.WriteException(ex);
        }

        FontFile? headerFontFile = overriddenFontFile;
        FontFile? regularFontFile = overriddenFontFile;
        string baseFileName = "Roboto";
        if (WelcomeScreen.languageMapping.TryGetValue(preferences.language, out LanguageInfo? language)) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && preferences.language == "ko" && File.Exists(MacKoreanSystemFont)) {
                headerFontFile ??= new FontFile(MacKoreanSystemFont, MacKoreanSystemFontLightFace);
                regularFontFile ??= new FontFile(MacKoreanSystemFont, MacKoreanSystemFontRegularFace);
            }
            else if (Font.FilesExist(language.BaseFontName)) {
                baseFileName = language.BaseFontName;
            }
        }

        hasOverriddenFont = overriddenFontFile != null;
        Font.header = new Font(headerFontFile ?? new FontFile($"Data/{baseFileName}-Light.ttf"), 2f);
        var regular = regularFontFile ?? new FontFile($"Data/{baseFileName}-Regular.ttf");
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
}
