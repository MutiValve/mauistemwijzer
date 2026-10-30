//using mauistemwijzer.Models;

//namespace mauistemwijzer.Services;

///// <summary>
///// Hardcoded mock-implementatie van IStemwijzerService.
///// Vervang dit later door een HttpStemwijzerService die dezelfde interface implementeert.
///// </summary>
//public class MockStemwijzerService : IStemwijzerService
//{
//    public Task<List<Party>> GetPartiesAsync()
//    {
//        // PoliticalX: -1 = Links (left) … +1 = Rechts (right)
//        // PoliticalY: -1 = Conservatief … +1 = Progressief
//        var parties = new List<Party>
//        {
//            new() { Id = "vvd",    Name = "Volkspartij voor Vrijheid en Democratie", Abbreviation = "VVD",     Color = "#FF6600", PoliticalX =  0.55, PoliticalY =  0.10 },
//            new() { Id = "pvv",    Name = "Partij voor de Vrijheid",                  Abbreviation = "PVV",     Color = "#003082", PoliticalX =  0.70, PoliticalY = -0.65 },
//            new() { Id = "d66",    Name = "Democraten 66",                            Abbreviation = "D66",     Color = "#00A1DE", PoliticalX =  0.20, PoliticalY =  0.70 },
//            new() { Id = "glpvda", Name = "GroenLinks-PvdA",                          Abbreviation = "GL-PvdA", Color = "#CC0000", PoliticalX = -0.60, PoliticalY =  0.75 },
//            new() { Id = "nsc",    Name = "Nieuw Sociaal Contract",                   Abbreviation = "NSC",     Color = "#1B8A3C", PoliticalX =  0.10, PoliticalY = -0.40 },
//            new() { Id = "bbb",    Name = "BoerBurgerBeweging",                       Abbreviation = "BBB",     Color = "#7CB342", PoliticalX =  0.40, PoliticalY = -0.55 },
//        };
//        return Task.FromResult(parties);
//    }

