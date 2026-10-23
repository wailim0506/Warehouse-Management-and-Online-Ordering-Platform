using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace TestController
{
    public class TestData
    {
        public static readonly string invalidEmail = "peter.zhangweqwe@abc.com";
        public static readonly string invalidPhone = "13412345678";
        public static string UnqualifiedEmail = invalidEmail;
        public static string UnqualifiedPhone = invalidPhone;
        public static string invalidId = "LMS59001";
        public static string invalidPassword = "password103";
        public static string CustomersId1 = "LMS00001";
        public static string CustomersPassword1 = "password123";
        public static string CustomersEmail1 = "peter.zhang@abc.com";
        public static string CustomersPhone1 = "13012345678";
        public static string CustomersId2 = "LMS00002";
        public static string CustomersPassword2 = "1234567890";
        public static string CustomersId3 = "LMC00003";
        public static string CustomersPassword3 = "5432154321";
        public static string CustomersId4 = "LMC00004";
        public static string CustomersPassword4 = "0000000000";
        public static string ClerksId1 = "LMS00008";
        public static string ClerkPassword1 = "asdfghjkl0";
        public static string ClerksId2 = "LMS00007";
        public static string ClerkPassword2 = "1234567890";
        public static string ClerksId3 = "LMS00006";
        public static string ClerkPassword3 = "1234567890";
        public static string ClerksId4 = "LMS00002";
        public static string ClerkPassword4 = "0987654321";
        public static string ClerksId5 = "LMS00003";
        public static string ClerkPassword5 = "xyz789!@#";
        public static string ClerksId6 = "LMS00001";
        public static string ClerkPassword6 = "password123";
        public static string ClerksId7 = "LMS00004";
        public static string ClerkPassword7 = "qwer5678";
        public static string ClerksId8 = "LMS00005";
        public static string ClerkPassword8 = "1234567890";
        public static string DeliverymenId1 = "LMS00009";
        public static string DeliverymenPassword1 = "1234567890";
        public static string DeliverymenId2 = "LMS00010";
        public static string DeliverymenPassword2 = "0987654321";
        public static string DeliverymenId3 = "LMS00011";
        public static string DeliverymenPassword3 = "qwertyuiop";
    }

    internal sealed class RecordingDatabase : LMCIS.controller.Utilities.IDatabase
    {
        internal sealed class Call
        {
            public string Sql { get; set; }
            public Dictionary<string, object> Parameters { get; set; }
        }

        public readonly List<Call> Queries = new List<Call>();
        public readonly List<Call> Commands = new List<Call>();
        public Func<string, Dictionary<string, object>, DataTable> Query { get; set; }

        public DataTable ExecuteDataTable(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            var parameters = queryParameters == null ? new Dictionary<string, object>() : new Dictionary<string, object>(queryParameters);
            Queries.Add(new Call { Sql = sqlQuery, Parameters = parameters });
            if (Query == null) throw new InvalidOperationException("Unexpected query: " + sqlQuery);
            return Query(sqlQuery, parameters);
        }

        public DataTable ExecuteDataTable(string sqlQuery) => ExecuteDataTable(sqlQuery, null);

        public void ExecuteNonQueryCommand(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            Commands.Add(new Call { Sql = sqlQuery, Parameters = queryParameters == null ? new Dictionary<string, object>() : new Dictionary<string, object>(queryParameters) });
        }

        public object ExecuteScalarCommand(string sqlQuery, Dictionary<string, object> queryParameters) => throw new InvalidOperationException("Unexpected scalar query: " + sqlQuery);
        public string ExecuteScalarCommand(string sqlQuery) => throw new InvalidOperationException("Unexpected scalar query: " + sqlQuery);
        public object ExecuteScalar(string sqlCmd) => throw new InvalidOperationException("Unexpected scalar query: " + sqlCmd);
        public MySqlDataReader ExecuteReaderCommand(string sqlQuery, Dictionary<string, object> queryParameters) => throw new InvalidOperationException("Unexpected reader query: " + sqlQuery);
        public MySqlCommand CreateCommand(string query, Dictionary<string, object> parameters) => throw new InvalidOperationException("Unexpected command creation: " + query);
        public void ExecuteNonQuery(MySqlCommand command) => throw new InvalidOperationException("Unexpected command execution.");
        public object ExecuteCommand(string sqlQuery, Dictionary<string, object> queryParameters, Func<MySqlCommand, object> execute) => throw new InvalidOperationException("Unexpected command execution: " + sqlQuery);
        public void Dispose() { }
        public void ExecuteInTransaction(Action action) => action();
    }
}
