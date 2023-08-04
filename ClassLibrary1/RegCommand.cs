using LiteLoader.DynamicCommand;
using MC;
using static LiteLoader.DynamicCommand.DynamicCommand;
using LiteLoader.Logger;
using LiteLoader.Form;

namespace ClassLibrary1
{
    internal class RegCommand
    {
        Form a1 = new Form();
        Logger logger = new("trbank");
        DynamicCommandInstance cmd = CreateCommand("trbank", "A bank for owoser", CommandPermissionLevel.Any);
        public void CommandRegisiter()
        {
            logger.Info.WriteLine("Registering Commands...");
            logger.Info.WriteLine("正在注册命令……");
            cmd.SetAlias("owobank");
            cmd.SetCallback((cmd, origin, output, results) =>
            {
                a1.SendFrom1(origin.Player);
            });
            cmd.AddOverload(new List<string>());
            Setup(cmd);
            logger.Info.WriteLine("Commands Registered!");
            logger.Info.WriteLine("命令注册成功！");
        }
    }
    
}
