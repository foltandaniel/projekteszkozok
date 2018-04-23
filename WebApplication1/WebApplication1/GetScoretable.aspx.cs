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
            MySqlDataReader dataReader = SqlLayer.Query("select * from scores order by score desc");
            string data = "";

            while (dataReader.Read())
            {
                // (id, username, score)
                string name = dataReader.GetString(1);
                string score = dataReader.GetString(2);

                data += name + "|" + score + "/"; 
            }

            Response.Write(data);
        }
    }
}