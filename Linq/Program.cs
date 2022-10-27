int[] numbers = {3,4,7,9,234,65,2,12 };


//Query syntax:

IEnumerable<int> query1 =
    from num in numbers
    where num % 2 == 0
    orderby num
    select num;

Console.WriteLine(String.Join(" - ",query1));


//methode syntax

IEnumerable<int> query2 = numbers.Where(num => num % 2 == 0).OrderBy(n => n);
Console.WriteLine(String.Join(" ~ ", query2));

//source manipulation avec LINQ =>
// collections d;objests fortement types : linq to objects
//fichiers XML : LINQ to XML
//ado.net : linq to dataset
//entity framework : linq to entities

// IEnumerable => IEnumerator => MoveNext


//var res = GenererNombres1(10).Where(n => n>5);
//Console.WriteLine(String.Join(" ~ ", res));

var res = GenererNombres1(10)//systeme pull-based 
    .Where(n =>
    {
        return n > 5;
    })
    .Select(n => 
    {
        return n * 3;
    });

foreach (var item in res)
{
    Console.WriteLine(item);
}

//Console.WriteLine(String.Join(" ~ ", res));


IEnumerable<int> GenererNombres1(int maxValue) 
{
    var res = new List<int>();
    for (int i = 0; i < maxValue; i++)
    {
        res.Add(i);
    }
    return res;
}

IEnumerable<int> GenererNombres2(int maxValue)
{
    for (int i = 0; i < maxValue; i++)
    {
        yield return i;
    }
}



