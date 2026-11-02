using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mauistemwijzer.Models;

public class ResultaatResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("userPosition")]
    public PartyPositionCoords? UserPosition { get; set; }

    [JsonPropertyName("userAnsweredQuestions")]
    public int UserAnsweredQuestions { get; set; }

    [JsonPropertyName("totalQuestions")]
    public int TotalQuestions { get; set; }

    [JsonPropertyName("partijen")]
    public List<ResultPartyDto> Partijen { get; set; } = [];
}

public class PartyAnswerDto
{
    [JsonPropertyName("vraagId")]
    public int QuestionId { get; set; }

    [JsonPropertyName("antwoord")]
    public string Antwoord { get; set; } = string.Empty;
}

public class ResultPartyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("naam")]
    public string? Name { get; set; }

    [JsonPropertyName("beschrijving")]
    public string? Description { get; set; }

    [JsonPropertyName("imagePath")]
    public string? ImagePath { get; set; }

    [JsonPropertyName("imageName")]
    public string? ImageName { get; set; }

    [JsonPropertyName("answers")]
    public List<PartyAnswerDto> Answers { get; set; } = [];

    [JsonPropertyName("position")]
    public PartyPositionCoords? Position { get; set; }

    [JsonPropertyName("answeredQuestions")]
    public int AnsweredQuestions { get; set; }

    [JsonPropertyName("totalQuestions")]
    public int TotalQuestions { get; set; }

    // API DATA -> Party model
    public Party ToParty()
    {
        return new Party
        {
            Id = Id,
            Name = Name ?? string.Empty,
            Description = Description ?? string.Empty,
            ImagePath = ImagePath,
            Position = Position
        };
    }
}