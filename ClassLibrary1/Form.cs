using LiteLoader.Form;
using LiteLoader.Logger;
using LLMoney;
using MC;
using System.Linq.Expressions;

namespace ClassLibrary1
{
    internal class Form
    {
        private readonly static LoadConfig cfg = new();
        private readonly SqlHelper sqlHelper = new();
        private readonly Logger logger = new("trbank");
        private readonly static SimpleForm BuildBaseForm = new(cfg.get("form", "main", "title"), "");

        public void BuildForm1()
        {
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 0, "text"), cfg.get("form", "main", "button", 0, "icon")));
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 3, "text"), cfg.get("form", "main", "button", 3, "icon")));
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 1, "text"), cfg.get("form", "main", "button", 1, "icon")));
            BuildBaseForm.Append(new Button(cfg.get("form", "main", "button", 2, "text"), cfg.get("form", "main", "button", 2, "icon")));

            BuildBaseForm.Callback = (pl, val) =>
            {
                switch (val)
                {
                    case 0:
                        long wallet = EconomySystem.GetMoney(pl.Xuid);
                        long cost = Convert.ToInt32(cfg.get("form", "main", "cost"));

                        if (wallet >= cost)
                        {
                            bool isCreat = sqlHelper.creatSql1(pl.RealName, pl.Xuid);

                            if (isCreat)
                            {
                                EconomySystem.ReduceMoney(pl.Xuid, cost);
                                string str_ = cfg.get("form", "main", "feedback", "kaihu_success");
                                pl.SendText(str_.Replace("{cost}", cost.ToString()));
                            }
                            else
                            {
                                pl.SendText(cfg.get("form", "main", "feedback", "kaihu_have_existed"));
                            }
                        }
                        else
                        {
                            string stra = cfg.get("form", "main", "feedback", "kaihu_cost_not_enough");
                            pl.SendText(stra.Replace("{cost}", cost.ToString()));
                        }
                        break;
                    case 1:

                        bool notExist = !sqlHelper.exist(pl.Xuid);

                        if (notExist)
                        {
                            pl.SendText(cfg.get("form", "common", "feedback", "not_exist"));

                            return;
                        }

                        sqlHelper.ReadSql(pl.Xuid);

                        string balance = sqlHelper.s.ToString();
                        string str = cfg.get("form", "main", "feedback", "query_account");
                        pl.SendText(str.Replace("{money}", balance));

                        break;
                    case 2:
                        SendForm2(pl);
                        break;
                    case 3:
                        SendForm3(pl);
                        break;
                }
            };
        }

        public static void SendForm1(Player player)
        {
            BuildBaseForm.SendTo(player);
        }

        public void SendForm2(Player player)
        {
            CustomForm CunQian = new(cfg.get("form", "cunqian", "title"));

            string str = cfg.get("form", "cunqian", "label", 0);
            string wallet = EconomySystem.GetMoney(player.Xuid).ToString();
            CunQian.Append(new Label("Label1Name", str.Replace("{wallet}", wallet)));
            CunQian.Append(new Input("InputName", cfg.get("form", "cunqian", "input", 0)));
            
            CunQian.Callback = (pl, val) =>
            {
                if (val.Count == 0)
                {
                    CunQian.Dispose();
                    SendForm1(pl);

                    return;
                };

                bool notExist = !sqlHelper.exist(pl.Xuid);

                if (notExist)
                {
                    pl.SendText(cfg.get("form", "common", "feedback", "not_exist"));

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

                    bool isNumeric = uint.TryParse(a, out uint n);
                    if (isNumeric == false)
                    {
                        if (long.TryParse(a, out long xxxxx))
                        {
                            pl.SendText(cfg.get("form", "common", "feedback", "too_big_number"));

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

            CunQian.SendTo(player);
        }

        public void SendForm3(Player player)
        {
            CustomForm QuQian = new(cfg.get("form", "quqian", "title"));

            bool notExist = !sqlHelper.exist(player.Xuid);

            if (notExist)
            {
                player.SendText(cfg.get("form", "common", "feedback", "not_exist"));

                return;
            }

            sqlHelper.ReadSql(player.Xuid);

            string str = cfg.get("form", "quqian", "label", 0);
            string balance = sqlHelper.s.ToString();
            QuQian.Append(new Label("Label2Name", str.Replace("{balance}", balance)));
            QuQian.Append(new Input("Input2Name", cfg.get("form", "quqian", "input", 0)));
            
            QuQian.Callback = (pl, val) =>
            {
                if (val.Count == 0)
                {
                    QuQian.Dispose();
                    SendForm1(pl);

                    return;
                };

                try
                {
                    Input input = (Input)val["Input2Name"];
                    string a = input.Value;

                    if (a == "")
                    {
                        pl.SendText(cfg.get("form", "quqian", "feedback", "is_air"));

                        return;
                    }

                    bool isNumeric = uint.TryParse(a, out uint n);
                    if (isNumeric == false)
                    {
                        if (long.TryParse(a, out long xxxxx))
                        {
                            pl.SendText(cfg.get("form", "common", "feedback", "too_big_number"));

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

            QuQian.SendTo(player);
        }
    }
}
