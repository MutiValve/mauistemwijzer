namespace mauistemwijzer.Models;

public class Party
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Color { get; set; } = "#888888";

    /// <summary>Left-right economic axis. -1 = Links (left), +1 = Rechts (right).</summary>
    public double PoliticalX { get; set; } = 0;

    /// <summary>Progressive-conservative axis. -1 = Conservatief, +1 = Progressief.</summary>
    public double PoliticalY { get; set; } = 0;
}
