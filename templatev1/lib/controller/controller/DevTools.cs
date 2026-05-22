

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class DevTools : abstractController
    {
        private Database _db = new Database();
        private const string StaffAccountTable = "staff_account";
        private const string CustomerAccountTable = "customer_account";
        private const string StaffIdField = "staffID";
        private const string CustomerIdField = "customerID";

        public bool UpdatePassword(Dictionary<string, string> usersToUpdate)
        {
            try
            {
                foreach (var user in usersToUpdate)
                {
                    var oldPassword = GetOldPassword(user.Key);
                    var hashedPassword = HashPassword(oldPassword);
                    var sqlQuery = ResolveUpdatePasswordQuery(user.Key);
                    ExecuteNonQuery(sqlQuery, new Dictionary<string, object>
                    {
                        { "@password", hashedPassword },
                        { "@userID", user.Key }
                    });
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private string GetOldPassword(string userId)
        {
            var selectQuery = ResolveSelectPasswordQuery(userId);
            var dt = ExecuteSqlQuery(
                selectQuery,
                new Dictionary<string, object> { { "@userID", userId } });
            var key = Convert.FromBase64String(dt.Rows[0]["pwdKEY"].ToString());
            var iv = Convert.FromBase64String(dt.Rows[0]["pwdIV"].ToString());
            var pwdbyte = Convert.FromBase64String(dt.Rows[0]["password"].ToString());
            return Decrypt(pwdbyte, key, iv);
        }

        private string Decrypt(byte[] cipheredtext, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredtext))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public bool DeveloperToolForgetPassword(string userid, string newPassword)
        {
            string password = HashPassword(newPassword);
            string sqlQuery = ResolveUpdatePasswordQuery(userid);
            ExecuteNonQuery(sqlQuery, new Dictionary<string, object> { { "@password", password }, { "@userID", userid } });
            DataTable dt = ExecuteSqlQuery(
                ResolveSelectPasswordQuery(userid),
                new Dictionary<string, object> { { "@userID", userid } });
            return dt.Rows.Count == 1;
        }

        private static string ResolveUpdatePasswordQuery(string userId)
        {
            return IsStaffUser(userId)
                ? $"UPDATE {StaffAccountTable} SET password = @password WHERE {StaffIdField} = @userID"
                : $"UPDATE {CustomerAccountTable} SET password = @password WHERE {CustomerIdField} = @userID";
        }

        private static string ResolveSelectPasswordQuery(string userId)
        {
            return IsStaffUser(userId)
                ? $"SELECT * FROM {StaffAccountTable} WHERE {StaffIdField} = @userID"
                : $"SELECT * FROM {CustomerAccountTable} WHERE {CustomerIdField} = @userID";
        }

        private static bool IsStaffUser(string userId)
        {
            return !string.IsNullOrWhiteSpace(userId) && userId.StartsWith("LMS");
        }


        private DataTable ExecuteSqlQuery(string sqlQuery)
        {
            return _db.ExecuteDataTable(sqlQuery, null);
        }

        private DataTable ExecuteSqlQuery(string sqlQuery, Dictionary<string, object> parameters)
        {
            return _db.ExecuteDataTable(sqlQuery, parameters);
        }

        private void ExecuteNonQuery(string sqlQuery, Dictionary<string, object> parameters)
        {
            _db.ExecuteNonQueryCommand(sqlQuery, parameters);
        }

        public static string HashPassword(string password)
        {
            return RecoveryController.HashPassword(password);
        }

        public void UpdateDeveloperAccount()
        {
            var usersToUpdate = new Dictionary<string, string>
            {
                { "LMC00001", "password123" },
                { "LMC00002", "password123" },
                { "LMS00001", "password123" },
                { "LMS00002", "abc123456" },
                { "LMS00003", "xyz789!@#" },
                { "LMS00004", "qwer5678" },
                { "LMS00005", "asdf1234!" }
            };

            foreach (var user in usersToUpdate)
            {
                MessageBox.Show(UpdatePasswordAndTestLogin(user.Key, user.Value)
                    ? $"Password updated and login successful at {user.Key}"
                    : $"Update or login failed at {user.Key}");
            }
        }

        private bool UpdatePasswordAndTestLogin(string userId, string newPassword)
        {
            if (!DeveloperToolForgetPassword(userId, newPassword)) return false;
            return TestLogin(userId, newPassword);
        }

        private bool TestLogin(string userId, string password)
        {
            var accountController = new AccountController();
            var UIController = new UIController(accountController);
            return accountController.Login(userId, password, UIController);
        }
    }
}
