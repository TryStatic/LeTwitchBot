using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;

namespace LeTwitchBot.Utilities
{
    internal static class Extensions
    {
        public static void SendHostChannelMessage(this TwitchClient client, string message, bool dryRun = false)
        {
            if (client.JoinedChannels.Count == 0)
            {
                client.JoinChannel(LeTwitchBot.HostChannelName);
            }

            client.SendMessage(LeTwitchBot.HostChannelName, message, dryRun);
        }
    }
}
