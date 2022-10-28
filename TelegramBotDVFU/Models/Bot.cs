using Telegram.Bot;
using TelegramBotDVFU.Models.Commands;
using TelegramBotDVFU.Models.Queries;


namespace TelegramBotDVFU.Models;

public static class Bot
{
    private static TelegramBotClient botClient;
    private static List<Command> commandsList;
    public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();
    
    private static List<Query> QueryList;
    public static IReadOnlyList<Query> Queries => QueryList.AsReadOnly();

    public static Task<TelegramBotClient> GetBotClient()
    {
        commandsList = new List<Command>();
        commandsList.Add(new AdminCompleteStage());
        commandsList.Add(new Eblan());
        commandsList.Add(new StartCommand());
        commandsList.Add(new Help());
        commandsList.Add(new HelpRus());
        commandsList.Add(new Exit());
        commandsList.Add(new Entertainment());
        commandsList.Add(new SendCat());
        commandsList.Add( new CheckBalance());
        commandsList.Add( new Shop());
        commandsList.Add(new ShowProduct());

        QueryList = new List<Query>();
        QueryList.Add(new BuyReturn());
        
        botClient = new TelegramBotClient(AppSettings.Key);
        return Task.FromResult(botClient);
    } 
}