using Telegram.Bot;
using TelegramBotDVFU.Models.Commands;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.Models.Queries;
using TelegramBotDVFU.View;
using TelegramBotDVFU.View.Products;


namespace TelegramBotDVFU.Models;

public static class Bot
{
    private static TelegramBotClient botClient;
    private static List<Command> commandsList;
    public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();
    
    private static List<Query> QueryList;
    public static IReadOnlyList<Query> Queries => QueryList.AsReadOnly();

    public static void GetBotClient()
    {
        commandsList = new List<Command>();
        commandsList.Add(new AdminCompleteStage());
        commandsList.Add(new Eblan());
        commandsList.Add(new StartCommand());
        commandsList.Add(new HelpDeveloper());
        commandsList.Add(new HelpQuick());
        commandsList.Add(new HelpChat());
        commandsList.Add(new Exit());
        commandsList.Add(new ChangeMenu());
        commandsList.Add(new ChangeAdminMenu());
        commandsList.Add(new SendCat());
        commandsList.Add( new CheckBalance());
        commandsList.Add( new Shop());
        commandsList.Add(new ShowProduct());
        commandsList.Add(new MyProducts());
        commandsList.Add(new AdminGiveGift());
        commandsList.Add(new TestFox());
        commandsList.Add(new TestPenguin());
        commandsList.Add(new PinChannel());
        commandsList.Add(new PinMap());
        commandsList.Add(new CurrentMissions());
        commandsList.Add(new CompletedMissions());
        commandsList.Add(new Schedule());
        commandsList.Add( new HelpGetPincoins());
        
        QueryList = new List<Query>();
        QueryList.Add(new BuyReturn());
        
        //
        // botClient = new TelegramBotClient(AppSettings.Key);
        // return Task.FromResult(botClient);
    } 
}