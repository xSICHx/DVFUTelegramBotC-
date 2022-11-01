using TelegramBotDVFU.Models;
using TelegramBotDVFU.View;

namespace TelegramBot.Models.Consts;
//todo как получить покупку
public static class ConstAdminMenus
{
    public static readonly Menu[] AllMenus = 
    {
        new("Главное меню", new List<string> {"Магазин", "Развлечения","Админские штучки", "Помощь"}, "Главное меню"),
        new("Развлечения", new List<string> {"Прислать котика", "Назад"}, "Главное меню"),
        new("Магазин", new List<string>{"Ассортимент", "Узнать баланс", "Мои покупки", "Назад"}, "Главное меню"),
        new("Админские штучки",new List<string>{"Засчитать задание", "Выдать мерч","Назад"}, "Главное меню"),
        new("Засчитать задание", new List<string>{"Площадка", "Задать вопрос", "Победа в конкурсе", "ЕБЛАН"}, "Админские штучки"),
        new("Площадка", new List<string>{"VR", "PS", "просмотр", "лекторий", "фото", "СО", "ЕБЛАН"}, "Главное меню"),
        new("Выдать мерч", GetProductButtons(new List<string>{"ЕБЛАН"}), "Главное меню"),
        new("Ассортимент", GetProductButtons(new List<string>{"Назад"}), "Магазин")
    };

    private static List<string> GetProductButtons(List<string> additionalButtons)
    {
        var lst = new List<string>();
        using (var dbProduct = new ApplicationProductContext()){
            lst.AddRange(dbProduct.Products.Select(product => product.Name));
        }
        lst.AddRange(additionalButtons);
        return lst;
    }
}