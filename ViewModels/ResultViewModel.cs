using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauistemwijzer.Models;
using mauistemwijzer.Services;

namespace mauistemwijzer.ViewModels;

public class PartyResultItem
{
    public Party Party { get; set; } = null!;
    public double Score { get; set; }           // 0–100
    public bool IsTopMatch { get; set; }
    public string ScoreText => $"{Score:F0}%";
    public double ProgressValue => Score / 100.0;
}

public partial class ResultViewModel : ObservableObject
{
    private readonly StemwijzerState _state;
    private readonly IStemwijzerService _httpService;

    public ObservableCollection<PartyResultItem> PartyResults { get; } = [];

    [ObservableProperty]
    private string topPartyName = string.Empty;

    [ObservableProperty]
    private string topPartyScore = string.Empty;

    [ObservableProperty]
    private bool hasResults;

    public ResultViewModel(StemwijzerState state, IStemwijzerService httpService)
    {
        _state = state;
        _httpService = httpService;
    }

    public async Task LoadResultsAsync()
    {
        var response = await _httpService.GetResultaatAsync(_state.Answers);
        Calculate(response);
    }


    public void Calculate(ResultaatResponse apiResponse)
    {
        PartyResults.Clear();

        if (apiResponse?.Partijen == null || apiResponse.Partijen.Count == 0)
        {
            HasResults = false;
            return;
        }

        var selectedPartyDtos = apiResponse.Partijen
            .Where(p => _state.SelectedPartyIds != null && _state.SelectedPartyIds.Contains(p.Id.ToString()))
            .ToList();

        var answeredQuestions = _state.Answers
            .Where(a => a.Answer != AnswerType.Overgeslagen)
            .ToList();

        var results = new List<PartyResultItem>();

        foreach (var partyDto in selectedPartyDtos)
        {
            int matches = 0;
            int comparisons = 0;

            foreach (var userAnswer in answeredQuestions)
            {
                var partyAnswer = partyDto.Answers.FirstOrDefault(a => a.QuestionId == userAnswer.QuestionId);
                if (partyAnswer == null) continue;

                comparisons++;

                bool isMatch = userAnswer.Answer switch
                {
                    AnswerType.Eens => partyAnswer.Antwoord.Equals("Eens", StringComparison.OrdinalIgnoreCase),
                    AnswerType.Oneens => partyAnswer.Antwoord.Equals("Oneens", StringComparison.OrdinalIgnoreCase),
                    AnswerType.GeenVanBeide => partyAnswer.Antwoord.Equals("Midden", StringComparison.OrdinalIgnoreCase)
                                               || partyAnswer.Antwoord.Equals("Neutraal", StringComparison.OrdinalIgnoreCase),
                    _ => false
                };

                if (isMatch) matches++;
            }

            double score = comparisons > 0 ? Math.Round((matches * 100.0) / comparisons) : 0;

            results.Add(new PartyResultItem
            {
                Party = partyDto.ToParty(),
                Score = score
            });
        }

        results.Sort((a, b) => b.Score.CompareTo(a.Score));

        if (results.Count > 0)
        {
            results[0].IsTopMatch = true;
            TopPartyName = results[0].Party.DisplayName;
            TopPartyScore = results[0].ScoreText;
        }

        HasResults = results.Count > 0;
        foreach (var r in results)
            PartyResults.Add(r);
    }

    [RelayCommand]
    private async Task ShowPoliticalGraphAsync()
    {
        await Shell.Current.GoToAsync("politicalgraph");
    }

    [RelayCommand]
    private async Task RestartAsync()
    {
        _state.Reset();
        await Shell.Current.GoToAsync("//start");
    }
}
