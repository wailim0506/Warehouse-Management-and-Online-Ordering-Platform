using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace LMCIS.controller.Utilities
{
    public class Database : IDisposable, IDatabase
    {
        private readonly MySqlConnection _connection;
        private MySqlTransaction _transaction;

        public Database(string connectionString = null)
        {
            _connection = new MySqlConnection(connectionString ?? GetConnectionString());
            try
            {
                _connection.Open();
                new Log();
            }
            catch
            {
                _connection.Dispose();
                throw;
            }
        }

        private string GetConnectionString()
        {
            var connectionStrings = new List<string>();
            connectionStrings.Add(Configuration.DataBaseConnectionString);

            return TestConnection(connectionStrings) ?? throw new Exception("No valid connection string found.");
        }

        private string TestConnection(List<string> connectionStrings)
        {
            foreach (var connectionString in connectionStrings)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return connectionString;
                    }
                    catch
                    {
                        // Ignore the exception and try the next connection string
                    }
                }
            }

            // If none of the connection strings work, throw an exception or return null
            throw new Exception("No valid connection string found.");
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        public void ExecuteInTransaction(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (_transaction != null)
                throw new InvalidOperationException("Nested database transactions are not supported.");

            using (var transaction = _connection.BeginTransaction())
            {
                _transaction = transaction;
                try
                {
                    action();
                    transaction.Commit();
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackError)
                    {
                        // Preserve the operation's original error if the connection was lost.
                        Log.LogException(rollbackError, "Database rollback");
                    }
                    throw;
                }
                finally
                {
                    _transaction = null;
                }
            }
        }

        public object ExecuteScalarCommand(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            return ExecuteCommand(sqlQuery, queryParameters, command => command.ExecuteScalar());
        }

        // Execute a non-query command and return the number of affected rows
        public virtual void ExecuteNonQueryCommand(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            Log.LogMessage(LogLevel.Debug, "Database",
                $"ExecuteNonQueryCommand : {sqlQuery + queryParameters}");
            ExecuteCommand(sqlQuery, queryParameters, command => command.ExecuteNonQuery());
        }

        public MySqlDataReader ExecuteReaderCommand(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            Log.LogMessage(LogLevel.Debug, "Database",
                $"ExecuteReaderCommand : {sqlQuery + queryParameters}");
            return (MySqlDataReader)ExecuteCommand(sqlQuery, queryParameters, command => command.ExecuteReader());
        }

        public object ExecuteCommand(string sqlQuery, Dictionary<string, object> queryParameters,
            Func<MySqlCommand, object> execute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            using (var command = CreateCommand(sqlQuery, queryParameters))
            {
                try
                {
                    return execute(command);
                }
                catch (Exception ex)
                {
                    Log.LogException(ex, "Database");
                    throw new InvalidOperationException("Database command failed.", ex);
                }
            }
        }

        public MySqlCommand CreateCommand(string query, Dictionary<string, object> parameters)
        {
            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.Transaction = _transaction;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }
            }

            Log.LogMessage(LogLevel.Debug, "Database", $"CreateCommand : {command.CommandText}");
            return command;
        }

        public void ExecuteNonQuery(MySqlCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Connection != _connection)
                throw new ArgumentException("The command must use this database connection.", nameof(command));
            command.Transaction = _transaction;
            command.ExecuteNonQuery();
        }

        public virtual DataTable ExecuteDataTable(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            return (DataTable)ExecuteCommand(sqlQuery, queryParameters, command =>
            {
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            });
        }

        public DataTable ExecuteDataTable(string sqlQuery)
        {
            return ExecuteDataTable(sqlQuery, null);
        }

        // Execute a scalar command and return the result
        public object ExecuteScalar(string sqlCmd)
        {
            using (var command = CreateCommand(sqlCmd, null))
            {
                Log.LogMessage(LogLevel.Debug, "Database", $"ExecuteScalar : {sqlCmd}");
                return command.ExecuteScalar();
            }
        }

        // Execute a scalar command and return the result as a string
        public string ExecuteScalarCommand(string sqlQuery)
        {
            using (var command = CreateCommand(sqlQuery, null))
            {
                Log.LogMessage(LogLevel.Debug, "Database", $"ExecuteScalarCommand : {sqlQuery}");
                return command.ExecuteScalar().ToString();
            }
        }
    }
}
