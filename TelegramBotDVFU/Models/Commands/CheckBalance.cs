using Telegram.Bot;
using Telegram.Bot.Types;


namespace TelegramBotDVFU.Models.Commands;

public class CheckBalance : Command
{
    public override string[] Names
    {
        get => new[] {"Узнать баланс"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user = await db.Users.FindAsync(message.Chat.Username);
            await botClient.SendTextMessageAsync(chatId, "На вашем счету " + user.AmountOfMoney + " игровой валюты.");
            await db.SaveChangesAsync();
        }
        
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}