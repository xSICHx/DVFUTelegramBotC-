using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View.Products;

namespace TelegramBotDVFU.Models;

public class Usr
{
    public string Id { get; set; }//telegram name
    public long ChatId { get; set; } 
    public string Menu { get; set; }
    public int AdminFlag { get; set; } // 0 if not admin; 1 if admin; 2 if admin is about to increase smth money 
    public int AmountOfMoney { get; set; }
    public Dictionary<string, int> TrialsDict { get; set; }
    public Dictionary<string, int> ProductsPurchaced;

    public Usr(string id, long chatId, string menu)
    {
        Id = id;
        ChatId = chatId;
        Menu = menu;
        AdminFlag = 0;
        AmountOfMoney = 0;
        //todo переделать эту хуету
        TrialsDict = new Dictionary<string, int>();
        foreach (var trial in ConstTrials.TrialsList)
        {
            TrialsDict.Add(trial.Name, 0);
        }
        
        ProductsPurchaced = new Dictionary<string, int>();
        using (var dbProduct = new ApplicationProductContext())
        {
            foreach (var product in dbProduct.Products)
            {
                ProductsPurchaced.Add(product.Name, 0);
            }
            dbProduct.SaveChanges();
        }
        // foreach (var product in ConstAssortiment.Products)
        // {
        //     ProductsPurchaced.Add(product.Name, 0);
        // }
    }
}


public sealed class ApplicationUserContext : DbContext
{
    public DbSet<Usr> Users => Set<Usr>();
    public ApplicationUserContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=users_db.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usr>()
            .Property(e => e.TrialsDict)
            .IsRequired()
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => 
                    v == null ? new Dictionary<string, int>() // fallback
                        : JsonConvert.DeserializeObject<Dictionary<string, int>>(v)
            );
        modelBuilder.Entity<Usr>()
            .Property(e => e.ProductsPurchaced)
            .IsRequired()
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => 
                    v == null ? new Dictionary<string, int>() // fallback
                        : JsonConvert.DeserializeObject<Dictionary<string, int>>(v)
            );
    }
}


public class Admin
{
    public string AdminId { get; set; }//telegram name
    public long ChatAdminId { get; set; }
    public string UserId { get; set; }//telegram name
    public long ChatUserId { get; set; }
    public string[] CurrentUserTrial = new string[2];
    
    public Admin(string adminId, long chatAdminId)
    {
        AdminId = adminId;
        ChatAdminId = chatAdminId;
        UserId = "0";
        ChatUserId = 0;
        CurrentUserTrial[0] = "0";
        CurrentUserTrial[1] = "0";
    }
}

public sealed class ApplicationAdminContext : DbContext
{
    public DbSet<Admin> Admins => Set<Admin>();
    public ApplicationAdminContext() => Database.EnsureCreated();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=admins_db.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().HasKey(admin => admin.AdminId);
        modelBuilder.Entity<Admin>()
            .Property(e => e.CurrentUserTrial)
            .IsRequired()
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                u => JsonConvert.DeserializeObject<string[]>(u));
    }
}


public sealed class ApplicationProductContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public ApplicationProductContext() => Database.EnsureCreated();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=product_db.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(product => product.Name);
    }
}