using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.View;

public class BuyReturn
{
    public static InlineKeyboardMarkup Buttons => new(new[]
    {
        new InlineKeyboardButton(" "){CallbackData = "solve_none"},
        new InlineKeyboardButton("C"){CallbackData = "solve_C"},
    });
}