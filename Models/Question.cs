using System.Text.Json.Serialization;

namespace mauistemwijzer.Models;

public enum PartyPosition
{
    Eens,
    Oneens,
    Midden
}

public class Question
{
    [JsonPropertyName("idVragen")]
    public int Id { get; set; }

    [JsonPropertyName("Onderwerp")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("Beschrijving")]
    public string Statement { get; set; } = string.Empty;

    [JsonPropertyName("Spectrum")]
    public string Spectrum { get; set; } = string.Empty;

    [JsonPropertyName("LinksRechts")]
    public string LinksRechts { get; set; } = string.Empty;

    [JsonPropertyName("weight")]
    public float Weight { get; set; } = 0;
}