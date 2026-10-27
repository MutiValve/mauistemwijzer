using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauistemwijzer.Models;
using mauistemwijzer.Services;

namespace mauistemwijzer.ViewModels;

public partial class QuestionViewModel : ObservableObject
{
    private readonly StemwijzerState _state;

    [ObservableProperty]
    private Question? currentQuestion;

    [ObservableProperty]
    private string questionNumberText = string.Empty;

    [ObservableProperty]
    private double progressValue;

    [ObservableProperty]
    private int totalQuestions;

    [ObservableProperty]
    private AnswerType? selectedAnswer;

    public QuestionViewModel(StemwijzerState state)
    {
        _state = state;
        TotalQuestions = _state.Questions.Count;
        LoadCurrentQuestion();
    }

    private void LoadCurrentQuestion()
    {
        if (_state.Questions.Count == 0) return;

        var idx = _state.CurrentQuestionIndex;
        CurrentQuestion = _state.Questions[idx];
        QuestionNumberText = $"Vraag {idx + 1}/{TotalQuestions}";
        ProgressValue = (idx + 1.0) / TotalQuestions;

        // Reset former answer when returning
        var existing = _state.Answers.FirstOrDefault(a => a.QuestionId == CurrentQuestion.Id);
        SelectedAnswer = existing?.Answer;
    }

    private void SaveAnswer(AnswerType answer)
    {
        if (CurrentQuestion is null) return;

        var existing = _state.Answers.FirstOrDefault(a => a.QuestionId == CurrentQuestion.Id);
        if (existing is not null)
            existing.Answer = answer;
        else
            _state.Answers.Add(new UserAnswer { QuestionId = CurrentQuestion.Id, Answer = answer });
    }

    private async Task RecordAndAdvanceAsync(AnswerType answer)
    {
        SelectedAnswer = answer;
        SaveAnswer(answer);
        await Task.Delay(180); 

        if (_state.CurrentQuestionIndex >= TotalQuestions - 1)
        {
            await Shell.Current.GoToAsync("result");
        }
        else
        {
            _state.CurrentQuestionIndex++;
            LoadCurrentQuestion();
        }
    }

    [RelayCommand]
    private Task AnswerEensAsync() => RecordAndAdvanceAsync(AnswerType.Eens);

    [RelayCommand]
    private Task AnswerOneensAsync() => RecordAndAdvanceAsync(AnswerType.Oneens);

    [RelayCommand]
    private Task AnswerGeenVanBeideAsync() => RecordAndAdvanceAsync(AnswerType.GeenVanBeide);

    [RelayCommand]
    private Task OvergeslaanAsync() => RecordAndAdvanceAsync(AnswerType.Overgeslagen);

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (_state.CurrentQuestionIndex > 0)
        {
            _state.CurrentQuestionIndex--;
            LoadCurrentQuestion();
        }
        else
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
