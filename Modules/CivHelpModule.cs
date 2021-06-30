using System.Threading.Tasks;
using Discord.Commands;
using bot.aoe2.civpicker.services;
using System.Linq;
using Discord;
using System.Net;
using System.IO;
using Discord.WebSocket;
using System.Collections.Generic;
using bot.aoe2.civpicker.Models;

namespace bot.aoe2.civpicker.Modules
{
    [Name("civ")]
    public class CivHelperModule : ModuleBase<SocketCommandContext>
    {
        private readonly AoeAPIService _aoeApi;
        private readonly AoeMatchUpService _aoeMatchUp;
        private IReadOnlyCollection<GuildEmote> _emotes;
        private IReadOnlyCollection<Units> _units;
        private const string TechTreeUrl = "https://ageofempires.fandom.com/wiki/{0}/Tree";
        private const string CivIconUrl = "https://tools.unfamiliarplace.com/aoe2civs/assets/crests/CivIcon-{0}.png";

        public CivHelperModule(AoeAPIService aoeApi, AoeMatchUpService aoeMatchUp) 
        {
            _aoeApi = aoeApi;
            _aoeMatchUp = aoeMatchUp;
            
            _units = _units ?? _aoeApi.GetAllUnitsAsync().GetAwaiter().GetResult();
        }

        [Command("list")]
        [Summary("List all the available Civilizations in AOE2")]        
        public async Task ListAllCivs()
        {
            var response = await _aoeApi.GetCivilizationsAsync();
            var civList = response?.Select(x => $"{x.Id} --> {x.Name}").ToList();

            var message = string.Join("\r\n", civList);

            await ReplyAsync(message);
        }

        [Command("unit"), Alias("u")]
        [Summary("Get a unit details")]
        public async Task ViewUnit(string unit)
        {
            var filteredUnit = _units.Where(x => x.Id.ToString() == unit || x.Name.Contains(unit, System.StringComparison.OrdinalIgnoreCase));
            if (filteredUnit != null && filteredUnit.Any())
            {
                if (filteredUnit.Count() > 3)
                {
                    await ReplyAsync(embed: TooManyUnits(filteredUnit));
                }
                else
                {
                    foreach (var u in filteredUnit)
                    {
                        var embed = CreateUnitEmbed(u);
                        await ReplyAsync(embed: embed);
                    }
                }
            }
            else 
            {
                await ReplyAsync("Unit not found. Search with id (1 - 104) or with name");
            }
        }

        [Command("team"), Alias("t")]
        [Summary("Match up 4v4 team with Random civs")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.UseExternalEmojis)]
        public async Task TagCivEightPlayer(params SocketUser[] users)
        {
            var list = await _aoeMatchUp.PickCivs(users.Where(x => !x.IsBot).Take(8).ToList());
            _emotes = Context.Client.Guilds.SelectMany(x => x.Emotes).ToList();
            
            var embed = CreateUserEmbed(list);
            await ReplyAsync($"{string.Join(",", users.Select(x => x.Mention))}", isTTS: false, embed: embed);
        }

        private Embed CreateUserEmbed(List<Player> players)
        {
            var fields = players.Select(CreateUserField).ToList();
            return new EmbedBuilder{
                Title = "Team Match",
                ThumbnailUrl = "https://static.wikia.nocookie.net/ageofempires/images/3/3f/Age2de-library-boxart.jpg",
                Fields = fields,
                Color = Color.Red,
            }.Build();
        }

        private Embed TooManyUnits(IEnumerable<Units> units)
        {
            return new EmbedBuilder
            {
                Title = "Too Many Units...",
                Description = string.Join("\r\n", units.Select(x=> $"Id: {x.Id} -> Name: {x.Name}"))
            }.Build();
        }

        private Embed CreateUnitEmbed(Units unit)
        {
            // var fields = new List<EmbedFieldBuilder> {
            //         CreateUnitField("Attack", unit.Attack.ToString()),
            //         CreateUnitField("HitPoints", unit.HitPoints.ToString()),
            //         CreateUnitField("Armor", unit.Armor.ToString()),
            //         CreateUnitField("Attack Bonous", unit.AttackBonus != null ? string.Join("\r\n", unit.AttackBonus) : "NA")
            //     };
            var attackBonus = unit.AttackBonus != null ? CreateUnOrderList(unit.AttackBonus) : "NA";
            return new EmbedBuilder
            {
                Title = unit.Name,
                Description = $":arrow_right: Id: {unit.Id} \r\n :arrow_right: Attack: {unit.Attack} \r\n :arrow_right: Hit Points: {unit.HitPoints} \r\n :arrow_right: Armor: {unit.Armor} \r\n :arrow_right: Attack Bonus: \n{attackBonus}",
                Color = Color.Green,
                Footer = new EmbedFooterBuilder().WithText($"{unit.Cost.ToString()}")
            }.Build();
        }

        private string CreateUnOrderList(List<string> items)
        {
            var li = string.Join("\n", items?.Select(x => $"      -> {x}"));
            return @$"{li}";
        }
        private EmbedFieldBuilder CreateUnitField(string fieldName, string fieldValue)
        {
            return new EmbedFieldBuilder().WithName(fieldName).WithValue($":arrow_right: {fieldName}: {fieldValue}");
        }

        private EmbedFieldBuilder CreateUserField(Player player)
        {
            var civIcon = _emotes?.Where(x => x.Name == player.Civilization).FirstOrDefault();
            return new EmbedFieldBuilder
            {
                Name = $"{player.UserMention} [{player?.Ratings?.Elo.ToString() ?? "NA"}] - {player.Team} - {player.PlayerColor}",
                Value = $"<:{civIcon?.Name}:{civIcon?.Id}> - [{player.Civilization}]({GetCivUrl(player.Civilization)})"
            };
        }

        private string GetCivUrl(string civName)
        {
            const string brokenUrl = "_(Age_of_Empires_II)";
            string[] brokenCivs = new string[] {"Chinese", "Japanese", "Persians", "Aztecs", "Spanish", "Incas", "Indians", "Portuguese"};
            if (brokenCivs.Any(x => x == civName))
                return string.Format(TechTreeUrl, civName + brokenUrl);
            else 
                return string.Format(TechTreeUrl, civName);
        }

        private Embed CreateUserEmbed(string civName, string team, Color playerColor)
        {
            return new EmbedBuilder{
                Title = civName,
                Url = string.Format(TechTreeUrl, civName),
                Color = playerColor,
                Footer = new EmbedFooterBuilder {
                    Text = team,
                    IconUrl = string.Format(CivIconUrl, civName)
                }
            }.Build();
        }

        private Image GetFromUrl(string url)
        {
            var client = new WebClient();
            var imagearr = client.DownloadData(url);

            return new Image (new MemoryStream(imagearr));

        }
    }
}