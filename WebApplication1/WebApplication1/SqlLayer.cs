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
        
        static SqlLayer()
        {
            MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
            connectionString.Server = "127.0.0.1";
            connectionString.UserID = "foltandaniel";
            connectionString.Password = "wL7nX84PMhXbSqZj";
            connectionString.Database = "ASP_DB";

            connection = new MySqlConnection(connectionString.ToString());
            connection.Open();
        }


        public static MySqlDataReader Query(string query)
        {
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;

            return command.ExecuteReader();
        }

        public static int Execute(string query)
        {
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;

            return command.ExecuteNonQuery();
        }
    }
}