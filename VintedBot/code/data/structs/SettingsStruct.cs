using Newtonsoft.Json;

namespace VintedBot.code.data.structs;

public struct SettingsStruct
{
    [JsonProperty("discord_token")] public string DiscordToken { set; get; }

    public static readonly SettingsStruct BasicSettings = new SettingsStruct()
    {
        DiscordToken = "ENTER TOKEN HERE"
    };
}