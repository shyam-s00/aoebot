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

        [Command("team4"), Alias("t4")]
        [Summary("Match up 4v4 team with Random civs")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.UseExternalEmojis)]
        public async Task TagACiv(params SocketUser[] users)
        {
            var list = await _aoeMatchUp.PickCivs(users.Take(8).ToList());
            var emotes = await Context.Guild.GetEmotesAsync(RequestOptions.Default);
            var embed = CreateUserEmbed(list);
            //list.ForEach(async x => await ReplyAsync(x.UserMention, embed: CreateUserEmbed(x.Civilization, x.Team.ToString(), x.PlayerColor)));

            // var test = new EmbedBuilder {

            //     //Description = "Britons",
            //     Title = "Britons",
            //     Url = "https://ageofempires.fandom.com/wiki/Britons/Tree",
            //     //ImageUrl = "https://discordapp.com/assets/e4923594e694a21542a489471ecffa50.svg",
            //     //Fields = new[] { fields }.ToList(),
            //     // Fields = new System.Collections.Generic.List<EmbedFieldBuilder> {
            //     //     new EmbedFieldBuilder()
            //     //         .WithName(users[0].ToString())
            //     //         .WithValue("[Britons](https://ageofempires.fandom.com/wiki/Britons/Tree)")
            //     // },
            //     Color = Color.Red,
            //     Footer = new EmbedFooterBuilder {
            //         Text = "Team 2",
            //         IconUrl = "https://tools.unfamiliarplace.com/aoe2civs/assets/crests/CivIcon-Burmese.png"
            //     },
            // }.Build();           

            await ReplyAsync($"{string.Join(",", users.Select(x => x.Mention))}", isTTS: false, embed: embed);
           
        }

        private Embed CreateUserEmbed(List<Player> players)
        {
            var fields = players.Select(CreateUserField).ToList();
            return new EmbedBuilder{
                Title = "Team Match",
                ThumbnailUrl = "https://static.wikia.nocookie.net/ageofempires/images/3/3f/Age2de-library-boxart.jpg",
                //Url = string.Format(TechTreeUrl, civName),
                Fields = fields,
                Color = Color.Red,
                // Footer = new EmbedFooterBuilder {
                //     Text = team,
                //     IconUrl = string.Format(CivIconUrl, civName)
                // }
            }.Build();
        }

        private EmbedFieldBuilder CreateUserField(Player player) => new EmbedFieldBuilder
        {
            Name = $"{player.UserMention} - {player.Team} - {player.PlayerColor}",
            Value = $":{player.Civilization}: - [{player.Civilization}]({string.Format(TechTreeUrl, player.Civilization)})"
        };

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