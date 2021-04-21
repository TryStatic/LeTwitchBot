using System;
using System.Threading.Tasks;
using LeTwitchBot.Data;
using LeTwitchBot.Utilities;
using Microsoft.EntityFrameworkCore;

namespace LeTwitchBot
{
    internal class Runner
    {
        internal static async Task Main(string[] args)
        {
            // Get secrets
            Secrets secrets;
            try
            {
                secrets = await Secrets.Get();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }

            // Migrate database
            await using (Storage storage = new Storage())
            {
                await storage.Database.MigrateAsync();
            }

            // Init bot
            LeTwitchBot.InitializeBot(secrets);


            // Keep console app running. Can do console commands here later too.
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
