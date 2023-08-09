using LiteLoader.Form;
using LiteLoader.Logger;
using LLMoney;
using MC;
using System.Linq.Expressions;

namespace ClassLibrary1
{
    internal class Form
    {
        private static LoadConfig cfg = new LoadConfig();
        private static readonly SimpleForm BuildBaseForm = new(cfg.get("form", "main", "title"), ""); 
        private static readonly CustomForm CunQian = new(cfg.get("form", "cunqian", "title"));
        private static readonly CustomForm QuQian = new(cfg.get("form", "quqian", "title"));
        SqlHelper sqlHelper = new SqlHelper();
        private Logger logger = new("trbank");

        public void Form1()
        {
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 0, "text"), cfg.get("form", "main", "button", 0, "icon")));
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 1, "text"), cfg.get("form", "main", "button", 1, "icon")));
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 2, "text"), cfg.get("form", "main", "button", 2, "icon")));

            BuildBaseForm.Callback = (pl, val) =>
            {
                switch (val)
                {
                    case 0:
                         bool isCreat = sqlHelper.creatSql1(pl.RealName, pl.Xuid);
                            
                         if (isCreat)
                         {
                             pl.SendText(cfg.get("form", "main", "feedback", "kaihu_success"));
                         }
                         else
                         {
                             pl.SendText(cfg.get("form", "main", "feedback", "kaihu_have_existed"));
                         }
                        break;
                    case 1:
                        SendFrom2(pl);
                        break;
                    case 2:
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
            CunQian.Append(new Label("Label1Name", cfg.get("form", "cunqian", "label", 0)));
            CunQian.Append(new Input("InputName", cfg.get("form", "cunqian", "input", 0)));
            
            CunQian.Callback = (pl, val) =>
            {
                if (val.Count == 0) return;

                bool notExist = !sqlHelper.exist(pl.Xuid);

                if (notExist)
                {
                    pl.SendText(cfg.get("form", "cunqian", "feedback", "not_exist"));

                    return;
                }

                try
                {
                    
                    Input input = (Input)val["InputName"];
                    string a = input.Value;

                    if (a == "")
                    {
                        pl.SendText(cfg.get("form", "cunqian", "feedback", "is_air"));

                        return;
                    }

                    uint n;
                    bool isNumeric = uint.TryParse(a, out n);
                    if (isNumeric == false)
                    {
                        if (long.TryParse(a, out long xxxxx))
                        {
                            pl.SendText(cfg.get("form", "cunqian", "feedback", "too_big_number"));

                            return;
                        }

                        pl.SendText(cfg.get("form", "cunqian", "feedback", "format_error"));
                    }
                    else
                    {
                        int hav = (int)EconomySystem.GetMoney(pl.Xuid);
                        if (hav < n)
                        {
                            string str = cfg.get("form", "cunqian", "feedback", "not_enough_money");
                            pl.SendText(str.Replace("{money}", hav.ToString()));
                        }
                        else
                        {
                            EconomySystem.ReduceMoney(pl.Xuid, n);
                            string model = "+";
                            sqlHelper.Updata(n, pl.Xuid, model);
                            sqlHelper.ReadSql(pl.Xuid);

                            string str = cfg.get("form", "cunqian", "feedback", "success");
                            pl.SendText(str.Replace("{deposit}", n.ToString())
                                           .Replace("{money}", sqlHelper.s.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error.WriteLine(ex.Message);
                }
            };
        }
        public void SendFrom2(Player name)
        {
            CunQian.SendTo(name);
        }
        public void Form3()
        {
            QuQian.Append(new Label("Label2Name", cfg.get("form", "quqian", "label", 0)));
            QuQian.Append(new Input("Input2Name", cfg.get("form", "quqian", "input", 0)));
            
            QuQian.Callback = (pl, val) =>
            {
                if (val.Count == 0) return;

                bool notExist = !sqlHelper.exist(pl.Xuid);

                if (notExist)
                {
                    pl.SendText(cfg.get("form", "quqian", "feedback", "not_exist"));

                    return;
                }

                try
                {
                    Input input = (Input)val["Input2Name"];
                    string a = input.Value;

                    if (a == "")
                    {
                        pl.SendText(cfg.get("form", "quqian", "feedback", "is_air"));

                        return;
                    }

                    uint n;
                    bool isNumeric = uint.TryParse(a, out n);
                    if (isNumeric == false)
                    {
                        if (long.TryParse(a, out long xxxxx))
                        {
                            pl.SendText(cfg.get("form", "quqian", "feedback", "too_big_number"));

                            return;
                        }

                        pl.SendText(cfg.get("form", "quqian", "feedback", "format_error"));
                    }
                    else
                    {
                        sqlHelper.ReadSql(pl.Xuid);
                        if (sqlHelper.s < n)
                        {
                            string str = cfg.get("form", "quqian", "feedback", "not_enough_money");
                            pl.SendText(str.Replace("{money}", sqlHelper.s.ToString()));
                        }
                        else
                        {
                            EconomySystem.AddMoney(pl.Xuid, n);
                            string model = "-";
                            sqlHelper.Updata(n, pl.Xuid, model);
                            sqlHelper.ReadSql(pl.Xuid);

                            string str = cfg.get("form", "quqian", "feedback", "success");
                            pl.SendText(str.Replace("{fetch}", n.ToString())
                                           .Replace("{money}", sqlHelper.s.ToString()));
                        }
                    }
                }
                catch (Exception ex) {
                    logger.Error.WriteLine(ex.Message);
                }
            };
        }
        public void SendFrom3(Player name)
        {
            QuQian.SendTo(name);
        }
    }
}
