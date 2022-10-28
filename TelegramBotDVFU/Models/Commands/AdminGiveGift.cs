using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class AdminGiveGift: Command
{
    public override string[] Names => new[]
    {
        "Выдать мерч",
        "Пиво", "Рыба", "Сухарики"
    };
    public override int AdminsCommand => 1;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var userAdmin = await db.Users.FindAsync(message.Chat.Username);
            var (name, buttons, parent) = Menu.GetMenu(message);
            switch (userAdmin.AdminFlag)
            {
                case 1:
                    userAdmin.AdminFlag = 2;
                    await botClient.SendTextMessageAsync(chatId, "Выберите название испытания", replyMarkup:buttons);
                    break;
                case 2:
                    userAdmin.AdminFlag = 3;
                    await using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                    {
                        var admin = await dbAdmin.Admins.FindAsync(message.Chat.Username);
                        admin.CurrentUserTrial[0] = message.Text;
                        dbAdmin.Admins.Update(admin);
                        await dbAdmin.SaveChangesAsync();
                    }
                    await botClient.SendTextMessageAsync(chatId, "Выберите название стадии", replyMarkup: buttons);
                    break;
                case 3:
                    userAdmin.AdminFlag = 4;
                    await using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                    {
                        var admin = await dbAdmin.Admins.FindAsync(message.Chat.Username);
                        admin.CurrentUserTrial[1] = message.Text;
                        dbAdmin.Admins.Update(admin);
                        await dbAdmin.SaveChangesAsync();
                    }
                    buttons = new ReplyKeyboardMarkup("ЕБЛАН"){ResizeKeyboard = true};
                    await botClient.SendTextMessageAsync(
                        chatId,
                        "Введите на клавиатуре имя пользователя в формате: \n@имя",
                        replyMarkup: buttons);
                    break;
                case 4:
                    userAdmin.AdminFlag = 1;
                    var regex = new Regex(@"@\w*");
                    if (regex.IsMatch(message.Text))
                    {
                        try
                        {
                            var word = message.Text[1..];
                            var user = await db.Users.FindAsync(word);
                            if (user != null)
                            {
                                using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                                {
                                    var admin = await dbAdmin.Admins.FindAsync(message.Chat.Username);
                                    if (!user.TrialsDict.ContainsKey(admin.CurrentUserTrial[0]))
                                        throw new Exception();
                                    if (!user.TrialsDict[admin.CurrentUserTrial[0]]
                                            .ContainsKey(admin.CurrentUserTrial[1]))
                                        throw new Exception();
                                    var flagAllTrialsCompleted = 1;
                                    foreach (var trial in user.TrialsDict[admin.CurrentUserTrial[0]].Values)
                                    {
                                        if (trial == 0)
                                        {
                                            flagAllTrialsCompleted = 0;
                                            break;
                                        }
                                    }
                                    if (flagAllTrialsCompleted != 0)
                                    {
                                        await botClient.SendTextMessageAsync(chatId, 
                                            "У данного конкурсанта выполнены все задания на этом испытании",
                                            replyMarkup: buttons);
                                        break;
                                    }
                                    
                                    if (user.TrialsDict[admin.CurrentUserTrial[0]][admin.CurrentUserTrial[1]] == 1)
                                    {
                                        await botClient.SendTextMessageAsync(chatId, 
                                            "У данного конкурсанта выполнено это задание",
                                            replyMarkup: buttons);
                                        break;
                                    }
                                    
                                    user.TrialsDict[admin.CurrentUserTrial[0]][admin.CurrentUserTrial[1]] = 1;
                                    db.Users.Update(user);
                                    await db.SaveChangesAsync();
                                    await botClient.SendTextMessageAsync(chatId, 
                                        "Задание " + admin.CurrentUserTrial[1]+" успешно зачтено конкурсанту " +
                                        user.Id,
                                        replyMarkup: buttons);
                                    await botClient.SendTextMessageAsync(user.ChatId,
                                        "Вы успешно выполнили задание " + admin.CurrentUserTrial[1]);
                                    
                                    flagAllTrialsCompleted = 1;
                                    foreach (var trial in user.TrialsDict[admin.CurrentUserTrial[0]].Values)
                                    {
                                        if (trial == 0)
                                        {
                                            flagAllTrialsCompleted = 0;
                                            break;
                                        }
                                    }

                                    if (flagAllTrialsCompleted == 1)
                                    {
                                        user.AmountOfMoney += ConstTrialsReward.Reward[admin.CurrentUserTrial[0]];
                                        await botClient.SendTextMessageAsync(user.ChatId,
                                            "Вы успешно выполнили испытание " + admin.CurrentUserTrial[0] +
                                            ". На ваш баланс зачислено " +
                                            ConstTrialsReward.Reward[admin.CurrentUserTrial[0]]);
                                    }
                                }
                            }
                            else
                                await botClient.SendTextMessageAsync(chatId,
                                    "Такой человек не был на мероприятии", replyMarkup: buttons);
                        }
                        catch
                        {
                            await botClient.SendTextMessageAsync(chatId,
                                "Неправильный формат ввода", replyMarkup: buttons);
                        }
                    }
                    else await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода",
                        replyMarkup: buttons);
                    break;
                default:
                    await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода", replyMarkup: buttons);
                    userAdmin.AdminFlag = 1;
                    break;
            }
            await db.SaveChangesAsync();
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