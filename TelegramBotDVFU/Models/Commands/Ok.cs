using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class Ok
{
    public static async Task Execute(Message? message, TelegramBotClient botClient)
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
            await botClient.SendTextMessageAsync(chatId.Id, "Извините, " + chatId.FirstName + ", я вас не понимаю(",
                replyMarkup: buttons);
        }
    }
}