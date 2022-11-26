using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class AdminGiveGift: Command
{
    public override string[] Names { get; set; }
    public override int AdminsCommand => 1;

    public AdminGiveGift()
    {
        var lstNames = new List<string> {"Выдать мерч"};
        using var dbProduct = new ApplicationProductContext();
        lstNames.AddRange(dbProduct.Products.Select(product => product.Name));
        Names = lstNames.ToArray();
    }
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
         using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var userAdmin =  db.Users.Find(message.Chat.Username);
            var (name, buttons, parent) = Menu.GetMenu(message);
            switch (userAdmin.AdminFlag)
            {
                case 1:
                    userAdmin.AdminFlag = 5;
                    // await botClient.SendTextMessageAsync(chatId, "Выберите название мерча", replyMarkup:buttons);
                    break;
                
                case 5:
                    userAdmin.AdminFlag = 6;
                     using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                    {
                        var admin =  dbAdmin.Admins.Find(message.Chat.Username);
                        admin.CurrentUserTrial[0] = message.Text;
                        dbAdmin.Admins.Update(admin);
                         dbAdmin.SaveChanges();
                    }
                    // buttons = new ReplyKeyboardMarkup("Запутався"){ResizeKeyboard = true};
                    // await botClient.SendTextMessageAsync(
                    //     chatId,
                    //     "Введите на клавиатуре имя пользователя в формате: \n@имя",
                    //     replyMarkup: buttons);
                    break;
                case 6:
                    userAdmin.AdminFlag = 1;
                    var regex = new Regex(@"@\w*");
                    if (regex.IsMatch(message.Text))
                    {
                        try
                        {
                            var word = message.Text[1..];
                            var user =  db.Users.Find(word);
                            if (user != null)
                            {
                                using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                                {
                                    var admin =  dbAdmin.Admins.Find(message.Chat.Username);
                                    if (user.ProductsPurchaced[admin.CurrentUserTrial[0]] > 0)
                                    {
                                        user.ProductsPurchaced[admin.CurrentUserTrial[0]]--;
                                        db.Update(user);
                                         db.SaveChanges();
                                        
                                        
                                        // await botClient.SendTextMessageAsync(chatId, 
                                        //     "Товар " + admin.CurrentUserTrial[0]+
                                        //     " выдан конкурсанту " + user.Id + ". Не забудьте его отдать в реальной жизни)",
                                        //     replyMarkup: buttons);
                                        // await botClient.SendTextMessageAsync(user.ChatId,
                                        //     "Вам выдали товар " + admin.CurrentUserTrial[0] + ". Не забудьте его забрать в реальной жизни)");
                                        
                                        
                                    }
                                    else
                                    {
                                        // await botClient.SendTextMessageAsync(chatId,
                                        //     "У конкурсанта " + user.Id + " не куплен товар " + admin.CurrentUserTrial[0],
                                        //     replyMarkup: buttons);
                                    }
                                }
                            }
                            else
                            {
                                // await botClient.SendTextMessageAsync(chatId,
                                //     "Такой человек не был на мероприятии", replyMarkup: buttons);
                            }
                        }
                        catch
                        {
                            // await botClient.SendTextMessageAsync(chatId,
                            //     "Неправильный формат ввода", replyMarkup: buttons);
                        }
                    }
                    else{ 
                        // await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода",
                        // replyMarkup: buttons);
                        
                    }
                    break;
                default:
                    // await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода", replyMarkup: buttons);
                    userAdmin.AdminFlag = 1;
                    break;
            }
             db.SaveChanges();
        }
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;
        var regex = new Regex(@"@\w*");
        return message.Text != null && Names.Contains(message.Text) || regex.IsMatch(message.Text);
    }
}