﻿using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class ChangeMenu : Command
{
    public override string[] Names
    {
        get => new[] {"Развлечения", "Магазин", "P!N", "Тесты", "Миссии","Помощь"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        var (_, buttons, _) = Menu.GetMenu(message);
        await botClient.SendTextMessageAsync(chatId, "Что дальше?",
            replyMarkup: buttons);
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}