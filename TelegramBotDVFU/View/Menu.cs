using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;
using TelegramBotDVFU.Models.Consts;

namespace TelegramBotDVFU.View;

public class Menu
{
    internal string Name { get;}
    private ReplyKeyboardMarkup Buttons { get; }
    private string Parent { get; } = "Главное меню";

    public Menu(string name, List<string> buttons)
    {
        Name = new string(name);
        var coolKb = new List<List<KeyboardButton>>();
        var temp = new List<KeyboardButton>();
        for (var i = 0; i < buttons.Count; ++i)
        {
            temp.Add(new KeyboardButton(buttons[i]));
            if (i % 2 != 1 && i != buttons.Count - 1) continue;
            var temp2 = new List<KeyboardButton>();
            foreach (var t in temp)
            {
                var temp3 = new KeyboardButton(t.Text);
                temp2.Add(temp3);
            }
            coolKb.Add(temp2);
            temp.Clear();
        }

        Buttons = new ReplyKeyboardMarkup(coolKb) {ResizeKeyboard = true};
    }
    public Menu(string name, List<string> buttons, string parent) : this(name, buttons) => 
        Parent = new string(parent);

    public static async Task<(string Name, ReplyKeyboardMarkup Buttons, string Parent)> GetMenu(Message? message)
    {
        await using var db = new ApplicationUserContext();
        
        var user = await db.Users.FindAsync(message.Chat.Username);
        var allMenus = user is {AdminFlag: > 0} ? ConstAdminMenus.AllMenus : ConstMenus.AllMenus;

        foreach (var menu in allMenus)
        {
            if (message?.Text != menu.Name) continue;
            
            if (user == null) db.Users.Add(new Usr(message.Chat.Username, message.Chat.Id,"Главное меню"));
            else user.Menu = menu.Name;
            await db.SaveChangesAsync();
            return (menu.Name, menu.Buttons, menu.Parent);
        }

        if (user != null) user.Menu = "Главное меню";
        return (allMenus[0].Name, allMenus[0].Buttons, allMenus[0].Parent);
    }

}
