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
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user = await db.Users.FindAsync(message.Chat.Username);
            var (_, buttons, _) = Menu.GetMenu(message);
            await botClient.SendTextMessageAsync(chatId,
                "В нашем магазе ровным счётом нихуя. Кроме рыбы, пива и сухарей. Выберите то, о чём желаете узнать поподробнее",
                replyMarkup:buttons);
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