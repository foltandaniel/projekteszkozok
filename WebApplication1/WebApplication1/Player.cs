using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace web
{
    public class Player
    {
        private String user;
        private String token;
        private bool status; // valid felhasznalo vagy nem
        private int score;
        public Player(String username, String password) //loginhoz felhasználó jelszó kombinációval hívjuk meg
        {
            user = username;
            status = Validate(username, password); //megnézzük, hogy helyes adatokat kaptunk e
            if (status == true)
            {
                token = CheckToken(username);  //megnézi, hogy létezik e már a token, ha igen beállítja tokennek
                if (token == "")
                {
                    token = GenerateToken(username); //ha még nincs token akkor generál

                    //berakja adatbázisba az új tokent
                    using (SqlLayer sql = new SqlLayer())
                    {
                        int newtoken = sql.Execute("update users set token='" + token + "' where username='" + username + "'");
                    }
                }
                
            }
        }

        public Player(String token) //token konstruktor
        {
            this.token = token;
            status = Validate(token);  //megnézzük, hogy az adott tokenhez tartozik e felhasználó
            if (status == true)
            {
                user = GetUser(token); //lekérjük a tokenhez tartozó felhasználót
            }
        }

        //validate függvény megnézi, hogy pontosan 1 felhasználó tartozik e a megadott adatokhoz count lekérdezéssel
        private bool Validate(String user, String pass)
        {
            
            using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                                                  //using végén bezár az sql kapcsolat
            {
                // Response.Write(sql.connectstring);
                MySqlDataReader data = sql.Query("select count(username) from users where username='"+user+"' and password='"+pass+"'");
                //data -bán lesz az adat. 1 sorból fog állni, hiszen count(username) selectet írtunk (1 sort ad vissza)

                data.Read(); //a jelenlegi pointert tovább mozgatjuk (első sorra lépünk)


               int count = data.GetInt32(0); //a jelenlegi sor 0-adik oszlopjára vagyunk kíváncsiak, ami egy int32
                
                return count == 1;
            }
        } 

        private bool Validate(String token)
        {
            using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                                                  //using végén bezár az sql kapcsolat
            {
                // Response.Write(sql.connectstring);
                MySqlDataReader data = sql.Query("select count(token) from users where token='"+token+"'");

                data.Read();
                return data.GetInt32(0) == 1;
            }
        }

        //megnézi, hogy létezik e token az adott felhasználóhoz, lekérve a token mezőjét,
        // ha nincs üres Stringet ad vissza
        private String CheckToken(String username)
        {

            String token= "";
            using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                                                  //using végén bezár az sql kapcsolat
            {
                // Response.Write(sql.connectstring);
                MySqlDataReader dataReader = sql.Query("select token from users where username='"+username+"'");


                while (dataReader.Read())
                {
                    // (id, username, pass,token)

                    token = dataReader.GetString(0).ToString();

                }



            }
            return token;
        }

        //rekurzívan legenerál egy tokent amíg egyedit nem kap
        private String GenerateToken(String username)
        {
            Random rnd = new Random();
            int rnd1= rnd.Next(1, 999999);
            int rnd2 = rnd.Next(1, 9999999);
            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _key = Encoding.ASCII.GetBytes(username);
            byte[] _rnd2 = BitConverter.GetBytes(rnd2);
            byte[] _rnd1= BitConverter.GetBytes(rnd1);
            byte[] data = new byte[_time.Length + _key.Length + _rnd1.Length + _rnd2.Length];

            System.Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            System.Buffer.BlockCopy(_key, 0, data, _time.Length, _key.Length);
            System.Buffer.BlockCopy(_rnd1, 0, data, _time.Length + _key.Length, _rnd1.Length);
            System.Buffer.BlockCopy(_rnd2, 0, data, _time.Length + _key.Length + _rnd1.Length, _rnd2.Length);
            String token= Convert.ToBase64String(data.ToArray());

            //a validate függvény segítségével deríti ki, hogy létezik e már az adott token.
            if (Validate(token) == true)
            {
                return GenerateToken(username);
            }

           

            return token;
        }

        private String GetUser(String token)
        {
            String name="";
            using (SqlLayer sql = new SqlLayer()) // sql kapcsolat
                                                  //using végén bezár az sql kapcsolat
            {
                // Response.Write(sql.connectstring);
                MySqlDataReader dataReader = sql.Query("select * from users where token='"+token+"'");
                

                while (dataReader.Read())
                {
                    // (id, username, pass,token,score)

                    name = dataReader.GetString(1).ToString();
                    score = dataReader.GetInt32(4);
                    
                }

                

            }
            return name;

        }

        public String getToken()
        {
            return token;
        }

        public bool getStatus()
        {
            return status;
        }

        public String getUser()
        {
            return user;
        }

        public int getScore()
        {
            return score;
        }
    }
}