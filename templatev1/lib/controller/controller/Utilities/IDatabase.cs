using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace LMCIS.controller.Utilities
{
    public interface IDatabase
    {
        // string GetConnectionString();
        // string TestConnection(List<string> connectionStrings);
        void Dispose();
        void ExecuteInTransaction(Action action);
        object ExecuteScalarCommand(string sqlQuery, Dictionary<string, object> queryParameters);
        void ExecuteNonQueryCommand(string sqlQuery, Dictionary<string, object> queryParameters);
        MySqlDataReader ExecuteReaderCommand(string sqlQuery, Dictionary<string, object> queryParameters);
        MySqlCommand CreateCommand(string query, Dictionary<string, object> parameters);
        void ExecuteNonQuery(MySqlCommand command);
        DataTable ExecuteDataTable(string sqlQuery, Dictionary<string, object> queryParameters);
        DataTable ExecuteDataTable(string sqlQuery);
        object ExecuteScalar(string sqlCmd);
        string ExecuteScalarCommand(string sqlQuery);

        object ExecuteCommand(string sqlQuery, Dictionary<string, object> queryParameters,
            Func<MySqlCommand, object> execute);
    }
}
