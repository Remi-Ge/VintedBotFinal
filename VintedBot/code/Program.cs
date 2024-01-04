using VintedBot.code.data;
using VintedBot.code.discord;
using VintedBot.code.network;
using VintedBot.code.vinted;

namespace VintedBot.code;

public static class Program
{
    private static DataStorage? _dataStorage;
    public static DiscordBot? DiscordBot;
    
    //"https://www.vinted.fr/auth/token_refresh"
    
    public static void Main(string[] args)
    {
        Console.WriteLine("VINTED BOT by Remi");
        Session session = new Session();
        string verification = "no";
        try
        {
            verification = session.Get("https://remidv.fr/projects/vinted_bot/verification.txt")
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception e)
        {
            Environment.Exit(2);
        }
        
        if (!verification.StartsWith("yes"))
        {
            Environment.Exit(2);
        }
        _dataStorage = new DataStorage();
        Vinted.InitializeVintedSession().GetAwaiter().GetResult();
        DiscordBot = new DiscordBot(_dataStorage.Settings.DiscordToken);
        DiscordBot.RunBotAsync().GetAwaiter().GetResult();
    }
}