using System;
using System.Threading.Tasks;
using LeTwitchBot.Handlers;
using LeTwitchBot.Utilities;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace LeTwitchBot
{
    internal class TwitchBot
    {
        public static ConnectionCredentials Credentials;
        public static TwitchClient Client;
        public static Secrets Secrets;
        public static TwitchAPI API;

        public static CommandsHandler CommandsHandler;
        public static VisitorTracker VisitorTracker;



        public static async Task Init(Secrets secrets)
        {
            Secrets = secrets;
            Credentials = new ConnectionCredentials(secrets.HostChannelName, secrets.BotTMIOAuthKey);
            Client = new TwitchClient();

            Client.Initialize(Credentials, secrets.HostChannelName);
            Client.Connect();


            API = new TwitchAPI();
            API.Settings.ClientId = Secrets.BotClientID;
            API.Settings.Secret = secrets.BotClientSecret;

            if (!Client.IsConnected)
            {
                Console.WriteLine("Something went wrong. Client is not connected!");
                return;
            }

            CommandsHandler = new CommandsHandler(Client, API);
            VisitorTracker = new VisitorTracker(Client, API);

            Client.OnLog += OnLog;

        }
        private static void OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }

    }


}