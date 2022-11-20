using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Consts;

public static class ConstMenus
{
    public static readonly Menu[] AllMenus;

    static ConstMenus()
    {
        AllMenus = new[]
        {
            new Menu("Главное меню",
                new List<string>{"P!N","Магазин", "Развлечения", "Миссии", "Помощь"}, "Главное меню"),
            
            new Menu("Помощь",
                new List<string>{"Разработчик бота","Срочная хелпа", "Чат P!N'a", @"Как купить мерч?", "Назад"}, "Главное меню"),
            
            new Menu("Миссии",
                new List<string>{"Текущие миссии", "Завершённые миссии", "Назад"}, "Главное меню"),
            
            new Menu("P!N",
                new List<string>{"Расписание",
                    "Тесты", "Канал P!N'a", "P!N - карта", "Назад"}, "Главное меню"),
            
            new Menu("Тесты",
                new List<string>{"Какая ты лисичка?", "Какой ты пингвин?", "Назад"}, "P!N"),
            
            new Menu("Развлечения",
                new List<string>{"Прислать котика", "Назад"}, "Главное меню"),
            
            new Menu("Магазин",
                new List<string>{"Ассортимент", "Узнать баланс", "Мои покупки", "Назад"}, "Главное меню"),
            
            new Menu("Ассортимент",
                GetProductButtons(new List<string>{"Назад"}), "Магазин")
            
        };
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
}