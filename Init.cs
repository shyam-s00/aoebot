using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord.WebSocket;
using Discord.Commands;

using bot.aoe2.civpicker.services;

namespace bot.aoe2.civpicker
{
    public class Init
    {
        private readonly IConfigurationRoot _configuration;
        public Init()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] { KeyValuePair.Create("token", "ODU4OTI2NTI5NzU2Mzk3NTc4.YNlPqg.r9nyfH4eNQ6HMakwSQ9my8nXIb0"), KeyValuePair.Create("prefix", "!") });

            _configuration = configBuilder.Build();
        }

        public static async Task RunAsync(string[] _)
        {
            var init = new Init();
            await init.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            RegisterServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandlerSerivce>();
            provider.GetRequiredService<AoeAPIService>();
            provider.GetRequiredService<AoeMatchUpService>();
            await provider.GetRequiredService<BotStartupService>().StartAsync();

            await Task.Delay(-1); 
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig{
                MessageCacheSize = 1000,
                DefaultRetryMode = Discord.RetryMode.RetryTimeouts,
                LogLevel = Discord.LogSeverity.Verbose,
                AlwaysDownloadUsers = true
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig {
                DefaultRunMode = RunMode.Async
            }))
            .AddSingleton<CommandHandlerSerivce>()
            .AddSingleton<BotStartupService>()
            .AddSingleton<AoeAPIService>()
            .AddSingleton<AoeMatchUpService>()
            .AddSingleton(_configuration);
        }
    }
}