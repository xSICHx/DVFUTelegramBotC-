using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class HelpQuick : Command
{
    public override string[] Names
    {
        get => new[] {"Срочная хелпа"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await botClient.SendTextMessageAsync(chatId,
            @"Потерялся? Забылся? Хочешь зарегестрироваться? Есть вопросы? Напиши @Kuchuganova_Alina");
       
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}