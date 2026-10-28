using mauistemwijzer.Models;

namespace mauistemwijzer.Services;

/// Holds loaded data, selected parties and user answers for the current quiz session.
public class StemwijzerState
{
    public List<int> Questions { get; set; } = [];
    public List<Party> Parties { get; set; } = [];
    public List<string> SelectedPartyIds { get; set; } = [];
    public List<UserAnswer> Answers { get; set; } = [];
    public int CurrentQuestionIndex { get; set; } = 0;

    public void Reset()
    {
        Answers.Clear();
        CurrentQuestionIndex = 0;
        SelectedPartyIds.Clear();
    }
}
