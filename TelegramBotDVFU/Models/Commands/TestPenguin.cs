using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class TestPenguin : Command
{
    public override string[] Names
    {
        get => new[] {"Какой ты пингвин?"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await botClient.SendTextMessageAsync(chatId,
            "Мы — Пингвины. Хочешь стать частью нас и понять, кто же мы такие?\n"+
            @"Проходи тест (https://onlinetestpad.com/7eagjo7533o3o), и тебе всё сразу станет ясно.");
        //todo скинут позже
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}