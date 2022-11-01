using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Consts;

public static class ConstMenus
{
    public static readonly Menu[] AllMenus = 
    {
        new("Главное меню", new List<string>{"Магазин", "Развлечения", "Помощь"}, "Главное меню"),
        new("Развлечения", new List<string>{"Прислать котика", "Назад"}, "Главное меню"),
        new("Магазин", new List<string>{"Ассортимент", "Узнать баланс", "Мои покупки", "Назад"}, "Главное меню"),
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