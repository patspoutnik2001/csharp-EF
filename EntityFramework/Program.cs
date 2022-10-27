

//Entity Framework : ORM ( Object Relational Mapper ) => liens entre objet et tables
//EF supporte +rs Provider Db : -> MSSQL Server, SQLite,Postgres,MySQL,In Memory(pratique pour test),...
//=> installer package : microsoft entity framework

//on va travailler avec SQL Server Express => on verifie sa presence : sqllocaldb i
// on intalle les tools -> dotnet tool install --global dotnet-ef

//-Recette de cuisine-
//besoin de: DB, tables, +rs approches possibles =>
// code first 
// db first
// model first

//classe Plat

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Net.Mime;

//realiser le lien etre les classes et la DB
//DbContext : version type d'une connnexion a notre DB -> contient tout les points d'entree pour 
// interroger la DB, les tables, ajouter, supprimer, modifier des donnees.

Console.WriteLine("Entity framwork starting");

var factory = new RecettesContextFactory();
//await EntityStates(factory);
//await AttachEntities(factory);
//await RawSQL(factory);
await Transactions(factory);



static async Task Transactions(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var plats = new Plat { Titre="Bob", Notes="Eponge" };

    using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        await context.SaveChangesAsync();
        await context.Database.ExecuteSqlRawAsync("Select 1/0 as CPasBien");
        await transaction.CommitAsync();
    }
    catch (Exception e)
    {
        Console.WriteLine($"Probleme {e.Message} ");
    }


}

static async Task RawSQL(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var plats = await context.Plats
        .FromSqlRaw("SELECT * from Plats")
        .ToArrayAsync();

    var filtre = "%e";
    plats = await context.Plats.FromSqlInterpolated($"SELECT * from Plats where Notes like{filtre}").ToArrayAsync();
}

static async Task NoTracking(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var plats = await context.Plats.AsNoTracking().ToListAsync();

    var state = context.Entry(plats[0]).State;

}


static async Task AttachEntities(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var nouveauPlat = new Plat { Titre = "Pipo", Notes = "Inzhagi" };
    context.Plats.Add(nouveauPlat);
    await context.SaveChangesAsync();

    context.Entry(nouveauPlat).State = EntityState.Deleted;
    context.Plats.Update(nouveauPlat); //? update force un changement d'etat
    await context.SaveChangesAsync();

}

static async Task EntityStates(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var nouveauPlat = new Plat { Titre = "Pipo", Notes = "Inzhagi" };
    var state = context.Entry(nouveauPlat).State;
    //detache-> objet en memeoire mais pas dans la db et inconnu du context
    context.Plats.Add(nouveauPlat);
    state = context.Entry(nouveauPlat).State;
    //ajoute->objet en memoire mais inconnu en DB et connu dans le context
    await context.SaveChangesAsync();
    state = context.Entry(nouveauPlat).State;
    //inchnage-> objet en memoire indentique a la db
    nouveauPlat.Notes = "BLABKLALBBAL";
    state = context.Entry(nouveauPlat).State;
    //modifie -> objet en memoire different de celui en DB
    context.Plats.Remove(nouveauPlat);
    state=context.Entry(nouveauPlat).State;
    //suprime -> objet en memoire supprime par rapport a DB
    await context.SaveChangesAsync();
    state=context.Entry(nouveauPlat).State;


}

static async Task ChangeTracking(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var nouveauPlat = new Plat { Titre = "Pipo", Notes = "Inzhagi" };
    context.Plats.Add(nouveauPlat);
    await context.SaveChangesAsync();
    nouveauPlat.Notes = "loloo";

    var entry = context.Entry(nouveauPlat);
    var originalValue = entry.OriginalValues[nameof(Plat.Notes)].ToString();
}

