using System.Collections.Generic;
using System.Threading.Tasks;
using mauistemwijzer.Models;

namespace mauistemwijzer.Services;

public interface IStemwijzerService
{
    Task<List<int>> GetQuestionsAsync();
    Task<Question?> GetQuestionByIdAsync(int id);
    Task<List<Party>> GetPartiesAsync();
    Task<ResultaatResponse?> GetResultaatAsync(List<UserAnswer>? answers = null);
    Task RefreshAsync();
}
