using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class Eblan: Command
{
    public override string[] Names
    {
        get => new[] {"Запутався"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 1;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message?.Chat;
        if (chatId != null)
        {
            await using (ApplicationUserContext db = new ApplicationUserContext())
            {
                var user = await db.Users.FindAsync(message.Chat.Username);
                if (user.AdminFlag > 0)
                    user.AdminFlag = 1;
                await db.SaveChangesAsync();
            }

            var (_, buttons, _) = Menu.GetMenu(message);
            await botClient.SendTextMessageAsync(chatId.Id, "Ошибки случаются, это нормально)",
                replyMarkup: buttons);
        }
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;
        return message.Text != null && Names.Contains(message.Text);
    }
}