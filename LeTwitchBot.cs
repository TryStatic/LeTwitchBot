using System;
using LeTwitchBot.Handlers;
using LeTwitchBot.Utilities;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace LeTwitchBot
{
    internal class LeTwitchBot
    {
        internal static ConnectionCredentials TMICredentials { get; private set; }
        internal static TwitchClient TwitchClient { get; private set; }
        internal static Secrets Secrets { get; private set; }
        internal static TwitchAPI BotAPI { get; private set; }
        internal static TwitchAPI HostAPI { get; private set; }
        internal static string HostChannelName { get; private set; }

        internal static CommandsHandler CommandsHandler;
        internal static VisitorTracker VisitorTracker;


        public static void InitializeBot(Secrets secrets)
        {
            Secrets = secrets;

            HostChannelName = secrets.HostChannelName;
            TMICredentials = new ConnectionCredentials(secrets.HostChannelName, secrets.BotTMIOAuthKey);


            TwitchClient = new TwitchClient();
            
            TwitchClient.Initialize(TMICredentials, secrets.HostChannelName);
            if (!TwitchClient.IsInitialized)
            {
                Console.WriteLine("Twitch Client did not initialize.");
                return;
            }

            TwitchClient.Connect();
            if (!TwitchClient.IsConnected)
            {
                Console.WriteLine("Twitch Client was not able to connect.");
                return;
            }

            TwitchClient.JoinChannel(secrets.HostChannelName);

            BotAPI = new TwitchAPI();
            BotAPI.Settings.ClientId = Secrets.BotClientID;
            BotAPI.Settings.Secret = secrets.BotClientSecret;

            HostAPI = new TwitchAPI();
            HostAPI.Settings.ClientId = Secrets.HostClientID;
            HostAPI.Settings.Secret = secrets.HostClientSecret;


            CommandsHandler = new CommandsHandler();
            VisitorTracker = new VisitorTracker();

            TwitchClient.OnLog += OnLog;
        }

        private static void OnLog(object sender, OnLogArgs e) => Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
    }


}