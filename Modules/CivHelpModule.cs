using System.Threading.Tasks;
using Discord.Commands;
using bot.aoe2.civpicker.services;
using System.Linq;
using Discord;
using System.Net;
using System.IO;
using Discord.WebSocket;
using System.Collections.Generic;

namespace bot.aoe2.civpicker.Modules
{
    [Name("civ")]
    public class CivHelperModule : ModuleBase<SocketCommandContext>
    {
        private readonly AoeAPIService _aoeApi;
        private readonly AoeMatchUpService _aoeMatchUp;
        private IReadOnlyCollection<GuildEmote> Emotes;
        private const string TechTreeUrl = "https://ageofempires.fandom.com/wiki/{0}/Tree";
        private const string CivIconUrl = "https://tools.unfamiliarplace.com/aoe2civs/assets/crests/CivIcon-{0}.png";

        public CivHelperModule(AoeAPIService aoeApi, AoeMatchUpService aoeMatchUp) 
        {
            _aoeApi = aoeApi;
            _aoeMatchUp = aoeMatchUp;
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

       

        [Command("team"), Alias("t")]
        [Summary("Match up 4v4 team with Random civs")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.UseExternalEmojis)]
        public async Task TagCivEightPlayer(params SocketUser[] users)
        {
            

            var list = await _aoeMatchUp.PickCivs(users.Take(8).ToList());
            Emotes = Context.Client.Guilds.SelectMany(x => x.Emotes).ToList();
            
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

        private EmbedFieldBuilder CreateUserField(Player player)
        {
            var civIcon = Emotes?.Where(x => x.Name == player.Civilization).FirstOrDefault();
            return new EmbedFieldBuilder
            {
                Name = $"{player.UserMention} [{player?.Ratings?.Elo}] - {player.Team} - {player.PlayerColor}",
                Value = $"<:{civIcon?.Name}:{civIcon?.Id}> - [{player.Civilization}]({string.Format(TechTreeUrl, player.Civilization)})"
            };
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