//    public Task<Question?> GetQuestionByIdAsync(int id)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<List<Question>> GetQuestionsAsync()
//    {
//        // EconomicWeight: +1 = Rechts / market, -1 = Links / state
//        // SocialWeight:   +1 = Progressief, -1 = Conservatief
//        //var questions = new List<Question>
//        //{
//        //    new()
//        //    {
//        //        Id = 1, Category = "Hypotheek",
//        //        Statement = "De hypotheekrenteaftrek moet volledig worden afgeschaft.",
//        //        EconomicWeight = -0.6, SocialWeight = 0.2,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens }, { "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Neutraal },{ "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 2, Category = "Klimaat",
//        //        Statement = "Nederland moet in 2035 volledig klimaatneutraal zijn.",
//        //        EconomicWeight = 0.0, SocialWeight = 0.9,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens }, { "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Neutraal },{ "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 3, Category = "Landbouw",
//        //        Statement = "De overheid moet boeren uitkopen om de stikstofuitstoot te verminderen.",
//        //        EconomicWeight = -0.3, SocialWeight = 0.7,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Neutraal },{ "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Oneens },  { "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 4, Category = "AOW",
//        //        Statement = "De AOW-leeftijd moet worden verlaagd naar 65 jaar.",
//        //        EconomicWeight = -0.5, SocialWeight = -0.4,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens }, { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Oneens }, { "glpvda", PartyPosition.Neutraal },
//        //            { "nsc", PartyPosition.Eens },   { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 5, Category = "Immigratie",
//        //        Statement = "Asielzoekers moeten strenger worden gescreend bij binnenkomst in Nederland.",
//        //        EconomicWeight = 0.4, SocialWeight = -0.8,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },   { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Neutraal },{ "glpvda", PartyPosition.Oneens },
//        //            { "nsc", PartyPosition.Eens },   { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 6, Category = "Onderwijs",
//        //        Statement = "Het collegegeld voor studenten moet worden gehalveerd.",
//        //        EconomicWeight = -0.5, SocialWeight = 0.5,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Neutraal },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Neutraal },{ "bbb", PartyPosition.Neutraal }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 7, Category = "Zorg",
//        //        Statement = "Het eigen risico in de zorg moet volledig worden afgeschaft.",
//        //        EconomicWeight = -0.6, SocialWeight = 0.3,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Neutraal }, { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 8, Category = "Woningmarkt",
//        //        Statement = "De overheid moet fors meer sociale huurwoningen bouwen.",
//        //        EconomicWeight = -0.7, SocialWeight = 0.2,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Neutraal }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 9, Category = "Belasting",
//        //        Statement = "Vermogens boven één miljoen euro moeten zwaarder worden belast.",
//        //        EconomicWeight = -0.8, SocialWeight = 0.1,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens }, { "pvv", PartyPosition.Neutraal },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Neutraal },{ "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 10, Category = "Europa",
//        //        Statement = "De Europese Unie moet meer bevoegdheden krijgen van de lidstaten.",
//        //        EconomicWeight = 0.1, SocialWeight = 0.6,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Neutraal },{ "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Oneens },  { "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 11, Category = "Defensie",
//        //        Statement = "Nederland moet minstens 2% van het bbp uitgeven aan defensie.",
//        //        EconomicWeight = 0.5, SocialWeight = -0.3,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },    { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Neutraal },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 12, Category = "Veiligheid",
//        //        Statement = "Er moeten meer agenten komen in wijken met veel criminaliteit.",
//        //        EconomicWeight = 0.3, SocialWeight = -0.5,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },   { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Eens },   { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 13, Category = "Kernenergie",
//        //        Statement = "Nederland moet nieuwe kerncentrales bouwen.",
//        //        EconomicWeight = 0.4, SocialWeight = -0.4,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },    { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Neutraal }, { "glpvda", PartyPosition.Oneens },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 14, Category = "Minimumloon",
//        //        Statement = "Het minimumloon moet worden verhoogd naar 16 euro per uur.",
//        //        EconomicWeight = -0.7, SocialWeight = 0.2,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 15, Category = "Drugs",
//        //        Statement = "De teelt en verkoop van cannabis moet volledig worden gelegaliseerd.",
//        //        EconomicWeight = 0.0, SocialWeight = 0.8,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Neutraal }, { "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },     { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Oneens },   { "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 16, Category = "Vluchtelingen",
//        //        Statement = "Nederland moet structureel meer vluchtelingen opnemen.",
//        //        EconomicWeight = -0.2, SocialWeight = 0.9,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens }, { "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Oneens }, { "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 17, Category = "Mobiliteit",
//        //        Statement = "De vliegbelasting moet worden verhoogd om vliegen te ontmoedigen.",
//        //        EconomicWeight = -0.1, SocialWeight = 0.7,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Oneens },
//        //            { "d66", PartyPosition.Eens },    { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Neutraal },{ "bbb", PartyPosition.Oneens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 18, Category = "Pensioenen",
//        //        Statement = "De nieuwe pensioenwet (WTP) moet worden teruggedraaid.",
//        //        EconomicWeight = 0.0, SocialWeight = -0.6,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Oneens },  { "pvv", PartyPosition.Neutraal },
//        //            { "d66", PartyPosition.Oneens },  { "glpvda", PartyPosition.Neutraal },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 19, Category = "Privacy",
//        //        Statement = "De overheid mag burgers digitaal volgen om terrorisme te bestrijden.",
//        //        EconomicWeight = 0.3, SocialWeight = -0.7,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },    { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Oneens },  { "glpvda", PartyPosition.Oneens },
//        //            { "nsc", PartyPosition.Eens },    { "bbb", PartyPosition.Neutraal }
//        //        }
//        //    },
//        //    new()
//        //    {
//        //        Id = 20, Category = "Inburgering",
//        //        Statement = "Nieuwkomers moeten verplicht een inburgeringscursus volgen voordat zij een uitkering ontvangen.",
//        //        EconomicWeight = 0.2, SocialWeight = -0.5,
//        //        PartyPositions = new()
//        //        {
//        //            { "vvd", PartyPosition.Eens },   { "pvv", PartyPosition.Eens },
//        //            { "d66", PartyPosition.Eens },   { "glpvda", PartyPosition.Eens },
//        //            { "nsc", PartyPosition.Eens },   { "bbb", PartyPosition.Eens }
//        //        }
//        //    },
//        //};

//        //return Task.FromResult(questions);
//        return null;
//    }

//    public Task<StemwijzerData?> GetRawDataAsync() => Task.FromResult<StemwijzerData?>(null);

//    public Task RefreshAsync() => Task.CompletedTask;

//    Task<List<int>> IStemwijzerService.GetQuestionsAsync()
//    {
//        throw new NotImplementedException();
//    }
//}
