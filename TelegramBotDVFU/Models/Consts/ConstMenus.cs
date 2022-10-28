using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Consts;

public static class ConstMenus
{
    public static readonly Menu[] AllMenus = 
    {
        new("Главное меню", new List<string>{"Магазин", "Развлечения", "Помощь"}, "Главное меню"),
        new("Развлечения", new List<string>{"Прислать котика", "Назад"}, "Главное меню"),
        new("Магазин", new List<string>{"Ассортимент", "Узнать баланс", "Как получить покупку?", "Назад"}, "Главное меню"),
        new("Ассортимент", new List<string>{"Пиво", "Рыба", "Сухарики", "Назад"}, "Магазин")
    };
}