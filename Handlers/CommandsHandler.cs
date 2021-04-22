using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using LeTwitchBot.Utilities;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace LeTwitchBot.Handlers
{
    internal class CommandsHandler
    {
        private readonly Random _random = new Random();

        public CommandsHandler()
        {
            LeTwitchBot.TwitchClient.OnChatCommandReceived += OnChatCommandReceived;
        }

        public void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            switch (e.Command.CommandText.ToLower())
            {
                case "dice":
                    HandleDiceRollCmd(sender, e);
                    break;
                case "sound":
                    HandlePlaySoundCmd(sender, e);
                    break;
                case "settitle":
                    HandleChangeTitleCmd(sender, e);
                    break;
                case "setgame":
                    HandleSetGame(sender, e);
                    break;
                case "followtime":
                    HandleFollowTime(sender, e);
                    break;
            }
        }

        private async void HandleSetGame(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            string newGame = e.Command.ArgumentsAsString;
            Console.WriteLine(newGame);

            if (newGame == null || newGame.Length <= 0) return;

            GetUsersResponse resp = await LeTwitchBot.BotAPI.Helix.Users.GetUsersAsync(null, new List<string> { senderClient.TwitchUsername });
            if (resp.Users.Length <= 0) return;
            User user = resp.Users[0];

            if (!user.DisplayName.ToLower().Equals(LeTwitchBot.HostChannelName.ToLower()))
            {
                return;
            }

            GetChannelInformationResponse existingInfo = await LeTwitchBot.BotAPI.Helix.Channels.GetChannelInformationAsync(user.Id);
            if (existingInfo.Data.Length <= 0) return;
            ChannelInformation info = existingInfo.Data[0];

            GetGamesResponse gameResponse = await LeTwitchBot.BotAPI.Helix.Games.GetGamesAsync(null, new List<string> {newGame});
            if (gameResponse.Games.Length <= 0)
            {
                LeTwitchBot.TwitchClient.SendHostChannelMessage($"Couldn't locate {newGame} :( :( :( :( ");
                return;
            }

            Game tehGame = gameResponse.Games[0];


            ModifyChannelInformationRequest changeReq = new ModifyChannelInformationRequest();
            changeReq.BroadcasterLanguage = info.BroadcasterLanguage;
            changeReq.Title = info.Title;
            changeReq.GameId = tehGame.Id;

            await LeTwitchBot.BotAPI.Helix.Channels.ModifyChannelInformationAsync(user.Id, changeReq, LeTwitchBot.Secrets.HostChannelUserToken);


            LeTwitchBot.TwitchClient.SendHostChannelMessage($"Broadcaster {user.DisplayName} will now be playin' {tehGame.Name} Kappa Kappa Kappa Kappa");
        }

        private void HandleFollowTime(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;


        }

        private async void HandleChangeTitleCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            string newTitle = e.Command.ArgumentsAsString;
            Console.WriteLine(newTitle);

            if(newTitle == null || newTitle.Length <= 0) return;

            GetUsersResponse resp = await LeTwitchBot.BotAPI.Helix.Users.GetUsersAsync(null, new List<string> {senderClient.TwitchUsername});
            if(resp.Users.Length <= 0) return;
            User user = resp.Users[0];

            if (!user.DisplayName.ToLower().Equals(LeTwitchBot.HostChannelName.ToLower()))
            {
                return;
            }

            GetChannelInformationResponse existingInfo = await LeTwitchBot.BotAPI.Helix.Channels.GetChannelInformationAsync(user.Id);
            if(existingInfo.Data.Length <= 0) return;
            ChannelInformation info = existingInfo.Data[0];


            ModifyChannelInformationRequest changeReq = new ModifyChannelInformationRequest();
            changeReq.BroadcasterLanguage = info.BroadcasterLanguage;
            changeReq.Title = newTitle;
            changeReq.GameId = info.GameId;

            await LeTwitchBot.BotAPI.Helix.Channels.ModifyChannelInformationAsync(user.Id, changeReq, LeTwitchBot.Secrets.HostChannelUserToken);


            LeTwitchBot.TwitchClient.SendHostChannelMessage($"Broadcaster {user.DisplayName} has changed the title of the stream to '{newTitle}' CoolCat CoolCat CoolCat CoolCat");
        }


        private void HandleDiceRollCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;
            LeTwitchBot.TwitchClient.SendHostChannelMessage($"Hey {senderClient.TwitchUsername}! You rolled {_random.Next(1, 7)}!");
        }

        private static DateTime _lastSoundInvoke = DateTime.MinValue;
        private static bool notifySent = false;
        private static bool soundsEnabled = false;
        private void HandlePlaySoundCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            bool isHost = senderClient.TwitchUsername.ToLower().Equals(LeTwitchBot.HostChannelName.ToLower());

            if (!soundsEnabled && !isHost) return;

            List<string> effects = new List<string>();
            string[] files = Directory.GetFiles(@"Assets\Soundeffects");
            foreach (string file in files)
            {
                string effectName = Path.GetFileName(file).Replace(".wav", "");
                effects.Add(effectName);
            }

            if (e.Command.ArgumentsAsList.Count == 0)
            {
                LeTwitchBot.TwitchClient.SendHostChannelMessage($"!sound [name] (names: {string.Join(' ', effects)})");
                return;
            }

            if (e.Command.ArgumentsAsList[0].Equals("enable", StringComparison.InvariantCultureIgnoreCase))
            {
                soundsEnabled = true;
                LeTwitchBot.TwitchClient.SendHostChannelMessage($"{senderClient.TwitchUsername} has enabled !sound for everyone >( >( >( >(");
                return;
            }

            if (e.Command.ArgumentsAsList[0].Equals("disable", StringComparison.InvariantCultureIgnoreCase))
            {
                soundsEnabled = false;
                LeTwitchBot.TwitchClient.SendHostChannelMessage($"{senderClient.TwitchUsername} has disabled !sound  :\\ :\\ :\\ :\\ ");
                return;
            }


            if (!effects.Contains(e.Command.ArgumentsAsList[0]))
            {
                LeTwitchBot.TwitchClient.SendHostChannelMessage(
                    $"Can't find that effect NotLikeThis NotLikeThis NotLikeThis @{senderClient.TwitchUsername}");
                return;
            }

            if (DateTime.Now.Subtract(_lastSoundInvoke).TotalSeconds < 10 && !isHost)
            {
                if (!notifySent)
                {
                    LeTwitchBot.TwitchClient.SendHostChannelMessage($"{senderClient.TwitchUsername} No spam on teh sounds plz ok?! cmonBruh cmonBruh cmonBruh cmonBruh cmonBruh");
                }

                notifySent = true;
                return;
            }

            _lastSoundInvoke = DateTime.Now;
            notifySent = false;


            int index = effects.FindIndex(0, p => p == e.Command.ArgumentsAsList[0]);
            using SoundPlayer player = new SoundPlayer(@"Assets\Soundeffects\" + $"{effects[index]}.wav");
            player.Play();
        }
    }
}