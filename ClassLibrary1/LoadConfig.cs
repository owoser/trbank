using LitJson;
using System.Text.RegularExpressions;

namespace ClassLibrary1
{
    internal class LoadConfig
    {
        private JsonData configData = "";

        public LoadConfig()
        {
            loadFile();
        }

        private void loadFile()
        {
            configData = JsonMapper.ToObject(File.ReadAllText("plugins/trbank/config.json"));
        }

        public dynamic get(params dynamic[] configKey)
        {
            JsonData temp = configData;

            foreach (var key in configKey) temp = temp[key];

            string str = Regex.Unescape(JsonMapper.ToJson(temp));

            if(!int.TryParse(str, out int a))
            {
                str = str.Substring(1, str.Length - 1);
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }
    }
}
