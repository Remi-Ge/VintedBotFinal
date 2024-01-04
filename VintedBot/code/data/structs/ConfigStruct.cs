using Newtonsoft.Json;

namespace VintedBot.code.data.structs;

public struct ConfigStruct
{
    [JsonProperty("error_channel_id")] public ulong ErrorChannelId;
    [JsonProperty("admin_ids")] public List<ulong> AdminIds;
    [JsonProperty("vinted_channels")] public List<ConfigVintedChannelStruct> VintedChannels;

    public struct ConfigVintedChannelStruct
    {
        [JsonProperty("channel_id")] public ulong ChannelId;
        [JsonProperty("filters")] public ConfigVintedChannelFilters Filters;

        public struct ConfigVintedChannelFilters
        {
            [JsonProperty("brands")] public List<string> Brands;
        }
    }

    public static readonly ConfigStruct BasicConfig = new ConfigStruct()
    {
        ErrorChannelId = 1,
        VintedChannels = new List<ConfigVintedChannelStruct>()
    };
}