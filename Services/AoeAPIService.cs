using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using bot.aoe2.civpicker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bot.aoe2.civpicker.services
{
    public class AoeAPIService
    {
        private const string BASEURL = "https://age-of-empires-2-api.herokuapp.com/api/v1/";
        private const string Civilizations = "civilizations";
        public AoeAPIService() { }
        

        public async Task<List<Civlization>> GetCivilizationsAsync()
        {
            var civUrl = BASEURL + Civilizations;
            
            var civs = await Get<List<Civlization>>(civUrl, Civilizations);
            return civs;
        }

        private async Task<T> Get<T>(string url, string key) where T : class
        {
            try {
                var request = WebRequest.Create(url) as HttpWebRequest;
                var response = await request.GetResponseAsync() as HttpWebResponse;

                if (response?.StatusCode != HttpStatusCode.OK)
                {
                    throw new System.Exception($"Server error, code {response.StatusCode}");
                }

                var json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var obj = JsonConvert.DeserializeObject<JObject>(json);
                var jsonValue =  obj.GetValue(key)?.ToString();
                //var test = obj as KeyValuePair<string, dynamic>;
                
                return JsonConvert.DeserializeObject<T>(jsonValue);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unknown error {ex.StackTrace}");
            }

            return default(T);
        }
    }
}