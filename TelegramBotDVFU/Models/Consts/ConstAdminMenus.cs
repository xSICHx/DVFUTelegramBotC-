using TelegramBot.View;
using TelegramBotDVFU.Models;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View;

namespace TelegramBot.Models.Consts;
public static class ConstAdminMenus
{
    public static readonly Menu[] AllMenus;

    static ConstAdminMenus()
    {
        var lstMenu = new List<Menu>();
        lstMenu.Add(new Menu("Главное меню", new List<string>{"P!N", "Магазин", "Развлечения", "Миссии", "Админские штучки", "Помощь"}, "Главное меню"));
        lstMenu.AddRange(ConstMenus.AllMenus.Where(menu => menu.Name != "Главное меню").ToList());
        lstMenu.AddRange(new Menu[]{new("Админские штучки",
                new List<string>{"Засчитать задание", "Выдать мерч","Назад"}, "Главное меню"),
            new("Засчитать задание", new List<string>{"Площадка", "Задать вопрос", "Победа в конкурсе", "Пасхальное фото", "Запутався"}, "Админские штучки"),
            new("Площадка", GetMissionPloshadkaButtons(new List<string>{"Запутався"}), "Главное меню"),
            new("Выдать мерч", GetProductButtons(new List<string>{"Запутався"}), "Главное меню")});
        AllMenus = lstMenu.ToArray();
    }

    private static List<string> GetProductButtons(List<string> additionalButtons)
    {
        var lst = new List<string>();
        using (var dbProduct = new ApplicationProductContext()){
            lst.AddRange(dbProduct.Products.Select(product => product.Name));
        }
        lst.AddRange(additionalButtons);
        return lst;
    }
    private static List<string> GetMissionPloshadkaButtons(List<string> additionalButtons)
    {
        var lst = (from trial in ConstTrials.TrialsList where trial.GetType() == typeof(Ploshadka) select trial.Name).ToList();
        lst.AddRange(additionalButtons);
        return lst;
    }
}