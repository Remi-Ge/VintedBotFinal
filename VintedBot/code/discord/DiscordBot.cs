using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using VintedBot.code.discord.commands;
using VintedBot.code.vinted;

namespace VintedBot.code.discord;

public class DiscordBot
{
    public readonly DiscordClient DiscordClient;
    
    public DiscordBot(string discordToken)
    {
        DiscordClient = ConfigureBot(discordToken);
        DiscordUtility.DiscordClient = DiscordClient;
    }

    private DiscordClient ConfigureBot(string discordToken)
    {
        DiscordConfiguration discordBotConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = discordToken,
            TokenType = TokenType.Bot,
            AutoReconnect = true
        };

        DiscordClient discordClient = new DiscordClient(discordBotConfig);

        discordClient.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromSeconds(30)
        });
        
        SlashCommandsExtension slashCommandsConfig = discordClient.UseSlashCommands();
        RegisterSlashCommands(slashCommandsConfig);

        return discordClient;
    }

    private void RegisterSlashCommands(SlashCommandsExtension slashCommandsExtension)
    {
        slashCommandsExtension.RegisterCommands<InformationCommands>();
        slashCommandsExtension.RegisterCommands<ConfigCommands>();
    }

    public async Task RunBotAsync()
    {
        await DiscordClient.ConnectAsync();
        Task.Run(async () => await Vinted.StartVintedLoop());
        await Task.Delay(-1);
    }
}