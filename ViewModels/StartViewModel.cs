using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauistemwijzer.Services;

namespace mauistemwijzer.ViewModels;

public partial class StartViewModel(IStemwijzerService service, StemwijzerState state) : ObservableObject
{
    [RelayCommand]
    private async Task StartAsync()
    {
        // Pre-load data als dat nog niet gedaan is
        if (state.Questions.Count == 0)
            state.Questions = await service.GetQuestionsAsync();
        if (state.Parties.Count == 0)
            state.Parties = await service.GetPartiesAsync();

        state.Reset();
        await Shell.Current.GoToAsync("partyselect");
    }
}
