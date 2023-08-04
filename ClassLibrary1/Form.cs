using LiteLoader.Form;
using LLMoney;
using MC;
using System.Linq.Expressions;

namespace ClassLibrary1
{

    internal class Form
    {
        private static readonly SimpleForm BuildBaseForm = new("OWO跨服银行", ""); 
        private static readonly CustomForm CunQian = new("存钱");
        private static readonly CustomForm QuQian = new("取钱");
        SqlHelper sqlHelper = new SqlHelper();
        public void Form1()
        {
            BuildBaseForm.Append(new("开户"));
            BuildBaseForm.Append(new("存钱"));
            BuildBaseForm.Append(new("取钱"));
            
            BuildBaseForm.Callback = (pl, val) =>
            {
                switch (val)
                {
                    case 0:
                        
                       
                         sqlHelper.creatSql1(pl.RealName, pl.Xuid);
                            
                         if (sqlHelper.inture == true)
                         {
                            pl.SendText("已有账户无需再次开通!");
                         }
                         else
                         {
                             pl.SendText("账户开通成功!");
                         }
                        break;
                    case 1:
                        SendFrom2(pl);
                        break;
                    default:
                        SendFrom3(pl);
                        break;
                }
            };
        }
        public void SendFrom1(Player name) {
            
            BuildBaseForm.SendTo(name);
        }

        public void From2()
        {

            CunQian.Append(new Label("Label1Name", $"存钱"));
            CunQian.Append(new Input("InputName", "输入你要存入钱的数量"));
            
            CunQian.Callback = (pl, val) =>
            {
                try
                {
                    
                    Input input = (Input)val["InputName"];
                    string a = input.Value;
                    int n;
                    bool isNumeric = int.TryParse(a, out n);
                    if (isNumeric == false)
                    {
                        pl.SendText("你存钱可以存字符串是吧");
                    }
                    else
                    {
                        int hav = (int)EconomySystem.GetMoney(pl.Xuid);
                        if (hav < n)
                        {
                            pl.SendText($"余额不足，存钱失败! 余额:{hav.ToString()}");
                        }
                        else
                        {
                            EconomySystem.ReduceMoney(pl.Xuid, n);
                            string model = "+";
                            sqlHelper.Updata(n, pl.Xuid, model);
                            sqlHelper.ReadSql(pl.Xuid);
                            pl.SendText($"已存入:{n},当前余额{sqlHelper.s}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            };
        }
        public void SendFrom2(Player name)
        {
            CunQian.SendTo(name);
        }
        public void Form3()
        {
            QuQian.Append(new Label("Label2Name", "取钱"));
            QuQian.Append(new Input("Input2Name", "输入你要取出的钱的数量"));
            
            QuQian.Callback = (pl, val) =>
            {
                try
                {
                    Input input = (Input)val["Input2Name"];
                    string a = input.Value;
                    int n;
                    bool isNumeric = int.TryParse(a, out n);
                    if (isNumeric == false)
                    {
                        pl.SendText("你取钱可以取字符串是吧");
                    }
                    else
                    {
                        sqlHelper.ReadSql(pl.Xuid);
                        if (sqlHelper.s < n)
                        {
                            pl.SendText($"余额不足，取钱失败！余额:{sqlHelper.s}");
                        }
                        else
                        {
                            EconomySystem.AddMoney(pl.Xuid, n);
                            string model = "-";
                            sqlHelper.Updata(n, pl.Xuid, model);
                            sqlHelper.ReadSql(pl.Xuid);
                            pl.SendText($"已取出:{n},当前余额{sqlHelper.s}");
                        }
                    }
                }
                catch (Exception ex) { 
                
                }
            };
        }
        public void SendFrom3(Player name)
        {
            QuQian.SendTo(name);
        }
    }
}
