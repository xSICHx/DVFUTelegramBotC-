using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Consts;
//todo как получить покупку
public static class ConstAdminMenus
{
    public static readonly Menu[] AllMenus = 
    {
        new("Главное меню", new List<string> {"Магазин", "Развлечения","Засчитать задание", "Помощь"}, "Главное меню"),
        new("Развлечения", new List<string> {"Прислать котика", "Назад"}, "Главное меню"),
        new("Магазин", new List<string>{"Ассортимент", "Узнать баланс", "Как получить покупку?", "Назад"}, "Главное меню"),
        new("Засчитать задание", new List<string>{"Площадка", "ЕБЛАН"}, "Главное меню"),
        new("Площадка", new List<string>{"VR", "PS", "просмотр", "лекторий", "фото", "СО", "ЕБЛАН"}, "Главное меню"),
        new("Ассортимент", new List<string>{"Пиво", "Рыба", "Сухарики", "Назад"}, "Магазин")
    };
}