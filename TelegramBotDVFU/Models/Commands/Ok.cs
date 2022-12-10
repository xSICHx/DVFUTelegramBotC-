using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class Ok
{
    public static async Task Execute(Message? message)
    {
        var chatId = message?.Chat;
        if (chatId != null)
        {
             using (ApplicationUserContext db = new ApplicationUserContext())
            {
                var user =  db.Users.Find(message.Chat.Username);
                if (user.AdminFlag > 0)
                    user.AdminFlag = 1;
                 db.SaveChanges();
            }

            var (_, buttons, _) = await Menu.GetMenu(message);
            // await botClient.SendTextMessageAsync(chatId.Id, "Извините, " + chatId.FirstName + ", я вас не понимаю(",
            //     replyMarkup: buttons);
        }
    }
}