using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LeTwitchBot.Utilities;
using Newtonsoft.Json;

namespace LeTwitchBot.Dep
{
    internal class TokenInfo
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }

        private TokenInfo(string accessToken, string tokenType, int expiresIn)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
        }

        public static TokenInfo GetTokenInfo(string jsonResponse)
        {
            if (jsonResponse == null || string.IsNullOrEmpty(jsonResponse))
            {
                throw new Exception("GetTokenInfo was given an empty or null string.");
            }

            dynamic data = JsonConvert.DeserializeObject(jsonResponse);
            if (data == null)
            {
                throw new Exception("GetTokenInfo couldn't deserialize response.");
            }

            return new TokenInfo((string)data.access_token, (string)data.token_type, (int)data.expires_in);
        }

        public static async Task test()
        {

            // POST https://id.twitch.tv/oauth2/token?client_id=uo6dggojyb8d6soh92zknwmi5ej1q2&client_secret=nyo51xcdrerl8z9m56w9w6wg&grant_type=client_credentials

            HttpClient client = new HttpClient();

            dynamic secrets = await Secrets.Get();

            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "client_id", secrets.ClientID },
                { "client_secret", secrets.ClientSecret },
                { "grant_type", "client_credentials" },
                { "scope", "channel:moderate chat:edit chat:read whispers:read whispers:edit channel:manage:broadcast moderation:read"}
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("https://id.twitch.tv/oauth2/token", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            dynamic dynResponse = JsonConvert.DeserializeObject(responseString);
            if (dynResponse == null)
            {
                Console.WriteLine("Error getting the response.");
                return;
            }

            string token = dynResponse.access_token;


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Client-Id", "");



            HttpResponseMessage res = await client.GetAsync("https://api.twitch.tv/helix/users?login=lestatiquebot");
            Console.WriteLine(await res.Content.ReadAsStringAsync());


        }
    }

}