using System.Text;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;

namespace TelegramBotDVFU.Controllers;

public class MessageController : Controller
{
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Некоторые действия
        using (FileStream fstream = new FileStream("logs.txt", FileMode.Append))
        {
            // преобразуем строку в байты
            byte[] buffer = Encoding.Default.GetBytes("\n" + Newtonsoft.Json.JsonConvert.SerializeObject(update)+"\n");
            // запись массива байтов в файл
            await fstream.WriteAsync(buffer, 0, buffer.Length);
            fstream.Close();
        }
        Console.WriteLine("\n" + Newtonsoft.Json.JsonConvert.SerializeObject(update)+"\n");
        var tresh = update.Message?.Chat.Username ?? update.CallbackQuery?.From.Username;
        if (tresh is not null)
        {
            if (update.Type is UpdateType.Message or UpdateType.CallbackQuery)
            {
                 
                //!(update.Message?.Chat.Username == null || update.CallbackQuery!.From.Username == null) 
                var commands = Bot.Commands;
                var queries = Bot.Queries;
                try
                {
                    switch (update.Type)
                    {
                        case UpdateType.Message:
                            var message = update.Message;
                            await using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                            {
                                var user = await dbUsr.Users.FindAsync(new object?[]
                                    {
                                        message.Chat.Username ?? message.Chat.Id.ToString()
                                    },
                                    cancellationToken);
                                if (user == null)
                                {
                                    if (message.Chat.Username != null)
                                    {
                                        dbUsr.Users.Add(new Usr(message.Chat.Username, message.Chat.Id,
                                            "Главное меню"));
                                        user = await dbUsr.Users.FindAsync(new object?[] {message.Chat.Username},
                                            cancellationToken);
                                    }
                                    else
                                    {
                                        dbUsr.Users.Add(
                                            new Usr(message.Chat.Id.ToString(), message.Chat.Id, "Главное меню"));
                                        user = await dbUsr.Users.FindAsync(new object?[] {message.Chat.Id.ToString()},
                                            cancellationToken);
                                    }

                                    foreach (var nick in ConstAdmins.AdminsNickname)
                                    {
                                        if (message.Chat.Username == nick)
                                            user.AdminFlag = 1;
                                    }

                                    await dbUsr.SaveChangesAsync(cancellationToken);
                                }

                                if (user.AdminFlag > 0)
                                {
                                    await using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                                    {
                                        var admin = await dbAdmin.Admins.FindAsync(
                                            new object?[] {message.Chat.Username},
                                            cancellationToken);
                                        if (admin == null)
                                        {
                                            dbAdmin.Admins.Add(new Admin(message.Chat.Username, message.Chat.Id));
                                            await dbAdmin.SaveChangesAsync(cancellationToken);
                                        }
                                    }
                                }

                                foreach (var command in commands)
                                {
                                    if (!command.Contains(message)) continue;
                                    if (user.AdminFlag == 0 && command.AdminsCommand == 1)
                                        break;
                                    if (user.AdminFlag > 0 && command.AdminsCommand == 0)
                                    {
                                        user.AdminFlag = 1;
                                        await dbUsr.SaveChangesAsync(cancellationToken);
                                    }

                                    try
                                    {
                                        await command.Execute(message, (TelegramBotClient) botClient);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }

                                    return;
                                }

                                await dbUsr.SaveChangesAsync(cancellationToken);
                            }

                            await Models.Commands.Ok.Execute(update.Message, (TelegramBotClient) botClient);
                            return;


                        case UpdateType.CallbackQuery:
                            foreach (var query in queries)
                            {
                                if (!query.Contains(update)) continue;
                                try
                                {
                                    await query.Execute(update, (TelegramBotClient) botClient);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }

                                break;
                            }

                            break;
                        default:
                            return;
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }
    
    public static void StartBot()
    {
        var bot = Bot.GetBotClient().Result;
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }
}