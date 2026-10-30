using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace mauistemwijzer.Models;

public class Party
{
    [JsonPropertyName("idPartij")]
    public int Id { get; set; }

    [JsonPropertyName("Naampartij")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string DisplayName => !string.IsNullOrWhiteSpace(Name) ? Name : Abbreviation;

    [JsonPropertyName("Beschrijving")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("idImage")]
    public int? ImageId { get; set; }

    [JsonPropertyName("image_path")]
    public string? ImagePath { get; set; }

    [JsonPropertyName("logo")]
    public string? LogoRaw { get; set; }

    [JsonPropertyName("partijLeider_id")]
    public int? LeaderId { get; set; }

    private string? _abbreviation;
    [JsonIgnore]
    public string Abbreviation
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_abbreviation))
                return _abbreviation;
            if (!string.IsNullOrWhiteSpace(Name))
                return DeriveAbbreviation(Name);
            return "?";
        }
        set => _abbreviation = value;
    }

    private string _color = "#888888";
    [JsonIgnore]
    public string Color
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_color) && _color != "#888888")
                return _color;
            return GeneratePaletteColor(Id);
        }
        set => _color = value;
    }

    [JsonIgnore]
    public string? Logo
    {
        get
        {
            var raw = !string.IsNullOrWhiteSpace(LogoRaw) ? LogoRaw : ImagePath;
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            if (raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return raw;

            // Prepend local backend base URL for relative upload paths
            return $"http://localhost/programming/stemwijzer/{raw.TrimStart('/')}";
        }
        set => LogoRaw = value;
    }

    [JsonIgnore]
    public bool HasLogo => !string.IsNullOrWhiteSpace(Logo);

    [JsonIgnore]
    public bool HasNoLogo => string.IsNullOrWhiteSpace(Logo);

    // Politieke assen (worden gevuld via de resultaat-API of berekening)
    [JsonPropertyName("position")]
    public PartyPositionCoords? Position { get; set; }

    [JsonIgnore]
    public double PoliticalX => Position?.X ?? 0;

    [JsonIgnore]
    public double PoliticalY => Position?.Y ?? 0;

    private static string DeriveAbbreviation(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "?";

        int openParen = fullName.IndexOf('(');
        int closeParen = fullName.IndexOf(')');
        if (openParen >= 0 && closeParen > openParen)
        {
            string inside = fullName.Substring(openParen + 1, closeParen - openParen - 1).Trim();
            if (!string.IsNullOrEmpty(inside) && inside.Length <= 8)
                return inside;
        }

        if (fullName.Length <= 6)
            return fullName;

        var words = fullName.Split(new[] { ' ', '-', '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length > 1)
        {
            return new string(words.Take(5).Select(w => char.ToUpperInvariant(w[0])).ToArray());
        }

        return fullName.Length > 4 ? fullName[..4] : fullName;
    }

    private static string GeneratePaletteColor(int id)
    {
        string[] palette =
        [
            "#E65100", "#1565C0", "#2E7D32", "#C2185B",
            "#6A1B9A", "#00838F", "#D84315", "#4527A0",
            "#00695C", "#AD1457", "#283593", "#4E342E",
            "#0277BD", "#558B2F", "#F57F17", "#37474F",
            "#880E4F", "#004D40", "#BF360C", "#311B92"
        ];
        return palette[Math.Abs(id) % palette.Length];
    }
}

public class PartyPositionCoords
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }
}
