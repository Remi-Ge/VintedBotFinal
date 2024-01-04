using DSharpPlus.SlashCommands;
using VintedBot.code.vinted;

namespace VintedBot.code.discord.commands;

public class InformationCommands : ApplicationCommandModule
{
    [SlashCommand("test", "test")]
    public async Task TestAsync(InteractionContext ctx)
    {
        try
        {
            CatalogStruct catalog = await Vinted.GetCatalog();
            await ctx.Channel.SendMessageAsync(catalog.Items[0].GetDiscordMessage());
        }
        catch (Exception e)
        {
            await DiscordUtility.SendErrorMessage(e);
        }
    }
}