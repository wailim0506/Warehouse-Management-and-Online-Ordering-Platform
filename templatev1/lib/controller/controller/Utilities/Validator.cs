using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LMCIS.controller.Utilities
{
    public class Validator
    {
        private readonly IDatabase _database;

        public Validator(IDatabase database = null)
        {
            _database = database;
        }

        private object ExecuteScalarCommand(string query, Dictionary<string, object> parameters)
        {
            if (_database != null) return _database.ExecuteScalarCommand(query, parameters);

            var database = new Database();
            try
            {
                return database.ExecuteScalarCommand(query, parameters);
            }
            finally
            {
                database.Dispose();
            }
        }
        private static readonly string EmailRegex = @"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$";

        private static readonly string PasswordRegex =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        private static readonly string NameRegex = @"^[a-zA-Z]+$";
        private static readonly string PhoneNumberRegex = @"^([0-9]{11})$";
        private static readonly string PhoneNumberRegex2 = @"^([0-9]{8})$";
        private static readonly string UsernameRegex = @"^(LMS|LMC)\d{5}$";
        private static readonly string EnglishAddressRegex = @"^[a-zA-Z0-9\s]+$";
        private static readonly string ChineseAddressRegex = @"^[\u4e00-\u9fa5]+$";


        // check if the orderId is valid
        public bool IsValidOrderId(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId)) return false;
            return ExecuteScalarCommand("SELECT OrderID FROM order_ WHERE OrderID = @orderId",
                new Dictionary<string, object> { { "@orderId", orderId } }) != null;
        }

        public bool IsValidRelayId(string relayId)
        {
            if (string.IsNullOrWhiteSpace(relayId)) return false;
            return ExecuteScalarCommand("SELECT RelayID FROM deliveryrelay WHERE RelayID = @relayId",
                new Dictionary<string, object> { { "@relayId", relayId } }) != null;
        }

        public bool IsValidRelayName(string relayName)
        {
            if (string.IsNullOrWhiteSpace(relayName)) return false;
            return ExecuteScalarCommand("SELECT RelayName FROM deliveryrelay WHERE RelayName = @relayName",
                new Dictionary<string, object> { { "@relayName", relayName } }) != null;
        }

        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailRegex);
        }

        public bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$");
        }
        
        public bool IsValidPassword(string inputPassword)
        {
            // return Regex.IsMatch(inputPassword, PasswordRegex) && !IsPotentialSqlInjection(inputPassword);
            return !string.IsNullOrEmpty(inputPassword) && !IsPotentialSqlInjection(inputPassword);
        }

        private bool IsPotentialSqlInjection(string userInput)
        {
            if (string.IsNullOrEmpty(userInput)) return false;
            // HashSet of patterns for SQL Injection
            HashSet<string> sqlCheckList = new HashSet<string>
            {
                "char", "nchar", "varchar", "nvarchar",
                "alter", "begin", "cast", "create", "cursor",
                "declare", "delete", "drop", "end", "exec", "execute",
                "fetch", "insert", "kill", "open",
                "select", "sys", "sysobjects", "syscolumns",
                "table", "update", "union"
            };
            // Check if any pattern is found in the user input
            return userInput.ToLower().Split(' ').Any(word => sqlCheckList.Contains(word));
        }

        public bool IsValidUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return Regex.IsMatch(username, UsernameRegex) && !IsPotentialSqlInjection(username);
        }

        public bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            return (Regex.IsMatch(phone, PhoneNumberRegex) || Regex.IsMatch(phone, PhoneNumberRegex2)) && !IsPotentialSqlInjection(phone);
        }
    }
}
