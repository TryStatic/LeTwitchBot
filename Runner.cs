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

            // await TokenInfo.test();


            await using (Storage storage = new Storage())
            {
                await storage.Database.MigrateAsync();
            }




            await TwitchBot.Init(secrets);


            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
