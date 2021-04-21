using System;
using System.Collections.Generic;
using System.Linq;
using LeTwitchBot.Data;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace LeTwitchBot.Handlers
{
    internal class VisitorTracker
    {
        public TwitchAPI API { get; set; }

        private TwitchClient Client { get; set; }

        public VisitorTracker(TwitchClient client, TwitchAPI api)
        {
            API = api;
            Client = client;


            client.OnUserJoined += OnUserJoined;
            client.OnUserLeft += OnUserLeft;
        }


        private void OnUserLeft(object sender, OnUserLeftArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            Client.SendMessage(Client.JoinedChannels.First(), $"{senderClient.TwitchUsername} left BibleThump BibleThump BibleThump !!");
        }

        private async void OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            GetUsersResponse resp = await API.Helix.Users.GetUsersAsync(null, new List<string> { senderClient.TwitchUsername });
            if (resp.Users.Length <= 0) return;

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
                        TwitchUsername = user.DisplayName,
                        TwitchID = userid,
                        Visits = new List<Visit> { newVisit },
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

    }
}