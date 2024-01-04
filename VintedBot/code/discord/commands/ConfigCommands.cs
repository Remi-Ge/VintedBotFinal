using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using VintedBot.code.data;
using VintedBot.code.data.structs;

namespace VintedBot.code.discord.commands;

public class ConfigCommands : ApplicationCommandModule
{
    [SlashCommand("SetVinted", "config a channel to be a vinted one")]
    public async Task SetVintedChannelAsync(InteractionContext ctx
        , [Option("brand_name", "the brand all if everything")] string brand)
    {
        if (!DataStorage.IsUserAdmin(ctx.User.Id))
        {
            return;
        }

        ConfigStruct config = DataStorage.GetConfig();
        int vintedChannelIndex =
            config.VintedChannels.FindIndex(vintedChannel => vintedChannel.ChannelId == ctx.Channel.Id);
        
        if (vintedChannelIndex != -1)
        {
            config.VintedChannels.RemoveAt(vintedChannelIndex);
        }

        List<string> brands = brand.Split(",").ToList();
        
        config.VintedChannels.Add(new ConfigStruct.ConfigVintedChannelStruct()
        {
            ChannelId = ctx.Channel.Id,
            Filters = new ConfigStruct.ConfigVintedChannelStruct.ConfigVintedChannelFilters()
            {
                Brands = brands
            }
        });

        string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(DataStorage.ConfigFilePath, jsonContent);

        await ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("Yep"));
    }
}