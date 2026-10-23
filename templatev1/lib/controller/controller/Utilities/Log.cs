using System;
using System.IO;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LMCIS.controller.Utilities
{
    public class Log
    {
        private static readonly object Sync = new object();
        private static volatile ILogger<Log> logger = NullLogger<Log>.Instance;
        // Reuse the console provider for the lifetime of the application.
        private static readonly Lazy<ILoggerFactory> ConsoleLoggerFactory = new Lazy<ILoggerFactory>(() =>
            LoggerFactory.Create(builder => builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                .AddConsole()));
        private static readonly string CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        private static readonly string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string LogFileName = Path.Combine(LogDirectory, $"Log-{CurrentTime}.txt");

        public Log(ILogger<Log> logger = null)
        {
            lock (Sync)
            {
                try
                {
                    Log.logger = logger ?? ConsoleLoggerFactory.Value.CreateLogger<Log>();
                }
                catch (Exception)
                {
                    Log.logger = NullLogger<Log>.Instance;
                }
                WriteToLogFile(null);
            }
        }

        public static void LogMessage(LogLevel logLevel, string programName, string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - [{logLevel}] - [{programName}] - {message}";
            Write(logLevel, logMessage);
        }

        public static void LogException(Exception ex, string programName)
        {
            // Exception messages may contain credentials, SQL values, or user input.
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - [Exception] - [{programName}] - {ex?.GetType().FullName ?? "Unknown exception"}";
            Write(LogLevel.Error, logMessage);
        }

        private static void Write(LogLevel level, string message)
        {
            try
            {
                logger.Log(level, "{LogMessage}", message);
            }
            catch (Exception)
            {
                // Diagnostics must not interrupt an application operation.
            }
            WriteToLogFile(message);
        }

        private static void WriteToLogFile(string message)
        {
            lock (Sync)
            {
                try
                {
                    Directory.CreateDirectory(LogDirectory);
                    File.AppendAllText(LogFileName, message == null ? string.Empty : message + Environment.NewLine);
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
                catch (SecurityException) { }
            }
        }
    }
}
