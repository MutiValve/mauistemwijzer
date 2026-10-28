using mauistemwijzer.Models;
using System.Text.Json.Serialization;

namespace mauistemwijzer.Services;

public partial class HttpStemwijzerService
{
    public class QuestionApiResponse
    {
        [JsonPropertyName("vraag")]
        public Question Vraag { get; set; } = new();

        [JsonPropertyName("partijen")]
        public List<PartyAnswerRelation> Partijen { get; set; } = [];
    }

}