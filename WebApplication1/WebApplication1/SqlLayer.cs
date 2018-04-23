using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Xml;
namespace web
{

    public class XmlParser : IDisposable
        //xml olvasó - felh/jelszó kiolvasása fájlból
    {
        private XmlDocument xDoc = new XmlDocument();

        public XmlParser()
        {
            xDoc.Load(HttpContext.Current.Server.MapPath(@"pw.password"));

        }

        public void Dispose()
        {
            xDoc = null; 
            //Dispose -oljuk a XML fájlt, GC pedig majd összeszedi a szemetet.
        }

        public string GetValue(string key)
        {
            return xDoc["SqlCredentials"][key].InnerText;
        }
    }
    public class SqlLayer : IDisposable
        /*
         * IDisposable -ből származtatunk, azaz
         * using blokkban használandó!!
         * using scope végén az objekt disposeolva lesz (bezáródik a kapcsolat, memória felszabadítás)
         * új requestkor új kapcsolat jön létre*/
    {
        private MySqlConnection connection;
        public string connectstring;
        public SqlLayer()
        {
            using (XmlParser xmlparser = new XmlParser())
            //csak addig van szükség az xmlparserre, amég létrejön a kapcsolat. kiolvassuk a fájlból a jelszót
            {
                MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();
                connectionString.Server = "127.0.0.1";
                connectionString.UserID = xmlparser.GetValue("username");
                connectionString.Password = xmlparser.GetValue("password");
                connectionString.Database = "ASP_DB";

                connectionString.SslMode = MySqlSslMode.None; //NEM támogatott a mysql SSL
                connectstring = connectionString.ToString();
                connection = new MySqlConnection(connectionString.ToString());
                connection.Open();
            }
        }


        public MySqlDataReader Query(string query)
            //sql queryk futtatásához
        {
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;

            return command.ExecuteReader();
        }

        public int Execute(string query)
            //sql execute
        {
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;

            return command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            connection.Close();
            connection = null;
            //Garbage Collector majd elintézi a többit..
        }
    }
}