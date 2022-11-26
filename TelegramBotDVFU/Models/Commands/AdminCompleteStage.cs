using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models.Consts;
using TelegramBot.View;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class AdminCompleteStage: Command
{
    public override string[] Names
    {
        get;
        set;
    }

    public AdminCompleteStage()
    {
        var names = new List<string>();
        names.Add("Засчитать задание");
        foreach (var trial in ConstTrials.TrialsList)
        {
            names.Add(trial.Name);
        }

        Names = names.ToArray();
    }
    public override int AdminsCommand => 1;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
        using var db = new ApplicationUserContext();
        var userAdmin = db.Users.Find(message.Chat.Username);
        // var (name, buttons, parent) = Menu.GetMenu(message);
        switch (userAdmin.AdminFlag)
        {
            case 1:
                userAdmin.AdminFlag = 2;
                db.Users.Update(userAdmin);
                db.SaveChanges();
                db.Entry(userAdmin).State = EntityState.Modified;
                // await botClient.SendTextMessageAsync(chatId, "Выберите название испытания", replyMarkup:buttons);
                break;
            case 2:
                userAdmin.AdminFlag = 3;
                Console.WriteLine("WIN");
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
            case 3:
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
                            using var dbAdmin = new ApplicationAdminContext();
                            var admin =  dbAdmin.Admins.Find(message.Chat.Username);
                            if (!user.TrialsDict.ContainsKey(admin.CurrentUserTrial[0]))
                                throw new Exception(admin.CurrentUserTrial[0]);
                            TrialConst trial = ConstTrials.Find(admin.CurrentUserTrial[0]);
                            var flagAllTrialsCompleted = trial.CheckIfAllCompleted(user.Id);
                                
                            if (flagAllTrialsCompleted)
                            {
                                // await botClient.SendTextMessageAsync(chatId, 
                                //     "У данного конкурсанта выполнены все задания на этом испытании",
                                //     replyMarkup: buttons);
                                break;
                            }
                                    
                            if (trial.CheckCompleted(user.Id))
                            {
                                // await botClient.SendTextMessageAsync(chatId, 
                                //     "У данного конкурсанта выполнено это задание",
                                //     replyMarkup: buttons);
                                break;
                            }
                                    
                            user.TrialsDict[trial.Name] += 1;
                            user.AmountOfMoney += trial.Reward;
                            db.Users.Update(user);
                            db.SaveChanges();
                            // await botClient.SendTextMessageAsync(chatId, 
                            //     "Задание " + admin.CurrentUserTrial[0]+" успешно зачтено конкурсанту " +
                            //     user.Id,
                            //     replyMarkup: buttons);
                            // await botClient.SendTextMessageAsync(user.ChatId,
                            //     "Вы успешно выполнили задание " + admin.CurrentUserTrial[0]
                            //                                     +". Начислено валюты: " + trial.Reward);

                            if (trial.AdditionalReward == 0)
                                break;
                            flagAllTrialsCompleted = trial.CheckIfAllCompleted(user.Id);
                                
                
                            if (flagAllTrialsCompleted)
                            {
                                user.AmountOfMoney += trial.AdditionalReward;
                                // await botClient.SendTextMessageAsync(user.ChatId,
                                //     "Вы успешно выполняли наши задания." +
                                //     " За это вам выдаётся дополнительное вознаграждение: " + trial.AdditionalReward);
                            }
                        }
                        else
                        {
                            // await botClient.SendTextMessageAsync(chatId,
                            //     "Такой человек не был на мероприятии", replyMarkup: buttons);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        // await botClient.SendTextMessageAsync(chatId,
                        //     "Неправильный формат ввода1", replyMarkup: buttons);
                    }
                }
                else{
                    // await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода2",
                    // replyMarkup: buttons);
                        
                }
                break;
            //
            default:
                // await botClient.SendTextMessageAsync(chatId, "Неправильный формат ввода3", replyMarkup: buttons);
                userAdmin.AdminFlag = 1;
                break;
        }

        db.Update(userAdmin);
        db.SaveChanges();
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;
        var regex = new Regex(@"@\w*");
        
        bool flag;
        using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var userAdmin = db.Users.Find(message.Chat.Username);
            flag = userAdmin.AdminFlag == 3;
        }
        return message.Text != null && Names.Contains(message.Text) || regex.IsMatch(message.Text) && flag;
    }
}