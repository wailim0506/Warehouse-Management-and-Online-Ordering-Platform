using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using LMCIS.controller.Utilities;
using Xunit;

namespace TestController
{
    public class ConnectionPerformanceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void StartupConnectionAttemptsCandidatesInOrderAndRetainsFirstSuccess(int successfulIndex)
        {
            var candidates = new[] { "candidate-0", "candidate-1", "candidate-2", "candidate-3" };
            var attempts = new List<string>();
            var created = new List<FakeConnection>();

            var connection = Database.OpenFirstAvailableConnection(candidates,
                candidate =>
                {
                    attempts.Add(candidate);
                    var fake = new FakeConnection(candidate == candidates[successfulIndex]);
                    created.Add(fake);
                    return fake;
                });

            Assert.Equal(candidates.Take(successfulIndex + 1), attempts);
            Assert.Same(created[successfulIndex], connection);
            Assert.Equal(ConnectionState.Open, created[successfulIndex].State);
            Assert.False(created[successfulIndex].WasDisposed);
            Assert.All(created.Take(successfulIndex), failed => Assert.True(failed.WasDisposed));
            Assert.Equal(successfulIndex + 1, created.Count);
        }

        [Fact]
        public void StartupConnectionFailureDisposesEveryCandidateAndAttemptsEachOnlyOnce()
        {
            var candidates = new[] { "unavailable-1", "unavailable-2", "unavailable-3" };
            var attempts = new List<string>();
            var created = new List<FakeConnection>();

            Assert.Throws<Exception>(() => Database.OpenFirstAvailableConnection(candidates,
                candidate =>
                {
                    attempts.Add(candidate);
                    var fake = new FakeConnection(false);
                    created.Add(fake);
                    return fake;
                }));

            Assert.Equal(candidates, attempts);
            Assert.All(created, failed => Assert.True(failed.WasDisposed));
        }

        [Fact]
        public void StartupConnectionWithNoCandidatesFailsWithoutCreatingAConnection()
        {
            var factoryCalls = 0;
            Assert.Throws<Exception>(() => Database.OpenFirstAvailableConnection(
                new string[0], candidate =>
                {
                    factoryCalls++;
                    return new FakeConnection(true);
                }));
            Assert.Equal(0, factoryCalls);
        }

        private sealed class FakeConnection : DbConnection
        {
            private readonly bool _canOpen;
            private ConnectionState _state = ConnectionState.Closed;

            public FakeConnection(bool canOpen) { _canOpen = canOpen; }
            public bool WasDisposed { get; private set; }
            public override string ConnectionString { get; set; }
            public override string Database => "fake";
            public override string DataSource => "fake";
            public override string ServerVersion => "1";
            public override ConnectionState State => _state;
            public override int ConnectionTimeout => 0;
            public override void Open()
            {
                if (!_canOpen) throw new InvalidOperationException("unavailable");
                _state = ConnectionState.Open;
            }
            public override void Close() { _state = ConnectionState.Closed; }
            public override void ChangeDatabase(string databaseName) { }
            protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => throw new NotSupportedException();
            protected override DbCommand CreateDbCommand() => throw new NotSupportedException();
            protected override void Dispose(bool disposing) { WasDisposed = true; base.Dispose(disposing); }
        }
    }
}
