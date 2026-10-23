using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace TestController
{
    [CollectionDefinition("Logging", DisableParallelization = true)]
    public class LoggingCollection { }

    [Collection("Logging")]
    public class LoggingTests
    {
        [Fact]
        public void LoggingBeforeExplicitInitializationDoesNotFail()
        {
            WithIsolatedLog((probe, directory) =>
                Assert.Null(Record.Exception(() => probe.Write())));
        }

        [Fact]
        public void DefaultConsoleLoggerCanBeInitializedWithItsRuntimeDependencies()
        {
            WithIsolatedLog((probe, directory) =>
            {
                probe.InitializeConsole();
                probe.Write();
                Assert.Contains("diagnostic", File.ReadAllText(
                    Directory.GetFiles(Path.Combine(directory, "Logs")).Single()));
            });
        }

        [Fact]
        public void AnUnavailableLogFileDoesNotInterruptTheCaller()
        {
            WithIsolatedLog((probe, directory) =>
            {
                probe.Initialize();
                var logFile = Directory.GetFiles(Path.Combine(directory, "Logs")).Single();
                using (var unavailable = new FileStream(logFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    Assert.Null(Record.Exception(() => probe.Write()));
            });
        }

        [Fact]
        public void ConcurrentDiagnosticsAreWrittenWithoutLostMessages()
        {
            WithIsolatedLog((probe, directory) =>
            {
                probe.Initialize();
                probe.WriteConcurrently();
                var logFile = Directory.GetFiles(Path.Combine(directory, "Logs")).Single();
                Assert.Equal(30, File.ReadAllLines(logFile).Count(line => line.Contains("parallel diagnostic")));
            });
        }

        [Fact]
        public void ExceptionDiagnosticsDoNotIncludeSensitiveExceptionMessages()
        {
            WithIsolatedLog((probe, directory) =>
            {
                probe.Initialize();
                probe.WriteException();
                var logFile = Directory.GetFiles(Path.Combine(directory, "Logs")).Single();
                var content = File.ReadAllText(logFile);
                Assert.Contains("InvalidOperationException", content);
                Assert.DoesNotContain("private-password-value", content);
            });
        }

        private static void WithIsolatedLog(Action<LoggingProbe, string> check)
        {
            var directory = Path.Combine(Path.GetTempPath(), "warehouse-logging-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            var originalDirectory = Directory.GetCurrentDirectory();
            var assemblyPath = new Uri(typeof(LoggingTests).Assembly.CodeBase).LocalPath;
            var domain = AppDomain.CreateDomain("Logging-" + Guid.NewGuid().ToString("N"), null,
                new AppDomainSetup
                {
                    ApplicationBase = Path.GetDirectoryName(assemblyPath),
                    ConfigurationFile = assemblyPath + ".config"
                });
            try
            {
                Directory.SetCurrentDirectory(directory);
                var probe = (LoggingProbe)domain.CreateInstanceFromAndUnwrap(
                    assemblyPath, typeof(LoggingProbe).FullName);
                check(probe, directory);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                AppDomain.Unload(domain);
                Directory.Delete(directory, true);
            }
        }
    }

    public class LoggingProbe : MarshalByRefObject
    {
        public void Initialize() { new Log(NullLogger<Log>.Instance); }
        public void InitializeConsole() { new Log(); }
        public void Write() { Log.LogMessage(LogLevel.Information, "Tests", "diagnostic"); }
        public void WriteConcurrently()
        {
            Parallel.For(0, 30, index => Log.LogMessage(LogLevel.Information, "Tests", "parallel diagnostic " + index));
        }
        public void WriteException()
        {
            Log.LogException(new InvalidOperationException("private-password-value"), "Tests");
        }
        public override object InitializeLifetimeService() { return null; }
    }
}
