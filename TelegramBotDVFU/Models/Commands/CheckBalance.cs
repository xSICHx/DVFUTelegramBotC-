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
    public override async Task Execute(Message message)
    {
        var chatId = message.Chat.Id;
        using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user =  db.Users.Find(message.Chat.Username);
            // await botClient.SendTextMessageAsync(chatId, 
            //     "Знаешь, ты молодец, ведь поднял бабла. И всего у тебя получилось: "
            //     + user.AmountOfMoney + " P!N-коинов.");
             db.SaveChanges();
        }
        
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}