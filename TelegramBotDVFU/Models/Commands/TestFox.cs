using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class TestFox : Command
{
    public override string[] Names
    {
        get => new[] {"Какая ты лисичка?"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
        // await botClient.SendTextMessageAsync(chatId,
        //     "Мы — Наставники." +
        //     " Нашим символом является лиса. Хочешь проверить какая ты лисичка из всеми известных персонажей?" +
        //     " Тогда скорее проходи тест и делись результатом в комментариях под постом)" +
        //     "Любим заботиться о других и помогать." +
        //     " Хочешь узнать, каким наставником ты бы мог быть? Тогда проходи наш тест (https://onlinetestpad.com/43psgcdu4tinu) ");
        
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}