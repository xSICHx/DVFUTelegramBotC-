using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class Shop : Command
{
    public override string[] Names
    {
        get => new[] {"Ассортимент"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
         using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user =  db.Users.Find(message.Chat.Username);
            var (_, buttons, _) = Menu.GetMenu(message);
            // await botClient.SendTextMessageAsync(chatId,
            //     "Зарабатывай P!N-коины, выполняя миссии, и обменивай их на наш крутой мерч",
            //     replyMarkup:buttons);
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