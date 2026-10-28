using mauistemwijzer.Models;

namespace mauistemwijzer.Services;

public interface IStemwijzerService
{
    Task<List<int>> GetQuestionsAsync();

    Task<Question?> GetQuestionByIdAsync(int id);
    Task<List<Party>> GetPartiesAsync();
    Task RefreshAsync();
}
