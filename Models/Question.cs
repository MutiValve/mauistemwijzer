using System.Text.Json.Serialization;

namespace mauistemwijzer.Models;

public enum PartyPosition
{
    Eens,
    Oneens,
    Neutraal
}

public class Question
{
    [JsonPropertyName("idVragen")]
    public int Id { get; set; }

    [JsonPropertyName("Onderwerp")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("Beschrijving")]
    public string Statement { get; set; } = string.Empty;

    [JsonPropertyName("Conservatief/Midden/Progressief")]
    public string Spectrum { get; set; } = string.Empty;

    [JsonPropertyName("Links/Rechts")]
    public string LinksRechts { get; set; } = string.Empty;

    [JsonPropertyName("weight")]
    public float Weight { get; set; } = 0;
}