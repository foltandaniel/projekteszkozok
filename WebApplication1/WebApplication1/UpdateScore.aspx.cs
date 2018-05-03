using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class UpdateScore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //megkapja tokent és a score-t


            if (Request.Form["token"] == null)
            {
                return;
            }
            string token = Request.Form["token"];
			
			
			// Ellenőrizzük az inputot
            int score = 0;
			if (Request.Form["score"] != null)
			{
				score = int.Parse( Request.Form["score"]);
			}
            Response.Write("Checking user..");

            //tokennel létrehozunk egy playert
            Player player = new Player(token);

            if (player.getStatus() == true)
            {
                //ha valid a token lekérjük a felhasználót
                String user = player.getUser();
                int oldScore = -1;

                int worstId = -1;

                int worstScore = -1;

                //lekérjük a legrosszabb játékos id, score párosát
                using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                                                      //using végén bezár az sql kapcsolat
                {
                    // Response.Write(sql.connectstring);
                    MySqlDataReader dataReader = sql.Query("select * from scores order by Score asc LIMIT 1");
                    

                    while (dataReader.Read())
                    {
                        // (id, username, score)


                        worstId = dataReader.GetInt32(0);
                        worstScore = int.Parse(dataReader.GetString(2).ToString());
                        
                    }
                    dataReader.Close();
                    Response.Write("worst id: " + worstId);

                    //lekérjük a felhasználó eddigi legjobb eredményét
                    dataReader = sql.Query("select TopScore from users where username='" + user + "'");
                    while (dataReader.Read())
                    {
                       
                        oldScore = int.Parse(dataReader.GetString(0).ToString());

                    }

                    dataReader.Close();
                    Response.Write("oldscore: " + oldScore);

                    //ha jobb eredményt ért el akkor frissítünk
                    if (oldScore < score)
                    {
                        Response.Write("new personal record: |"+score+"/");
                        int newscore = sql.Execute("update users set TopScore='" + score + "' where username='" + user + "'");

                    }

                    //ha jobb az eredmény mint a legrosszabb akkor lecseréljük a mostani eredményre.
                    if (score > worstScore)
                    {
                        Response.Write("new record: |" + score + "| place id: |" + worstId);
                        int updateboard = sql.Execute("update scores set Score='" + score + "', Name='"+user+"' where ID='" + worstId + "'");
                    }
                    

                }

            }

        }
    }
}