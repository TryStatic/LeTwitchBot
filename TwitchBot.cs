using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeTwitchBot.Data;
using LeTwitchBot.Handlers;
using LeTwitchBot.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
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
        public static CommandsHandler CommandsHandler;
        public static TwitchAPI API;

        public static async Task Init(Secrets secrets)
        {
            Secrets = secrets;
            Credentials = new ConnectionCredentials(secrets.ChannelName, secrets.Oauth);
            Client = new TwitchClient();

            Client.Initialize(Credentials, secrets.ChannelName);
            Client.Connect();


            API = new TwitchAPI();
            API.Settings.ClientId = Secrets.ClientID;
            API.Settings.Secret = secrets.ClientSecret;

            if (!Client.IsConnected)
            {
                Console.WriteLine("Something went wrong. Client is not connected!");
                return;
            }

            CommandsHandler = new CommandsHandler(Client);

            Client.OnChatCommandReceived += CommandsHandler.OnChatCommandReceived;
            Client.OnLog += OnLog;
            Client.OnUserJoined += OnUserJoined;
            Client.OnUserLeft += OnUserLeft;
        }

        private static void OnUserLeft(object? sender, OnUserLeftArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            Client.SendMessage(Client.JoinedChannels.First(), $"{senderClient.TwitchUsername} left BibleThump BibleThump BibleThump !!");
        }

        private static async void OnUserJoined(object? sender, OnUserJoinedArgs e)
        {
            if(!(sender is TwitchClient senderClient)) return;

            GetUsersResponse resp = await API.Helix.Users.GetUsersAsync(null, new List<string> {senderClient.TwitchUsername});
            if(resp.Users.Length <= 0) return;

            User user = resp.Users[0];


            int userid = int.Parse(user.Id);

            ChannelVisitor existingVisitor;
            await using (Storage storage = new Storage())
            {
                existingVisitor = storage.Visitors.FirstOrDefault(u => u.TwitchID == userid);

                if (existingVisitor == null)
                {

                    Visit newVisit = new Visit { VisitDate = DateTime.Now };

                    ChannelVisitor newVisitor = new ChannelVisitor
                    {
                        TwitchUsername = user.DisplayName, TwitchID = userid, Visits = new List<Visit> {newVisit},
                    };

                    storage.Attach(newVisitor);
                }
                else
                {
                    Visit newVisit = new Visit
                    {
                        Visitor = existingVisitor, 
                        VisitDate = DateTime.Now
                    };

                    storage.Add(newVisit);
                }

                await storage.SaveChangesAsync();
            }

            if (existingVisitor == null)
            {
                Client.SendMessage(Client.JoinedChannels.First(), $"{senderClient.TwitchUsername} joined the stream for the first time. Welcome!!");
            }
        }

        private static void OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }

    }
}