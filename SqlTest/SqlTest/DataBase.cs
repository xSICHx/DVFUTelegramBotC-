using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SqlTest;
public class Usr
{
    public string Id { get; set; }//telegram name
    public long ChatId { get; set; } 
    public string Menu { get; set; }
    public int AdminFlag { get; set; } // 0 if not admin; 1 if admin; 2 if admin is about to increase smth money 
    public int AmountOfMoney { get; set; }
    public Dictionary<string, Dictionary<string, int>> TrialsDict { get; set; }
    // public Dictionary<string, int> Dict { get; set; }   

    public Usr(string id, long chatId, string menu)
    {
        Id = id;
        ChatId = chatId;
        Menu = menu;
        AdminFlag = 0;
        AmountOfMoney = 0;
        
        // Dict = new Dictionary<string, int>
        // {
        //     ["12"] = 1,
        //     ["2"] = 0
        // };

        TrialsDict = new Dictionary<string, Dictionary<string, int>>
        {
            ["k"] = new()
            {
                ["utq"] = 1
            },
            ["p"] = new()
            {
                ["sad"] = 2
            },
            ["g"] = new()
            {
                ["utq"] = 3
            }
        };

    }
}


public sealed class ApplicationContext : DbContext
{
    public DbSet<Usr> Users => Set<Usr>();
    public ApplicationContext() => Database.EnsureCreated();
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:/Users/nik1m/Desktop/C#/TelegramBotDVFU/SqlTest/SqlTest/usrs.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usr>()
            .Property(e => e.TrialsDict)
            .IsRequired()
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => 
                    v == null ? new Dictionary<string, Dictionary<string, int>>() // fallback
                    : JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(v)
            );
    }
}