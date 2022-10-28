using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotDVFU.Models;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View.Products;

namespace TelegramBotDVFU.Controllers;

public class MessageController : Controller
{
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine("\n" + Newtonsoft.Json.JsonConvert.SerializeObject(update));

        var commands = Bot.Commands;
        switch (update.Type)
        {
            case UpdateType.Message:
                var message = update.Message;
                await using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                {
                    var user = await dbUsr.Users.FindAsync(new object?[] {message.Chat.Username}, cancellationToken);
                    if (user == null)
                    {
                        dbUsr.Users.Add(new Usr(message.Chat.Username, message.Chat.Id, "Главное меню"));
                        user = await dbUsr.Users.FindAsync(new object?[] {message.Chat.Username}, cancellationToken);
                        await dbUsr.SaveChangesAsync(cancellationToken);
                    }

                    if (user.AdminFlag > 0)
                    {
                        await using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                        {
                            var admin = await dbAdmin.Admins.FindAsync(new object?[] {message.Chat.Username},
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

                        await command.Execute(message, (TelegramBotClient) botClient);
                        return;
                    }

                    await dbUsr.SaveChangesAsync(cancellationToken);
                }

                await Models.Commands.Ok.Execute(update.Message, (TelegramBotClient) botClient);
                return;


            case UpdateType.CallbackQuery:
                var query = update.CallbackQuery;
                var chatId = query.From.Id;
                string? dataProduct;
                Product product;
                switch (query.Data)
                {
                    case var data when new Regex(@"Купить \w*").IsMatch(data):
                        dataProduct = query.Data.Split(new[] {' '})[1];
                        product = null;
                        for (int i = 0; i < ConstAssortiment.Products.Length; i++)
                        {
                            if (ConstAssortiment.Products[i].Name == dataProduct)
                            {
                                product = ConstAssortiment.Products[i];
                                break;
                            }
                        }

                        await using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                        {
                            var user = await dbUsr.Users.FindAsync(new object?[] {query.From.Username},
                                cancellationToken);
                            if (user.AmountOfMoney >= product.Cost)
                            {
                                if (product.Amount <= 0)
                                {
                                    await botClient.SendTextMessageAsync(chatId, "Увы, товар "
                                        + product.Name + " закончился");
                                    break;
                                }
                                //todo показать, сколько куплено у чела предметов + добавить команду админа для выдачи айтема

                                user.ProductsPurchaced[product.Name]++;
                                user.AmountOfMoney -= product.Cost;
                                product.Amount--;
                                dbUsr.Update(user);
                                await dbUsr.SaveChangesAsync();

                                await botClient.SendTextMessageAsync(chatId,
                                    "Вы приобрели товар "
                                    + product.Name +
                                    ". Чтобы получить его в реальности, подойдите к администратору." +
                                    "\nТекущий баланс: " + user.AmountOfMoney);
                            }
                            else
                                await botClient.SendTextMessageAsync(chatId, "Недостаточно средств для покупки "
                                                                             + product.Name);
                        }

                        break;
                    case var data when new Regex(@"Вернуть \w*").IsMatch(data):
                        dataProduct = query.Data.Split(new[] {' '})[1];
                        product = null;
                        for (int i = 0; i < ConstAssortiment.Products.Length; i++)
                        {
                            if (ConstAssortiment.Products[i].Name == dataProduct)
                            {
                                product = ConstAssortiment.Products[i];
                                break;
                            }
                        }

                        await using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                        {
                            var user = await dbUsr.Users.FindAsync(new object?[] {query.From.Username},
                                cancellationToken);
                            if (user.ProductsPurchaced[product.Name] > 0)
                            {
                                user.ProductsPurchaced[product.Name]--;
                                user.AmountOfMoney += product.Cost;
                                product.Amount++;
                                dbUsr.Update(user);
                                await dbUsr.SaveChangesAsync();
                                await botClient.SendTextMessageAsync(chatId,
                                    "Вы вернули товар "
                                    + product.Name +
                                    ". Текущий баланс: " + user.AmountOfMoney);
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(chatId,
                                    "Вы не можете вернуть " + product.Name + ", так как у вас нет этого товара");
                            }

                            break;
                        }
                }
                break;
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