using InvestmentApp.Models;
using System.Text.Json;

namespace InvestmentApp
{
    public static class SettingsReader
    {

        public static string GetAuthToken()
        {
            using(var fs = new FileStream("appsettings.json", FileMode.Open))
            {
                var text = JsonSerializer.Deserialize<SettingsModel>(fs);
                return text.AuthKey;
            }
           
        }
        public static string GetSandboxAddr()
        {
            using (var fs = new FileStream("appsettings.json", FileMode.Open))
            {
                var text = JsonSerializer.Deserialize<SettingsModel>(fs);
                return text.SandboxAddr;
            }
        }

        public static string GetProdAuthToken()
        {
            using (var fs = new FileStream("appsettings.json", FileMode.Open))
            {
                var text = JsonSerializer.Deserialize<SettingsModel>(fs);
                return text.ProdAuthKey;
            }
        }
        public static string GetProdAddr()
        {
            using (var fs = new FileStream("appsettings.json", FileMode.Open))
            {
                var text = JsonSerializer.Deserialize<SettingsModel>(fs);
                return text.ProdAddr;
            }
        }
    }
}
