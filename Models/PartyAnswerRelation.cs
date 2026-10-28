using mauistemwijzer.Models;
using System.Text.Json.Serialization;

namespace mauistemwijzer.Services;

public partial class HttpStemwijzerService
{
    public class PartyAnswerRelation
    {
        [JsonPropertyName("idPartij")]
        public int Id { get; set; }

        [JsonPropertyName("Naampartij")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Beschrijving")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("idImage")]
        public int? ImageId { get; set; }

        [JsonPropertyName("Antwoord")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PartyPosition Answer { get; set; }
    }

}