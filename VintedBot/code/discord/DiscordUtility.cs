using DSharpPlus;
using DSharpPlus.Entities;
using VintedBot.code.data;
using VintedBot.code.data.structs;

namespace VintedBot.code.discord;

public static class DiscordUtility
{
    public static DiscordClient? DiscordClient;

    public static async Task SendErrorMessage(Exception e)
    {
        ConfigStruct config = DataStorage.GetConfig();
        DiscordChannel discordChannel = await GetChannel(config.ErrorChannelId);
        string errorMessage = $"**ERROR {DateTime.Now}**\n{e}";
        await discordChannel.SendMessageAsync(errorMessage.Substring(0, Math.Min(2000, errorMessage.Length)));
    }
    
    public static async Task SendErrorMessage(string e)
    {
        ConfigStruct config = DataStorage.GetConfig();
        DiscordChannel discordChannel = await GetChannel(config.ErrorChannelId);
        string errorMessage = $"**ERROR {DateTime.Now}**\n{e}";
        await discordChannel.SendMessageAsync(errorMessage.Substring(0, Math.Min(2000, errorMessage.Length)));
    }

    public static async Task<DiscordChannel> GetChannel(ulong channelId)
    {
        DiscordChannel discordChannel = await DiscordClient.GetChannelAsync(channelId);
        return discordChannel;
    }
}