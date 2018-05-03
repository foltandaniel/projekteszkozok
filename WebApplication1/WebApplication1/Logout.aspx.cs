using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["token"]==null)
            {
                return;
            }
            Player user = new Player(Request.Form["token"].ToString());

            using (SqlLayer sql = new SqlLayer())
            {
                sql.Execute("UPDATE users SET token='' WHERE username='" + user.getUser() + "'");
            }
        }
    }
}