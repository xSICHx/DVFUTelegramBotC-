namespace TelegramBotDVFU.View.Products;

public class Product
{
    public string Name { get; set; }

    public string Description { get; set; }
    public int Amount { get; set; }
    public int Cost { get; set; }

    public Product(string name, string description, int amount, int cost)
    {
        Name = name;
        Description = description;
        Amount = amount;
        Cost = cost;
    }
}