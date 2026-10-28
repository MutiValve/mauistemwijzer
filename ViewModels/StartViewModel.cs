using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauistemwijzer.Services;

namespace mauistemwijzer.ViewModels;

public partial class StartViewModel(IStemwijzerService service, StemwijzerState state) : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [RelayCommand]
    private async Task StartAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        try
        {
            // Pre-load data 
            if (state.Questions.Count == 0)
                state.Questions = await service.GetQuestionsAsync();
            if (state.Parties.Count == 0)
                state.Parties = await service.GetPartiesAsync();

            if (state.Questions.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Geen vragen", "Er zijn geen vragen ontvangen van de API.", "OK");
                return;
            }

            if (state.Parties.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Geen partijen", "Er zijn geen politieke partijen ontvangen van de API.", "OK");
                return;
            }

            state.Reset();
            await Shell.Current.GoToAsync("partyselect");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Fout bij ophalen API", $"Kon de gegevens niet ophalen van de REST API:\n\n{ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
