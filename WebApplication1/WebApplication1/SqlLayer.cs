using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace web
{
    public static class SqlLayer
    {
        private static MySqlConnection connection;
        public static string result;
        static SqlLayer()
        {
            MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
            connectionString.Server = "194.182.67.11";
            connectionString.UserID = "foltandaniel";
            connectionString.Password = "wL7nX84PMhXbSqZj";
            connectionString.Database = "ASP_DB";

            result = connectionString.ToString();

            connection = new MySqlConnection(result);

            Console.WriteLine(result);
        }

        public static string GetScoreBoard()
        {
            return result;
        }
    }
}