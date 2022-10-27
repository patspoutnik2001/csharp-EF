
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");
var factory = new BrickContextFactory();
using var context = factory.CreateDbContext();
//await AddData();
await QueryData3();
//recup les bricks, dispo et prix de la db

async Task QueryData()
{
    var aviabilityData = await context.BrickAviabilities
        .Include(ba => ba.Brick)// on ajoute dans les bricksaviability la table des Bricks
        .Include(ba=> ba.Vendor)
        .ToArrayAsync();

    foreach (var item in aviabilityData)
    {
        Console.WriteLine($"Brick { item.Brick.Title } aviable at {item.Vendor.VendorName} for { item.PriceEuro }");
    }
}


//recup la liste des bricks avec les vendeurs de cette brique et les tags de cette brique
async Task QueryData2()
{
    var aviabilityData = await context.bricks
        .Include(br => br.Tags)//.Where(t=>t.Title.Contains("Minec")
        //.Where(b=>b.Tags.Any(t=>t.Title.Contains("Ninja")))
        //.Include("Availability.Vendor")//manual string activation path
        //.Include($"{nameof(Brick.Aviabilities)}.{nameof(BrickAviability.Vendor)}")//semi assiste snpp
        .Include(b=>b.Aviabilities).ThenInclude(a=>a.Vendor)
        .ToArrayAsync();

    foreach (var item in aviabilityData)
    {
        Console.WriteLine($"Brick {item.Title}");
        Console.WriteLine($"( {String.Join(',',item.Tags.Select(t=>t.Title))} )");
        Console.WriteLine($"is aviable at {String.Join(",",item.Aviabilities.Select(a=>a.Vendor.VendorName))}");
    }
}
//chargement apres coup
async Task QueryData3()
{
    var simpleBricks = await context.bricks.ToArrayAsync(); 

    foreach (var item in simpleBricks)
    {
        await context.Entry(item).Collection(i => i.Tags).LoadAsync();
        Console.WriteLine($"{item.Title}");
        Console.WriteLine($"( {String.Join(',', item.Tags.Select(t => t.Title))} )");
    }
}

async Task AddData() 
{
    Vendor brickLink, hotBricks;
    await context.AddRangeAsync(new[]
    {
        brickLink= new Vendor(){VendorName="Brick Link"},
        hotBricks= new Vendor(){VendorName="Hot Bricks"},
    });
    await context.SaveChangesAsync();
    Tag rare, ninjago, minecraft;
    await context.AddRangeAsync(new[]
    {
        rare = new Tag(){Title="Rare"},
        ninjago = new Tag(){Title="Ninjago"},
        minecraft = new Tag(){Title="Minecraft"},
    });
    await context.SaveChangesAsync();

    await context.AddAsync(new BasePlate
    {
        Title = "Baseplate 16x16 with blue water pattern",
        Color = Color.green,
        Tags = new() { rare, minecraft },
        Length = 16,
        Width = 16,
        Aviabilities = new()
        {
            new(){Vendor=brickLink,AvailableAmout=5,PriceEuro=6.5m},//m-> decimal-> c# ne arroundi pas les decimal
            new(){Vendor=hotBricks,AvailableAmout=10,PriceEuro=5.9m},
        }
    });
    await context.SaveChangesAsync();
}

enum Color 
{
    red,
    green,
    blue,
    white,
    orange
}


internal class Brick
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string Title { get; set; } = String.Empty;

    public Color? Color { get; set; }

    //une brique peut avoir 0 ou +rs tags

    public List<Tag> Tags { get; set; } = new();

    public List<BrickAviability> Aviabilities { get; set; } = new();
}

class Tag
{
    public int Id { get; set; }
    [MaxLength(250)]

    public string Title { get; set; } = String.Empty;

    //un tag peut etre asocier a 0 ou n brique 
    public List<Brick> Bricks { get; set; } = new();
}


class BasePlate : Brick
{
    public int Length { get; set; }
    public int Width { get; set; }
}

class MinifigHead : Brick
{
    public bool IsDualSided { get; set; }
}

class Vendor
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string VendorName { get; set; } = String.Empty;

    public List<BrickAviability> Availability { get; set; } = new();

}

class BrickAviability
{
    public int Id { get; set; }
    public int AvailableAmout { get; set; }

    [Column(TypeName = "decimal(8,2)")]//pour la db
    public decimal PriceEuro { get; set; }
    public Vendor Vendor { get; set; } //propiete de navigation pour C#

    public int VendorId { get; set; } //cle etrangere pour la DB

    public Brick Brick { get; set; } //pour c#

    public int BrickId { get; set; } //pour db



}


class BrickContext : DbContext
{
    public BrickContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Brick> bricks { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<BrickAviability> BrickAviabilities { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BasePlate>().HasBaseType<Brick>();
        modelBuilder.Entity<MinifigHead>().HasBaseType<Brick>();

    }

}


class BrickContextFactory : IDesignTimeDbContextFactory<BrickContext>
{

    public BrickContext CreateDbContext(string[]? args=null)
    {

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var optionBuilder = new DbContextOptionsBuilder<BrickContext>();
        optionBuilder
            //.UseLoggerFactory(LoggerFactory.Create(optionBuilder => optionBuilder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new BrickContext(optionBuilder.Options);
    }

    
}