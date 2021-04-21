using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeTwitchBot.Utilities
{
    internal class Secrets
    {
        private static Secrets _instance;

        public string BotClientID { get; set; }
        public string BotClientSecret { get; set; }
        public string BotApplicationRedirectUrl { get; set; }
        public string BotTMIOAuthKey { get; set; }

        public string HostChannelName { get; set; }
        public string HostClientID { get; set; }
        public string HostClientSecret { get; set; }
        public string HostApplicationRedirectUrl { get; set; }

        private Secrets()
        {
            
        }

        public static async Task<Secrets> Get()
        {

            if (_instance != null) return _instance;

            string jsonString = await File.ReadAllTextAsync("secrets.json", Encoding.UTF8);
            if (jsonString == null || string.IsNullOrEmpty(jsonString))
            {
                throw new Exception("Error reading secrets.");
            }

            Secrets secrets = JsonConvert.DeserializeObject<Secrets>(jsonString);
            _instance = secrets ?? throw new Exception("Error reading secrets.");
            return secrets;
        }
    }
}