static async Task ChangeTracking2(RecettesContextFactory factory)
{
    using var context = factory.CreateDbContext();
    var nouveauPlat = new Plat { Titre = "Pipo", Notes = "Inzhagi" };
    context.Plats.Add(nouveauPlat);
    await context.SaveChangesAsync();
    nouveauPlat.Notes = "loloo";

    var entry = context.Entry(nouveauPlat);
    var originalValue = entry.OriginalValues[nameof(Plat.Notes)].ToString();
    var platEnDB = await context.Plats.SingleAsync(p=>p.Id==nouveauPlat.Id);

    using var context2 = factory.CreateDbContext();
    var platEnDb2 = await context.Plats.SingleAsync(p=>p.Id == nouveauPlat.Id);
}

//=> using liberation des ressources a la fermeture de l'app

//Console.WriteLine("On ajouute des cereales ai petit dej");
//var cereales = new Plat 
//{
//    Titre = "Petit dej aux cereales",
//    Notes = "Avec un peu de lait c'est encore meilleur!",
//    Avis = 4
//};


//context.Plats.Add(cereales);//-> on lui passe un pointeur de cereales
//await context.SaveChangesAsync();
//Console.WriteLine("Plat de cereales ajoute!!");

//Console.WriteLine("C'est tellement bon! on mets 5 etoiles");
//cereales.Avis = 5;
//await context.SaveChangesAsync();
//Console.WriteLine("Nouveau avis");

//Console.WriteLine("Nous supprimons des cereales");
//context.Plats.Remove(cereales);
//await context.SaveChangesAsync();
//Console.WriteLine("Plat cereales supprime");



//Console.WriteLine("Verifions l'avis des cereales");
//var plat = await context.Plats.Where(plat => plat.Titre.Contains("cereales"))
//    .ToListAsync();//Linq -> SQL

//if (plat.Count!=1)
//{
//    Console.WriteLine("??? pas de cereales");
//}
//else
//{
//    Console.WriteLine($" le plat de cereales a {plat[0].Avis} etoiles!");
//}


//Console.WriteLine("On ajouute des cereales ai petit dej");
//var pates = new Plat
//{
//    Titre = "Petit dej aux pates saus sauce",
//    Notes = "Avec un peu de lait c'est encore meilleur!",
//    Avis = 1
//};


//context.Plats.Add(pates);
//await context.SaveChangesAsync();
//Console.WriteLine($"Plat de pate {pates.Id} ajoute");



//---------------------------------DEMO 2---------------------------------------------


//var newPlat = new Plat { Titre = "Chips", Notes = "lays" };
//context.Plats.Add(newPlat);
//await context.SaveChangesAsync();
//newPlat.Notes = "carefour";
//await context.SaveChangesAsync();


//eF -> repose sur le patron de conception Unit Of WOrk






#region mes classes

class RecettesContext : DbContext
{

    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Plat> Plats { get; set; }

    public RecettesContext(DbContextOptions<RecettesContext> options) : base(options)
    {

    }
}

//Fabrique de classe

class RecettesContextFactory : IDesignTimeDbContextFactory<RecettesContext>
{
    public RecettesContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var optionBuilder = new DbContextOptionsBuilder<RecettesContext>();
        optionBuilder
            //.UseLoggerFactory(LoggerFactory.Create(optionBuilder => optionBuilder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new RecettesContext(optionBuilder.Options);
    }
}

class Plat 
{
    public int Id { get; set; } //clee primaire par convention
    [MaxLength(100)]
    public string Titre { get; set; } = String.Empty;
    [MaxLength(1000)]
    public string? Notes { get; set; } //? => autorise null

    public int? Avis { get; set; } //? => autorise null 

    public List<Ingredient> Ingredients { get; set; } = new();
}

//classe Ingredient

class Ingredient 
{
    public int Id { get; set; }
    [MaxLength(100)]

    public string Description { get; set; } = string.Empty;

    public string UniteDeMesure { get; set; } = string.Empty;

    [Column(TypeName ="decimal(5,2)")] // (5,2) : 5 chiffres dont 2 apres la virgule
    public decimal Quantite { get; set; }

    //cle etrangere vers plat
    public int PlatId { get; set; }
    //propriete de navigation
    public Plat? plat { get; set; }
}

#endregion