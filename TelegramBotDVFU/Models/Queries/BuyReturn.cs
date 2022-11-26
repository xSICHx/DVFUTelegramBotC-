using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.Models.Queries;
using TelegramBotDVFU.View.Products;

namespace TelegramBotDVFU.Models.Commands;

public class BuyReturn : Query
{
    public override string[] Names { get; }

    public override void Execute(Update update)
    {
        var query = update.CallbackQuery;
        var chatId = query.From.Id;
        string? dataProduct;
        Product product;
        switch (query.Data)
        {
            case var data when new Regex(@"Купить \w*").IsMatch(data):
                dataProduct = query.Data[("Купить ".Length)..];
                
                product = null;
                 using (ApplicationProductContext dbProduct = new ApplicationProductContext())
                {
                    foreach (var prdct in dbProduct.Products)
                    {
                        if (prdct.Name != dataProduct) continue;
                        product = prdct;
                        break;
                    }
                    //sssssssss
                    if (product == null)
                        Console.WriteLine(dataProduct);
                    //asdasdads
                     using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                    {
                        var user = dbUsr.Users.Find(query.From.Username);
                        if (user.AmountOfMoney >= product.Cost)
                        {
                            if (product.Amount <= 0)
                            {
                                // await botClient.SendTextMessageAsync(chatId, "Увы, товар "
                                //                                              + product.Name + " закончился");
                                break;
                            }

                            user.ProductsPurchaced[product.Name]++;
                            user.AmountOfMoney -= product.Cost;
                            product.Amount -= 1;
                            dbProduct.Update(product);
                             dbProduct.SaveChanges();
                            dbUsr.Update(user);
                             dbUsr.SaveChanges();

                            // await botClient.SendTextMessageAsync(chatId,
                            //     "Вы приобрели товар "
                            //     + product.Name +
                            //     ". Чтобы получить его в реальности, подойдите к организатору." +
                            //     "\nТекущий баланс: " + user.AmountOfMoney);
                        }
                        else
                        {
                            // await botClient.SendTextMessageAsync(chatId, "Недостаточно средств для покупки "
                            //                                              + product.Name);
                        }
                    }
                }

                break;
            case var data when new Regex(@"Вернуть \w*").IsMatch(data):
                dataProduct = query.Data[("Вернуть ".Length)..];
                product = null;
                 using (ApplicationProductContext dbProduct = new ApplicationProductContext())
                {
                    foreach (var prdct in dbProduct.Products)
                    {
                        if (prdct.Name == dataProduct)
                        {
                            product = prdct;
                            break;
                        }
                    }

                 
                     using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                    {
                        var user = dbUsr.Users.Find(new object?[] {query.From.Username});
                        if (user.ProductsPurchaced[product.Name] > 0)
                        {
                            user.ProductsPurchaced[product.Name]--;
                            user.AmountOfMoney += product.Cost;
                            product.Amount++;
                            dbProduct.Update(product);
                            dbProduct.SaveChanges();
                            dbUsr.Update(user);
                             dbUsr.SaveChanges();
                            // await botClient.SendTextMessageAsync(chatId,
                            //     "Вы вернули товар "
                            //     + product.Name +
                            //     ". Текущий баланс: " + user.AmountOfMoney);
                        }
                        else
                        {
                            // await botClient.SendTextMessageAsync(chatId,
                            //     "Вы не можете вернуть " + product.Name + ", так как у вас нет этого товара");
                        }

                        break;
                    }
                }
        }
    }

    public override bool Contains(Update update)
    {
        if (update.Type != UpdateType.CallbackQuery)
            return false;
        var data = update.CallbackQuery.Data;
        return new Regex(@"Купить \w*").IsMatch(data) || new Regex(@"Вернуть \w*").IsMatch(data);
    }
}