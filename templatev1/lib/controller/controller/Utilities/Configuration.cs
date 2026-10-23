using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace LMCIS.controller.Utilities
{
    public class Configuration
    {
        public Configuration()
        {
            CreateAvatarFolder();
        }

        private static string GetApiKey() => ConfigurationManager.AppSettings["GoogleMapsApiKey"] ??
                                             throw new Exception("No valid Google Maps Api Key found.");

        internal static List<string> GetDataBaseConnectionStringsList()
        {
            return ConfigurationManager.AppSettings.AllKeys
                .Where(key => key.StartsWith("ConnectionString"))
                .Select(key => ConfigurationManager.AppSettings[key])
                .Where(connectionString => !string.IsNullOrEmpty(connectionString))
                .ToList();
        }

        public static string DataBaseConnectionString => TestConnection(GetDataBaseConnectionStringsList()) ??
                                                         throw new Exception("No valid connection string found.");

        public static string TestConnection(List<string> connectionStrings)
        {
            return connectionStrings.FirstOrDefault(connectionString =>
            {
                try
                {
                    using (var connection =
                           new MySqlConnection(new MySqlConnectionStringBuilder(connectionString).ConnectionString))
                    {
                        connection.Open();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }) ?? throw new Exception("No valid connection string found.");
        }

        public static string GoogleMapsApiKey => GetApiKey();

        public static bool IsDevMode()
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings["DevMode"]);
            }
            catch
            {
                // check if the key is not found in the app.config Default value is false
                return false;
            }
        }

        public static void CreateAvatarFolder()
        {
            string path = Directory.GetCurrentDirectory() + "\\Upload\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}