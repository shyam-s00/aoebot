using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bot.aoe2.civpicker.Extensions;
using bot.aoe2.civpicker.Models;
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

        private List<PlayerRating> ratings;

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
            var teamSize = Math.Ceiling(users.Count / 2d);
            var response = await _aoeApi.GetCivilizationsAsync();
            // var playerRatings = await users.Select(x => x.Id.ToString()).Distinct().SelectManyAsync(x =>  _aoeApi.GetPlayerRatingsAsync(x));
            // ratings = playerRatings.ToList();

            PlayerColors = new Stack<string>(TeamColors1.Shuffle());
            var civList = response?.Select(x =>x.Name).Append(Burgundians).Append(Sicilians).ToList();     
            return users.Select((x,y) => CreatePlayer(civList.PickRandom(), y, x.Username, teamSize, x.Id.ToString())).ToList();

        }

        private Player CreatePlayer(string civ, int playerPos, string user, double teamSize, string playerId)
        {
            var playerRating = _aoeApi.GetPlayerRatingsAsync(playerId).GetAwaiter().GetResult();
            return new Player {
                Civilization = civ,
                Team = playerPos >= teamSize ? Team2 : Team1,
                UserMention = user, 
                PlayerColor = PlayerColors.Pop(),
                Ratings = playerRating?.FirstOrDefault()
            };
        }
    }

    public class Player
    {
        public string Civilization { get; set; }

        public string Team { get;  set; }

        public string UserMention {get; set;}

        public string PlayerColor { get; set; }

        public PlayerRating Ratings {get; set;}
    }
}