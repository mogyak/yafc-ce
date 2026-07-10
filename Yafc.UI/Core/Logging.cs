using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Yafc.UI;

/// <summary>
/// Contains default configuration for logging, to both a structured log file and to the console.
/// </summary>
public static class Logging {
    private static Action<LoggerConfiguration> configureLogger;
    private static bool canChangeConfiguration = true;
    private static readonly Lazy<ILogger> logger = new(CreateLogger);

    static Logging() => configureLogger = configure => configure
        .WriteTo.File(new JsonFormatter(renderMessage: true), GetDefaultLogFile(), rollingInterval: RollingInterval.Day, retainedFileTimeLimit: new TimeSpan(7, 0, 0, 0))
        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, standardErrorFromLevel: LogEventLevel.Error, outputTemplate: "{Message:lj}{NewLine}{Exception}")
        .Enrich.WithThreadId()
        .Enrich.With<StackTraceEnricher>()
        .MinimumLevel.Verbose();

    /// <summary>
    /// Call before calling <see cref="GetLogger"/> to replace the logging configuration with your preferred configuration.
    /// All default settings (Sinks and Enrichers) will be removed if you call this method.
    /// </summary>
    /// <param name="configureLogger">An <see cref="Action{T}"/> that configures the supplied <see cref="LoggerConfiguration"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="GetLogger"/> has been called.</exception>
    public static void SetLoggerConfiguration(Action<LoggerConfiguration> configureLogger) {
        ArgumentNullException.ThrowIfNull(configureLogger, nameof(configureLogger));

        if (!canChangeConfiguration) {
            throw new InvalidOperationException("Do not change configuration after getting the logger; it will have no effect.");
        }
        Logging.configureLogger = configureLogger;
    }

    /// <summary>
    /// Call to to get the logger, with either the default configuration or the configuration most recently passed to <see cref="SetLoggerConfiguration"/>.
    /// </summary>
    public static ILogger GetLogger<T>() => logger.Value.ForContext<T>();
    public static ILogger GetLogger(Type type) => logger.Value.ForContext(type);

    private static Logger CreateLogger() {
        canChangeConfiguration = false;
        LoggerConfiguration configuration = new LoggerConfiguration();
        configureLogger(configuration);

        return configuration.CreateLogger();
    }

    private static string GetDefaultLogFile() {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (!string.IsNullOrEmpty(home)) {
                return Path.Combine(home, "Library", "Logs", "YAFC-CE", "yafc.log");
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (!string.IsNullOrEmpty(localAppData)) {
                return Path.Combine(localAppData, "YAFC-CE", "Logs", "yafc.log");
            }
        }

        string xdgStateHome = Environment.GetEnvironmentVariable("XDG_STATE_HOME") ?? "";
        if (!string.IsNullOrEmpty(xdgStateHome)) {
            return Path.Combine(xdgStateHome, "yafc-ce", "yafc.log");
        }

        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrEmpty(userProfile)) {
            return Path.Combine(userProfile, ".local", "state", "yafc-ce", "yafc.log");
        }

        return "yafc.log";
    }

    /// <summary>
    /// Enrich log events with the source class unconditionally, and with a full stack trace for debug and verbose events.
    /// </summary>
    private sealed class StackTraceEnricher : ILogEventEnricher {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
            if (logEvent.Level is LogEventLevel.Debug or LogEventLevel.Verbose) {
                for (int i = 1; ; i++) {
                    StackTrace trace = new(i); // Null-forgive everything in StackTrace.

                    if (trace.GetFrame(0)!.GetMethod()!.DeclaringType!.Namespace!.StartsWith("Yafc")) {
                        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("StackTrace", trace));
                        break;
                    }

                    if (trace.FrameCount == 0) {
                        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("StackTrace", new StackTrace()));
                        break;
                    }
                }
            }
        }
    }
}
