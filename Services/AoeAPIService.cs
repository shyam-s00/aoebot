using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using bot.aoe2.civpicker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using bot.aoe2.civpicker.Constants;
using System.Collections.Concurrent;

namespace bot.aoe2.civpicker.services
{
    public class AoeAPIService
    {
        private const string BASEURL = "https://age-of-empires-2-api.herokuapp.com/api/v1/";
        private const string Civilizations = "civilizations";

        private const string Aoe2 = "aoe2de";

        private readonly ConcurrentDictionary<string, string> playerMaps;
        
        public AoeAPIService() {
            playerMaps = new ConcurrentDictionary<string, string>();
            playerMaps.TryAdd("736274349287407706", "76561198088035394"); //maniac
            playerMaps.TryAdd("176022163491520513", "76561198032506144"); // glitch
            playerMaps.TryAdd("163305625051201536", "76561198207690565"); // firehawk
            playerMaps.TryAdd("128076865742176256", "76561198044613146"); // protox
            playerMaps.TryAdd("186369989371101194", "76561198057496453"); // kuroko
            playerMaps.TryAdd("693756257957576745", "76561198087325373"); // hawk
            playerMaps.TryAdd("692033052540403712", "76561198115672759"); // lezion
            playerMaps.TryAdd("685774329157648405", "76561198308551669"); // gunjack
            playerMaps.TryAdd("477862232571510827", "76561198823747771"); // retemp
            playerMaps.TryAdd("128184187663417345", "76561198159403850"); // kronos
            
         }

        public async Task<List<PlayerRating>> GetPlayerRatingsAsync(string playerId, int count = 1)
        {
            string steamId = "";
            playerMaps.TryGetValue(playerId, out steamId);
            if (!string.IsNullOrEmpty(steamId))
            {
                var url = new StringBuilder(AppConstants.Aoe2NetBaseUrl + AppConstants.PlayerRating);
                AddParam(url, AppConstants.Game, Aoe2);
                AddParam(url, AppConstants.LeaderBoardId, (int)Enumerations.LeaderBoard.Unranked);
                AddParam(url, AppConstants.SteamId, steamId);
                AddParam(url, AppConstants.Count, count);

                var playerRating = await Get<List<PlayerRating>>(url.ToString());

                return playerRating;
            }
            else
                return new List<PlayerRating>();
        }

        public async Task<List<Units>> GetAllUnitsAsync()
        {
            //var unitUrl = AppConstants.AoeHerokuBaseUrl + AppConstants.Units;
            var cwd = Directory.GetCurrentDirectory();
            var path = cwd + "/Data/units.json";
            var json = await File.ReadAllTextAsync(path);

            var units = JsonConvert.DeserializeObject<List<Units>>(json);

            //var units = await Get<List<Units>>(unitUrl, AppConstants.Units);
            return units;
        }

        public async Task<List<Civlization>> GetCivilizationsAsync()
        {
            //var civUrl = BASEURL + Civilizations;            
            //var civs = await Get<List<Civlization>>(civUrl, Civilizations);
            var cwd = Directory.GetCurrentDirectory();
            var path = cwd + "/Data/civs.json";
            var json = await File.ReadAllTextAsync(path);

            var civs = JsonConvert.DeserializeObject<List<Civlization>>(json);

            return civs;
        }

        private async Task<T> Get<T>(string url, string key="") where T : class
        {
            try {
                var request = WebRequest.Create(url) as HttpWebRequest;
                var response = await request.GetResponseAsync() as HttpWebResponse;

                if (response?.StatusCode != HttpStatusCode.OK)
                {
                    throw new System.Exception($"Server error, code {response.StatusCode}");
                }

                var json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (string.IsNullOrEmpty(key))
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<JObject>(json);
                    var jsonValue =  obj.GetValue(key)?.ToString();
                    return JsonConvert.DeserializeObject<T>(jsonValue);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unknown error {ex.StackTrace}");
            }

            return default(T);
        }

        private StringBuilder AddParam(StringBuilder URI, string paramName, object paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue?.ToString()))
            {
                if (URI.ToString().Contains("?"))
                    return URI.Append("&" + paramName + $"={paramValue}");
                else
                    return URI.Append("?" + paramName + $"={paramValue}");
            }
            return URI;
        }
    }
}