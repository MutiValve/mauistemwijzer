using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mauistemwijzer.Models;

public class StemwijzerData
{
    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("generatedAt")]
    public DateTime GeneratedAt { get; set; }

    [JsonPropertyName("vragen")]
    public List<Vraag> Vragen { get; set; } = [];

    [JsonPropertyName("partijen")]
    public List<Partij> Partijen { get; set; } = [];

    [JsonPropertyName("relaties")]
    public List<Relatie> Relaties { get; set; } = [];
}

public class Vraag
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("onderwerp")]
    public string? Onderwerp { get; set; }

    [JsonPropertyName("beschrijving")]
    public string? Beschrijving { get; set; }

    [JsonPropertyName("spectrum")]
    public string? Spectrum { get; set; }

    [JsonPropertyName("linksRechts")]
    public string? LinksRechts { get; set; }

    [JsonPropertyName("weight")]
    public float Weight { get; set; }

    [JsonPropertyName("partijIds")]
    public List<int> PartijIds { get; set; } = [];
}

public class Partij
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("naam")]
    public string? Naam { get; set; }

    [JsonPropertyName("beschrijving")]
    public string? Beschrijving { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("vraagIds")]
    public List<int> VraagIds { get; set; } = [];
}

public class Relatie
{
    [JsonPropertyName("vraagId")]
    public int VraagId { get; set; }

    [JsonPropertyName("partijId")]
    public int PartijId { get; set; }
}
