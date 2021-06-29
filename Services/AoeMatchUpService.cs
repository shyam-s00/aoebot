using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bot.aoe2.civpicker.Extensions;
using Discord;
using Discord.WebSocket;

namespace bot.aoe2.civpicker.services
{
    public class AoeMatchUpService
    {
        private readonly AoeAPIService _aoeApi;

        private const string Team1 = "TEAM 1";
        private const string Team2 = "TEAM 2";

        private const string Burgundians = "Burgundians";
        private const string Sicilians = "Sicilians";
        private static Random random = new Random();

        private Stack<string> PlayerColors;

        private readonly Color[] TeamColors = new [] {
            Color.DarkBlue,
            Color.Red,
            Color.Green,
            Color.LightOrange,
            Color.Teal,
            Color.Purple,
            Color.LightGrey,
            Color.DarkOrange
        };

        private readonly string[] TeamColors1 = new[] {
            "BLUE",
            "RED",
            "GREEN",
            "YELLOW",
            "TEAL",
            "PINK",
            "GREY",
            "ORANGE"
        };

        public AoeMatchUpService(AoeAPIService aoeAPI)
        {
            _aoeApi = aoeAPI;
        }

        public async Task<List<Player>> PickCivs(IList<SocketUser> users)
        {
            var response = await _aoeApi.GetCivilizationsAsync();
            PlayerColors = new Stack<string>(TeamColors1.Shuffle());
            var civList = response?.Select(x =>x.Name).Append(Burgundians).Append(Sicilians).ToList();     
            return users.Select((x,y) => CreatePlayer(civList.PickRandom(), y, x.Username)).ToList();

        }

        private Player CreatePlayer(string civ, int team, string user)
        {
            return new Player {
                Civilization = civ,
                Team = team > 3 ? Team2 : Team1,
                UserMention = user, 
                PlayerColor = PlayerColors.Pop()
            };
        }
    }

    public class Player
    {
        public string Civilization { get; set; }

        public string Team { get;  set; }

        public string UserMention {get; set;}

        public string PlayerColor { get; set; }
    }
}