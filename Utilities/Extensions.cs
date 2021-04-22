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

            try
            {
                client.SendMessage(LeTwitchBot.HostChannelName, message, dryRun);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Error, bot couldn't send the message.");
            }
        }
    }
}
