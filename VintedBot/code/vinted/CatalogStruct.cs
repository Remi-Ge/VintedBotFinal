using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace VintedBot.code.vinted;

public struct CatalogStruct
{
    public static CatalogStruct BasicCatalog = new CatalogStruct()
    {
        Items = new List<CatalogItemStruct>()
    };
    
    [JsonProperty("items")]
    public List<CatalogItemStruct> Items { set; get; }

    public struct CatalogItemStruct
    {
        [JsonProperty("id")] public ulong Id { set; get; }
        [JsonProperty("title")] public string Title { set; get; }
        [JsonProperty("price")] public float Price { set; get; }
        [JsonProperty("currency")] public string Currency { set; get; }
        [JsonProperty("brand_title")] public string BrandTitle { set; get; }
        [JsonProperty("user")] public CatalogItemUserStruct User { set; get; }
        [JsonProperty("url")] public string Url { set; get; }
        [JsonProperty("photo")] public CatalogItemPhotoStruct? Photo { set; get; }
        [JsonProperty("size_title")] public string SizeTitle { set; get; }
        [JsonProperty("status")] public string Status { set; get; }

        public struct CatalogItemUserStruct
        {
            [JsonProperty("login")] public string Pseudo { set; get; }
            [JsonProperty("profile_url")] public string Url { set; get; }
        }
        
        public struct CatalogItemPhotoStruct
        {
            [JsonProperty("url")] public string Url { set; get; }
            [JsonProperty("high_resolution")] public CatalogItemPhotoHighResolutionStruct HighResolution { set; get; }

            public struct CatalogItemPhotoHighResolutionStruct
            {
                [JsonProperty("timestamp")] public ulong Timestamp;
            }
        }

        public DiscordMessageBuilder GetDiscordMessage()
        {
            DiscordMessageBuilder discordMessage = new DiscordMessageBuilder();
            
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder()
            {
                Title = Title,
                Url = Url,
                ImageUrl = Photo?.Url ?? "",
                Description = $"**\ud83d\udc64 Vendeur:** [{User.Pseudo}]({User.Url})"
            };

            if (!string.IsNullOrWhiteSpace(BrandTitle))
            {
                discordEmbedBuilder.AddField("**\ud83d\udd16  Marque:**", $"{BrandTitle}", true);
            }

            if (!string.IsNullOrWhiteSpace(SizeTitle))
            {
                discordEmbedBuilder.AddField("**\ud83d\udccf Taille:**", $"{SizeTitle}", true);
            }

            discordEmbedBuilder.AddField("**\ud83d\udcb0 Prix:**", $"{Price} {Currency}", true);
            discordEmbedBuilder.AddField("**\ud83d\udc8e Etat:**", $"{Status}", true);
            
            discordMessage.WithEmbed(discordEmbedBuilder);
            
            DiscordLinkButtonComponent urlButton = new DiscordLinkButtonComponent(Url, "\ud83d\udcc4 Détails");
            DiscordLinkButtonComponent buyButton = new DiscordLinkButtonComponent(
                $"https://www.vinted.fr/transaction/buy/new?source_screen=item&transaction%5Bitem_id%5D={Id}"
                , "\ud83d\udcb3 Acheter");
            
            discordMessage.AddComponents(urlButton, buyButton);
                
            return discordMessage;
        }
    }
}