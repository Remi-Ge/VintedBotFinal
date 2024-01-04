using Newtonsoft.Json;
using VintedBot.code.data.structs;

namespace VintedBot.code.data;

public class DataStorage
{
    private static readonly string ProjectName = "VintedBot";
    private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath
        (Environment.SpecialFolder.ApplicationData), "RemiDv", ProjectName);
    public static readonly string SettingsFilePath = Path.Combine(SaveDirectory
        , "settings.json");
    public static readonly string ConfigFilePath = Path.Combine(SaveDirectory
        , "config.json");

    public SettingsStruct Settings;

    public DataStorage()
    {
        if (!Directory.Exists(SaveDirectory) && !File.Exists(SettingsFilePath))
        {
            FirstTimeOpened();
        }

        string settingsFileContent = File.ReadAllText(SettingsFilePath);
        Settings = JsonConvert.DeserializeObject<SettingsStruct>(settingsFileContent);
    }
    
    public static void FirstTimeOpened()
    {
        Directory.CreateDirectory(SaveDirectory);
        SettingsStruct settings = SettingsStruct.BasicSettings;
        string jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(SettingsFilePath, jsonSettings);
        ConfigStruct config = ConfigStruct.BasicConfig;
        string jsonConfig = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(ConfigFilePath, jsonConfig);
        Environment.Exit(2);
    }

    public static ConfigStruct GetConfig()
    {
        string configJson = File.ReadAllText(ConfigFilePath);
        ConfigStruct config = JsonConvert.DeserializeObject<ConfigStruct>(configJson);
        return config;
    }

    public static bool IsUserAdmin(ulong userId)
    {
        ConfigStruct config = GetConfig();
        if (config.AdminIds.Contains(userId))
        {
            return true;
        }

        return false;
    }
}