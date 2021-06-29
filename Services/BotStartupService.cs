using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;


namespace bot.aoe2.civpicker.services
{
    public class BotStartupService
    {
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;

        public BotStartupService(IConfigurationRoot config, 
            DiscordSocketClient discord,
            IServiceProvider serviceProvider,
            CommandService commands)
        {
            _config = config;
            _commands = commands;
            _discord = discord;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync()
        {
            var token = _config["token"];
            if (string.IsNullOrEmpty(token?.Trim())) {
                throw new Exception("Discord Token is missing in the configuration. Please check");
            }

            _discord.Log += OnLog;
            
            await _discord.LoginAsync(Discord.TokenType.Bot, token);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider); // Add the entire assembly to add all the modules ... 
        }

        private Task OnLog(LogMessage log)
        {
            Console.WriteLine(log.Message);
            return Task.CompletedTask;
        }
    }
}