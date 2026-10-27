namespace mauistemwijzer.Models;

public enum PartyPosition
{
    Eens,
    Oneens,
    Neutraal
}

public class Question
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Statement { get; set; } = string.Empty;
    public Dictionary<string, PartyPosition> PartyPositions { get; set; } = [];

    /// <summary>
    /// How much an "Eens" answer pushes along the economic (X) axis.
    /// Positive = Rechts, negative = Links. Range [-1, 1].
    /// </summary>
    public double EconomicWeight { get; set; } = 0;

    /// <summary>
    /// How much an "Eens" answer pushes along the social (Y) axis.
    /// Positive = Progressief, negative = Conservatief. Range [-1, 1].
    /// </summary>
    public double SocialWeight { get; set; } = 0;
}
