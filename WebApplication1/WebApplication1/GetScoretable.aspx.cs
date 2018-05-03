using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class GetScoretable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                //using végén bezár az sql kapcsolat
            {
               // Response.Write(sql.connectstring);
                MySqlDataReader dataReader = sql.Query("select * from scores order by score desc");
                string data = "";

                while (dataReader.Read())
                {
                    // (id, username, score)
                    
                    string name = dataReader.GetString(1).ToString();
                    string score = dataReader.GetString(2).ToString();

                    data += name + "|" + score + "/";
                }

                data = data.Substring(0, data.Length - 1);

                Response.Write(data);
               
            }
        }
    }
}