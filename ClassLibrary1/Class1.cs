using ClassLibrary1;
using LiteLoader.Event;
using LiteLoader.Logger;
using LiteLoader.NET;


namespace ClassLibrary1;
[PluginMain("trbank")]
public class Trbank : IPluginInitializer
{
    Logger logger = new("trbank");
    Form a1 = new Form();
    RegCommand reg = new();
    SqlHelper sqlHelper = new SqlHelper();
    //元信息
    public Dictionary<string, string> MetaData => new()
    {
        {"Auther","wanan"}
    };
    //介绍
    public string Introduction => "A bank for owoser!";
    //版本
    public Version Version => new(1, 0, 0);
    //载入
    public void OnInitialize()
    {
        reg.CommandRegisiter();
        a1.Form1();
        a1.From2();
        a1.Form3();
        
    }
}