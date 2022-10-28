namespace TelegramBotDVFU.View.Products;

public class Product
{
    public string Name;
    public string Description;
    public int Amount;
    public int Cost;

    public Product(string name, string description, int amount, int cost)
    {
        Name = name;
        Description = description;
        Amount = amount;
        Cost = cost;
    }
}