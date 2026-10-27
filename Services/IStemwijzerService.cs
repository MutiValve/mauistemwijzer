using mauistemwijzer.Models;

namespace mauistemwijzer.Services;

public interface IStemwijzerService
{
    Task<List<Question>> GetQuestionsAsync();
    Task<List<Party>> GetPartiesAsync();
}
