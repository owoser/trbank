using LiteLoader.Logger;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace ClassLibrary1
{
    internal class SqlHelper
    {
        public bool inture;
        public uint s;
        private static LoadConfig cfg = new();
        private static string a = $"data source = {cfg.get("db", "addr")}; port = {cfg.get("db", "port")}; database = {cfg.get("db", "base")}; user = {cfg.get("db", "user")}; password = {cfg.get("db", "pass")}; charset = utf8;";
        private MySqlConnection cnn = new MySqlConnection(a);
        private Logger logger = new("trbank");

        private void open()
        {
            try
            {
                cnn.Open();
            }
            catch(Exception ex)
            {
                logger.Error.WriteLine("数据库连接失败！");
                logger.Error.WriteLine(ex.Message);
            }
        }

        public void test()
        {
            this.open();
            cnn.Close();
        }

        public bool exist(string xuid)
        {
            bool result;

            this.open();

            string sql = $"SELECT xuid FROM trbank WHERE xuid = '{xuid}'";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            MySqlDataReader reader = cmd.ExecuteReader();
            cmd.Dispose();

            result = reader.HasRows;

            reader.Close();
            cnn.Close();

            return result;
        }
        
        public bool creatSql1(string name,string xuid)
        {
            bool notExist = !this.exist(xuid);

            if (notExist)
            {
                this.open();

                string sql1 = $"insert into trbank (name,money,xuid) values('{name}',0,'{xuid}')"; ;
                MySqlCommand cmd1 = new MySqlCommand(sql1, cnn);
                cmd1.ExecuteNonQuery();

                cnn.Close();

                return true;
            }

            return false;
        }
        public void ReadSql(string xuid)
        {
            this.open();

            string sql2 = $"select money from trbank where xuid= '{xuid}'"; ;
            MySqlCommand cmd1 = new MySqlCommand(sql2, cnn);
            s = (uint)cmd1.ExecuteScalar();
            cnn.Close();
        }

        public void Updata(uint money,string xuid,string model)
        {
            this.open();

            string sql3 = $"UPDATE trbank SET money = money {model} {money} WHERE xuid = '{xuid}'";
            MySqlCommand cmd2 = new MySqlCommand(sql3, cnn);
            cmd2.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
