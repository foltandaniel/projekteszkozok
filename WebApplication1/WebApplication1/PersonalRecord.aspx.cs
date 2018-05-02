using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class PersonalRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["token"] == null)
            {
                return;
            }
            string token = Request.Form["token"];
            Player player = new Player(token);
            if (player.getStatus() == true)
            {
                Response.Write("OK|" + player.getScore());


            }
            else
            {
                Response.Write("HIBA| hibás token");
            }

        }
    }
}