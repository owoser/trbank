using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace ClassLibrary1
{
    internal class SqlHelper
    {
        public bool inture;
        public int s;
        static string a = "data source = 4.owox.tk; port = 3306; database = trbank; user = trbank; password = jFGbHk6yWciTjAP8; charset = utf8;";
        MySqlConnection cnn = new MySqlConnection(a);
        
        public void creatSql1(string name,string xuid)
        {
            cnn.Open();
            string sql = $"SELECT * FROM trbank WHERE xuid = '{xuid}'";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            MySqlDataReader reader = cmd.ExecuteReader();
            cmd.Dispose();
            inture = reader.HasRows;
            reader.Close();
            if (inture == false)
            {
                string sql1 = $"insert into trbank (name,money,xuid) values('{name}',0,'{xuid}')"; ;
                MySqlCommand cmd1 = new MySqlCommand(sql1, cnn);
                cmd1.ExecuteNonQuery();
            }
            cnn.Close();
        }
        public void ReadSql(string xuid)
        {
            cnn.Open();
            string sql2 = $"select money from trbank where xuid= '{xuid}'"; ;
            MySqlCommand cmd1 = new MySqlCommand(sql2, cnn);
            s = (int)cmd1.ExecuteScalar();
            cnn.Close();
        }

        public void Updata(int money,string xuid,string model)
        {
            cnn.Open();
            string sql3 = $"UPDATE trbank SET money = money {model} {money} WHERE xuid = '{xuid}'";
            MySqlCommand cmd2 = new MySqlCommand(sql3, cnn);
            cmd2.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
