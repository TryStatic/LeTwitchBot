using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace LeTwitchBot.Handlers
{
    internal class CommandsHandler
    {
        private readonly TwitchClient _client;

        public CommandsHandler(TwitchClient client)
        {
            _client = client;
        }

        public void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {

            switch (e.Command.CommandText.ToLower())
            {
                case "dice":
                    HandleDiceRoll(sender, e);
                    break;
                case "sound":
                    HandlePlaySound(sender, e);
                    break;
            }
        }


        private void HandleDiceRoll(object sender, OnChatCommandReceivedArgs onChatCommandReceivedArgs)
        {
            if (!(sender is TwitchClient senderClient)) return;
            _client.SendMessage(_client.JoinedChannels.First(), $"Hey {senderClient.TwitchUsername}! You rolled {new Random().Next(1, 7)}!");
        }

        private void HandlePlaySound(object sender, OnChatCommandReceivedArgs e)
        {
            if (!(sender is TwitchClient senderClient)) return;

            List<string> effects = new List<string>();
            string[] files = Directory.GetFiles("Soundeffects");
            foreach (string file in files)
            {
                string effectName = Path.GetFileName(file).Replace(".wav", "");
                effects.Add(effectName);
            }

            if (e.Command.ArgumentsAsList.Count == 0)
            {
                _client.SendMessage(_client.JoinedChannels.First().Channel, $"!sound [name] (names: {string.Join(' ', effects)})");
            }
            else if (effects.Contains(e.Command.ArgumentsAsList[0]))
            {
                int index = effects.FindIndex(0, p => p == e.Command.ArgumentsAsList[0]);
                using SoundPlayer player = new SoundPlayer($"soundeffects//{effects[index]}.wav");
                player.Play();
            }
            else
            {
                _client.SendMessage(_client.JoinedChannels.First().Channel, $"Can't find that effect NotLikeThis NotLikeThis NotLikeThis @{senderClient.TwitchUsername}");
            }
        }
    }
}