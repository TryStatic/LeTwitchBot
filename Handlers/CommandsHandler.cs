using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using LeTwitchBot.Utilities;
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
                case "title":
                    HandleChangeTitleCmd(sender, e);
                    break;
            }
        }

        private void HandleChangeTitleCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            string newTitle = e.Command.ArgumentsAsString;
            Console.WriteLine(newTitle);

            if(newTitle == null || newTitle.Length <= 0) return;

            // TODO
        }


        private void HandleDiceRollCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;
            LeTwitchBot.TwitchClient.SendHostChannelMessage($"Hey {senderClient.TwitchUsername}! You rolled {_random.Next(1, 7)}!");
        }

        private void HandlePlaySoundCmd(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

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
            }
            else if (effects.Contains(e.Command.ArgumentsAsList[0]))
            {
                int index = effects.FindIndex(0, p => p == e.Command.ArgumentsAsList[0]);
                using SoundPlayer player = new SoundPlayer(@"Assets\Soundeffects\" + $"{effects[index]}.wav");
                player.Play();
            }
            else
            {
                LeTwitchBot.TwitchClient.SendHostChannelMessage($"Can't find that effect NotLikeThis NotLikeThis NotLikeThis @{senderClient.TwitchUsername}");
            }
        }
    }
}