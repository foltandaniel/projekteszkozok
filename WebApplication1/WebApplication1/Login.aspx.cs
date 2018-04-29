using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            Player player = new Player(username, password);
            if (player.getStatus() == true)
            {
                Response.Write("OK|" + player.getToken());
            }

            else
            {
                Response.Write("HIBA|nem megfelelő felhasználó jelszó páros");
            }

        }
    }
}