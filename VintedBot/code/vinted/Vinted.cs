using DSharpPlus.Entities;
using Newtonsoft.Json;
using VintedBot.code.discord;
using VintedBot.code.network;
using VintedBot.code.data;
using VintedBot.code.data.structs;

namespace VintedBot.code.vinted;

public static class Vinted
{
    private static readonly string CatalogUrl = "https://www.vinted.fr/api/v2/catalog/items?&brand_ids[]=88" +
                                                "&brand_ids[]=53&brand_ids[]=362&brand_ids[]=65&brand_ids[]=304" +
                                                "&brand_ids[]=94&brand_ids[]=441&brand_ids[]=10&brand_ids[]=14" +
                                                "&brand_ids[]=2319&brand_ids[]=73306&brand_ids[]=73952&brand_ids[]=6539" +
                                                "&brand_ids[]=3573&brand_ids[]=567&brand_ids[]=417" +
                                                "&brand_ids[]=671&brand_ids[]=364&brand_ids[]=2021&brand_ids[]=765" +
                                                "&order=newest_first&per_page=50";

    private static readonly Session Session = new Session();
    
    private static ulong _recentDate = 0;

    public static async Task InitializeVintedSession()
    {
        await Session.Post("https://www.vinted.fr/auth/token_refresh");
    }

    public static async Task<CatalogStruct> GetCatalog()
    {
        try
        {
            Console.WriteLine("Starting to get the Catalog...");
            string jsonCatalog;
            try
            {
                jsonCatalog = await Session.Get(CatalogUrl);
            }
            catch (Exception e)
            {
                await InitializeVintedSession();
                throw;
            }
            Console.WriteLine("Deserializing...");
            CatalogStruct catalog = JsonConvert.DeserializeObject<CatalogStruct>(jsonCatalog);
            Console.WriteLine("Returning...");
            return catalog;
        }
        catch (Exception e)
        {
            await DiscordUtility.SendErrorMessage(e);
        }

        return CatalogStruct.BasicCatalog;
    }

    public static async Task StartVintedLoop()
    {
        while (true)
        {
            try
            {
                await Task.Delay(2000);
                ConfigStruct config = DataStorage.GetConfig();
                CatalogStruct catalog = await GetCatalog();
                int a = 0;
                foreach (CatalogStruct.CatalogItemStruct item in catalog.Items)
                {
                    Console.WriteLine(item.Photo?.HighResolution.Timestamp);
                    if (item.Photo?.HighResolution.Timestamp > _recentDate)
                    {
                        a++;
                        Console.WriteLine(item.BrandTitle);
                        foreach (ConfigStruct.ConfigVintedChannelStruct vintedChannel in config.VintedChannels)
                        {
                            if (vintedChannel.Filters.Brands.Contains(item.BrandTitle))
                            {
                                DiscordChannel channel = await DiscordUtility.GetChannel(vintedChannel.ChannelId);
                                await channel.SendMessageAsync(item.GetDiscordMessage());
                            }
                        }
                    }
                }

                _recentDate = catalog.Items.MaxBy
                    (item => item.Photo?.HighResolution.Timestamp).Photo?.HighResolution.Timestamp ?? 0;


                Console.WriteLine(a);
            }
            catch (Exception e)
            {
                await DiscordUtility.SendErrorMessage(e);
            }
        }
    }
}