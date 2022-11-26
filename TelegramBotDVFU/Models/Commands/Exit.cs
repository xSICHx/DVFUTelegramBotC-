using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBotDVFU.Models;
using TelegramBotDVFU.Models.Commands;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class Exit : Command
{
    public override string[] Names
    {
        get => new[] {"Назад"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override void Execute(Message message)
    {
         using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user =  db.Users.Find(message.Chat.Username);
            if (user == null) db.Users.Add(new Usr(message.Chat.Username, message.Chat.Id,"Главное меню"));
            else
            {
                var menuMes = new Message {Chat = message.Chat, Text = user.Menu};
                var (_, _, parent) = Menu.GetMenu(menuMes);
                user.Menu = menuMes.Text = parent;
                
                var (_, buttons, _) = Menu.GetMenu( menuMes);
                // await botClient.SendTextMessageAsync(message.Chat.Id,
                //     ConstReplyes.GetRandomReply(),
                //     replyMarkup: buttons);
                if (user.AdminFlag > 1)
                {
                    user.AdminFlag--;
                }
            }
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