// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

Console.WriteLine("Hello, patryk!");


var factory = new PeopleContextFactory();
var dbContext = factory.CreateDbContext();

await LoadSampleData(dbContext);
//Query1(dbContext);
//Query2(dbContext);

BenchmarkRunner.Run<BenchEF>();

async Task LoadSampleData(PeopleContext _db)
{
    if (_db.People.Count()==0)
    {
        string file = await System.IO.File.ReadAllTextAsync("generated.json");
        var people = JsonSerializer.Deserialize<List<Person>>(file);
        await _db.AddRangeAsync(people);
        await _db.SaveChangesAsync();
        Console.WriteLine("Peoples added ");
    }
    
}




bool ApprovedAge(int age)
{
    return  age >= 18 && age <= 65;
}




public class BenchEF
{
    [Benchmark]
    public void Query1()
    {
        var factory = new PeopleContextFactory();
        var dbContext = factory.CreateDbContext();
        var people = dbContext.People
            .Include(a => a.Addresses)
            .Include(b => b.EmailAddresses)
            .Where(x => x.Age >= 18 && x.Age <= 65)//DB qui fait le JOb
            .ToList();
    }

    [Benchmark]
    public void Query2()
    {
        var factory = new PeopleContextFactory();
        var dbContext = factory.CreateDbContext();
        var people = dbContext.People
            .Include(a => a.Addresses)
            .Include(b => b.EmailAddresses)
            .ToList()
            .Where(x => x.Age >= 18 && x.Age <= 65);//DB qui fait le JOb
            //.Where(x => ApprovedAge(x.Age));// C# qui fait le JoB
    }
}