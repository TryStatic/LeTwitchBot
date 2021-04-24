using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using LeTwitchBot.Data;
using LeTwitchBot.Data.Models;
using Newtonsoft.Json;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace LeTwitchBot.Handlers
{
    internal class CurrencyHandler
    {
        private readonly Timer _timer;

        private const int CurrencyPerIteration = 10;
        private const int CurrencyForNewUsers = 50;
        private const int IterationInterval = 10000;

        public CurrencyHandler()
        {
            _timer = new Timer();
            _timer.Elapsed += OnTick;
            _timer.Interval = IterationInterval;
            _timer.Enabled = true;
        }

        public static int? GetUserCurrency(string username)
        {
            using Storage storage = new Storage();
            ChannelVisitor visitor = storage.Visitors.FirstOrDefault(u => username.ToLower() == u.TwitchUsername.ToLower());
            return visitor?.Currency;
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            try
            {
                List<ChatterFormatted> response =
                    await LeTwitchBot.BotAPI.Undocumented.GetChattersAsync(LeTwitchBot.HostChannelName);

                List<string> usernames = response.Select(s => s.Username).ToList();

                GetUsersResponse apiResponse =
                    await LeTwitchBot.BotAPI.Helix.Users.GetUsersAsync(null, usernames,
                        LeTwitchBot.Secrets.HostChannelUserToken);

                if (apiResponse.Users.Length == 0)
                {
                    return;
                }

                foreach (User u in apiResponse.Users)
                {
                    await using Storage storage = new Storage();
                    int userid = int.Parse(u.Id);
                    ChannelVisitor existingVisitor = storage.Visitors.FirstOrDefault(v => v.TwitchID == userid);

                    if (existingVisitor != null)
                    {
                        existingVisitor.Currency += CurrencyPerIteration;
                    }
                    else
                    {
                        ChannelVisitor newVisitor = new ChannelVisitor(userid, u.DisplayName);
                        newVisitor.Currency = CurrencyForNewUsers;
                        await storage.Visitors.AddAsync(newVisitor);
                    }

                    await storage.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                // ignored
            }

        }
    }
}