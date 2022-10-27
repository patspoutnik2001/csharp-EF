



using System.Diagnostics;

var heroes = new List<Hero>{
    new Hero("Bruce","Wayne","Batman",false),
    new Hero("Tone","Stark","Ironman",true),
    new Hero(String.Empty,String.Empty,"Thanos",false),
    new Hero(String.Empty,String.Empty,"Vision",true),
};

//peut voler
//var res = (List)FiltrerLesHeros(heroes, h => h.canFly);
//var herosQuiPeuventVoler = string.Join(", ", res);
//Console.WriteLine(herosQuiPeuventVoler);

//sans nom
//var res = (List)FiltrerLesHeros(heroes, h => string.IsNullOrEmpty(h.lastName)));
//var herosSansNom = string.Join(", ", res);
//Console.WriteLine(herosSansNom);


//test generique

var resG = Filtrer(new[ ]{1,2,3,4,5,3,2,6,8,9,0,34}, n => n % 2 == 0);

var toPrint = string.Join(",", resG);
//Console.WriteLine(toPrint);

List<Hero> FiltrerLesHeros1(List<Hero> heros, Filtre f)
{
    var result = new List<Hero>();

    foreach (var hero in heros)
    {
        if (f(hero)) result.Add(hero);
    }
    return result;
}



IEnumerable<Hero> FiltrerLesHeros2(IEnumerable<Hero> heroes, Filtre f)
{
    var result = new List<Hero>();

    foreach (var hero in heroes)
    {
        if (f(hero)) result.Add(hero);
    }
    return result;
}
/*
IEnumerable<Hero> FiltrerLesHeros3(IEnumerable<Hero> heros, Filtre f)
{
    foreach (var hero in heros)
    {
        if (f(hero)) yield return(heros);
        //yield permis de creer l'element hero a ajouter dans le IEnumerable
    }
}*/

var resImpaire = Filtrer(new[] { 1, 2, 3, 4, 5, 3, 2, 6, 8, 9, 0, 34 }, n => n % 2 != 0); //-> impere

//Console.WriteLine(string.Join(",", resImpaire));

IEnumerable<T> Filtrer<T>(IEnumerable<T> items, Func<T, bool> f)
{
    foreach (var item in items)
    {
        if(f(item)) yield return item;
        //yield permi s de creer l'element hero a ajouter dans le IEnumerable
    }
}












//Benchmark2(CompterJusqueInfini);
//Console.WriteLine();
//Benchmark2(CompterJusqueInfiniEtAuDela);

//$ => string format + extrapolariation de haine
//Console.WriteLine($"Le resultat est { Benchmark3( ()=>CalculerQuelqueChose() ) }"); // on donne lamda en parametre
//Console.WriteLine($"Le resultat est {Benchmark3( CalculerQuelqueChose )}"); 


void CompterJusqueInfini()
{
    for (int i = 0; i < 1000000000; i++)
    {

    }
}
void CompterJusqueInfiniEtAuDela()
{
    for (int i = 0; i < 1000000000; i++)
    {

    }
}

int CalculerQuelqueChose() 
{
    for (int i = 0; i < 1000000000; i++);
    
    //blhfoefjej
    return 69;
    
}
int CalculerQuelqueChose2(int n)
{

    var facteur = 8;
    return facteur * n;

}

int CalculerQuelqueChose3(int n)
{
    if (n == 0) return 0;
    
    var facteur = 8;
    
    return facteur * CalculerQuelqueChose3(n - 1);
}

Func<int, int> calculatrice = CreerCalculatrice();
Console.WriteLine("Calculatrice: "+calculatrice(45));
Func<int, int> CreerCalculatrice()
{
    var facteur = 8;
    return n => n * facteur;//closure
}



void Benchmark1(UneFxAMesurer fx)
{
    var watch = Stopwatch.StartNew();
    fx();
    watch.Stop();
    Console.WriteLine(watch.Elapsed);
}

void Benchmark2(Action action)// on utlise action parceque il return rien. func doit retourner quelquechose
{
    var watch = Stopwatch.StartNew();
    action();
    watch.Stop();
    Console.WriteLine(watch.Elapsed);
}
//predicat(func) =  un delege qui renvois juste un bool

int Benchmark3(Func<int> fx)
{
    var watch = Stopwatch.StartNew();
    var x = fx();
    watch.Stop();
    Console.WriteLine(watch.Elapsed);
    return x;
}

// lamda: 
// pas de parametre : () ...
// 1 param          : x => ... ou (x) => ...
// +rs param        : (x,y...) => ...

// n => n%2==0
//Action a1 = () => Console.WriteLine("Coucou");
//a1();

//Action<int> a2 = n => Console.WriteLine(n*n);
//a2(2);

//Action<string, string> a3 = (n1, n2) => System.Console.WriteLine(n1+n2);
//a3("pat ","pat");

Func<int> f1 = () => 24;
Console.WriteLine("Print: "+f1());

Func<int, int> f2 = n => n * n;
Console.WriteLine("Multily: "+f2(3));

Func<int, int,bool> f3 = (n1,n2) => n1 == n2;
Console.WriteLine("Are equal: "+f3(3,3));



delegate void UneFxAMesurer();
delegate bool Filtre(Hero hero); //predicat -> le deleget qui renvois un boolean
delegate bool FiltreGenerique<T>(T item);//predicat
record Hero(string firstName, string lastName, string heroName, bool canFly);


//PourComprendreLesClosure CreerCalculatriceInterne()
//{
//    return new PourComprendreLesClosure { facteur = 8 };
//}
//class PourComprendreLesClosure
//{
//    public int facteur;
//    public int Calculatrice(int n) => n * facteur;
//}

