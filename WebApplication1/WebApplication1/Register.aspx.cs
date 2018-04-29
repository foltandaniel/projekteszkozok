using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    public partial class Register : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);
        }

        private bool CheckUsername(SqlLayer sql,string name)
        {
            MySqlDataReader data = sql.Query("SELECT * FROM users WHERE username='" + name + "'");


           bool taken = data.HasRows;
            data.Close();
            return taken;
        }
        protected void Button1_Click(object sender,EventArgs e)
        {
            result.Text = "";
            if(uname.Text == "")
            {
                result.Text = "Invalid username!";
                return;
            }
            if(pw.Text != pw_repeat.Text)
            {
                result.Text = "Passwords does not match!";
                return;
            }
            using (SqlLayer sql = new SqlLayer())
            {
                if(CheckUsername(sql,uname.Text))
                {
                    result.Text = "Name already taken!";
                    return;
                }

                //OK, REGISZTRÁCIÓ
                string query = string.Format("INSERT INTO users VALUES(null,'{0}','{1}','',0)",
                    uname.Text, MD5Hash(pw.Text));
                sql.Execute(query);
                result.Text = "REGISZTRÁLVA!";
            }
        }




        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


    }
}