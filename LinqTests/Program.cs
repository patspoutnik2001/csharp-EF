using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;  //json from MS => à utiliser
//NewtonSoft => ne plus utiliser.
using System.Text.Json.Serialization;

var fileContent = await File.ReadAllTextAsync("MOCK_DATA.json"); //lecture du contenu du json et stockage dans la variable
var voitures = JsonSerializer.Deserialize<Voiture[]>(fileContent);

//var voitureAvecAuMoinsQuatresPortes = voitures.Where(v => v.NombreDePortes >= 4 && v.Constructeur.Equals("Mazda"));
//voitureAvecAuMoinsQuatresPortes = voitures.Where(v => v.NombreDePortes >= 4)
//                                          .Where(v => v.Constructeur.Equals("Mazda"));

//foreach (var voiture in voitureAvecAuMoinsQuatresPortes)
//{
//    Console.WriteLine($"La voiture {voiture.Modele} a {voiture.NombreDePortes} portes.");
//}

//afficher les marques + modèles pour les constructeurs qui commencent par "M"
//voitures.Where(v => v.Constructeur.StartsWith("M"))
//                                      .Select(v => $"{v.Constructeur} {v.Modele}")
//                                      .ToList()
//                                      .ForEach(v => Console.WriteLine(v));

//foreach (var voiture in marquesPlusModelesAvecM)
//{
//    Console.WriteLine($"{voiture.Constructeur} {voiture.Modele}");
//}

//foreach (var voiture in marquesPlusModelesAvecM)
//{
//    Console.WriteLine($"{voiture}");
//}

//afficher une liste des 10 voitures les plus puissantes


//voitures.OrderByDescending(v => v.Puissance)
//        //.Skip(980) //on skip les 980 premiers --> pagination
//        .Take(10)
//        .Select(v => $"{v.Constructeur} {v.Modele} {v.Puissance}")
//        .ToList()
//        .ForEach(v => Console.WriteLine(v)); //take permet de récupérer les x premiers éléments de la collections

//afficher le nombre de modèles par marque construits après 1995 

// ==> utiliser un groupBy( v => v.Constructeur )

//voitures.GroupBy(v => v.Constructeur)
//        .ToList()
//        .ForEach(item => Console.WriteLine(item.Key));

//voitures//.Where(v=>v.Annee>1995)
//    .GroupBy(v => v.Constructeur)
//    //.Select(v => new { v.Key, NombreDeModeles = v.Where(v=>v.Annee>1995).Count() })
//    .Select(v => new { v.Key, NombreDeModeles = v.Count(v => v.Annee > 1995),voit = v.Select(v=>v) })
//    .ToList()
//    .ForEach(item => Console.WriteLine($" {item.Key} : {item.NombreDeModeles} {string.Join(" - "),.voit.Select(v => v.Modele))} "));

//voitures.Where(v => v.Puissance >= 400)
//    .GroupBy(v => v.Constructeur)
//    .Select(v => new { Constructeur = v.Key, NombreDeVoituresPuissantes = v.Count() })
//    .Where(constructeur => constructeur.NombreDeVoituresPuissantes >= 2)
//    .ToList()
//    .ForEach(constructeur => Console.WriteLine($"{constructeur.Constructeur} : {constructeur.NombreDeVoituresPuissantes}"));

//afficher la puissance moyenne par constructeur

//voitures.GroupBy(v => v.Constructeur)
//        .Select(v => new { Constructeur = v.Key, PuissanceMoyenne = v.Average(v => v.Puissance) })
//        .ToList()
//        .ForEach(constructeur => Console.WriteLine($"{constructeur.Constructeur}:{constructeur.PuissanceMoyenne}"));

//afficher le nombre de constructeur par tranches de puissance 0->100, 101->200,201->300,301->400,401->500

voitures.GroupBy(v => v.Puissance switch
{
    <= 100 => "0..100",
    <= 200 => "101..200",
    <= 300 => "201..300",
    <= 400 => "301..400",
    _=> "401..", //_ -> defaut? >:D
})
    .OrderBy(gv => gv.Key)
    .Select(v => new { HPCategory = v.Key,
        NombreDeConstructeurs = v.Select(v => v.Constructeur).Distinct().Count() })
    .ToList()
    .ForEach(item => Console.WriteLine($"{item.HPCategory} : {item.NombreDeConstructeurs}"));
        



class Voiture
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("car_make")]
    public string Constructeur { get; set; }

    [JsonPropertyName("car_model")]
    public string Modele { get; set; }

    [JsonPropertyName("car_year")]
    public int Annee { get; set; }

    [JsonPropertyName("number_of_doors")]
    public int NombreDePortes { get; set; }

    [JsonPropertyName("hp")]
    public int Puissance { get; set; }

}