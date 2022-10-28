

using SqlTest;

using (ApplicationContext db = new ApplicationContext())
{
    var user1 = new Usr("gasdn1", 1, "12");
    var user2 = new Usr("lggasdgads",2, "4");
    var user = await db.Users.FindAsync(user1.Id);
    if (user == null){
        db.Users.Add(user1);
    }
    user = await db.Users.FindAsync(user2.Id);
    if (user == null){
        db.Users.Add(user2);
    }
    db.SaveChanges();
    var user11 = await db.Users.FindAsync(new object?[] {"gasdn1"});
    var dict = user11.TrialsDict;
    Console.WriteLine(dict);
    var names = new string[]
    {
        new("10"), new("20"), new("1"), new("ok"), new("21")
    };
    var name = new string("ok0");
    if (names.Contains(name))
        Console.WriteLine(1);
    else
        Console.WriteLine(0);
}