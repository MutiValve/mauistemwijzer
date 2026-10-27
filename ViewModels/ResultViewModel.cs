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

    public ObservableCollection<PartyResultItem> PartyResults { get; } = [];

    [ObservableProperty]
    private string topPartyName = string.Empty;

    [ObservableProperty]
    private string topPartyScore = string.Empty;

    [ObservableProperty]
    private bool hasResults;

    public ResultViewModel(StemwijzerState state)
    {
        _state = state;
    }

    /// Berekent de score per partij. Roep aan vanuit ResultPage.OnAppearing.
    /// Scoring: Eens+Eens of Oneens+Oneens = match (+1). Neutraal of Overgeslagen telt niet mee.

    public void Calculate()
    {
        PartyResults.Clear();

        var selectedParties = _state.Parties
            .Where(p => _state.SelectedPartyIds.Contains(p.Id))
            .ToList();

        var answeredQuestions = _state.Answers
            .Where(a => a.Answer != AnswerType.Overgeslagen)
            .ToList();

        var results = new List<PartyResultItem>();

        foreach (var party in selectedParties)
        {
            int matches = 0;
            int comparisons = 0;

            foreach (var answer in answeredQuestions)
            {
                var question = _state.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question is null) continue;
                if (!question.PartyPositions.TryGetValue(party.Id, out var partyPos)) continue;
                if (partyPos == PartyPosition.Neutraal) continue;

                comparisons++;

                bool isMatch =
                    (answer.Answer == AnswerType.Eens   && partyPos == PartyPosition.Eens)   ||
                    (answer.Answer == AnswerType.Oneens && partyPos == PartyPosition.Oneens)  ||
                    (answer.Answer == AnswerType.GeenVanBeide); // neutraal telt als half-match

                if (isMatch) matches++;
            }

            double score = comparisons > 0 ? Math.Round(matches * 100.0 / comparisons) : 0;
            results.Add(new PartyResultItem { Party = party, Score = score });
        }

        results.Sort((a, b) => b.Score.CompareTo(a.Score));

        if (results.Count > 0)
        {
            results[0].IsTopMatch = true;
            TopPartyName = results[0].Party.Abbreviation;
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
