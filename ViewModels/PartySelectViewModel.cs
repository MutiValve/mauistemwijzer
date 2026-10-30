using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauistemwijzer.Models;
using mauistemwijzer.Services;

namespace mauistemwijzer.ViewModels;

public partial class SelectableParty : ObservableObject
{
    public Party Party { get; set; } = null!;

    [ObservableProperty]
    private bool isSelected = true;
}

public partial class PartySelectViewModel : ObservableObject
{
    private readonly StemwijzerState _state;
    private readonly IStemwijzerService httpService; 
    private bool _updatingAll;

    public ObservableCollection<SelectableParty> Parties { get; } = [];

    [ObservableProperty]
    private bool allPartiesSelected = true;

    // Pas de constructor aan zodat Dependency Injection de service meestuurt
    public PartySelectViewModel(StemwijzerState state, IStemwijzerService httpService)
    {
        _state = state;
        this.httpService = httpService;
    }

    // Maak deze async zodat we de API kunnen aanroepen
    public async Task InitializeAsync()
    {
      
        if (_state.Parties == null || _state.Parties.Count == 0)
        {
            _state.Parties = await httpService.GetPartiesAsync();
        }

        Parties.Clear();
        foreach (var party in _state.Parties)
        {
            var sp = new SelectableParty { Party = party, IsSelected = true };
            sp.PropertyChanged += (_, _) => UpdateAllPartiesSelected();
            Parties.Add(sp);
        }
        AllPartiesSelected = true;
    }

    partial void OnAllPartiesSelectedChanged(bool value)
    {
        if (_updatingAll) return;
        _updatingAll = true;
        foreach (var p in Parties)
            p.IsSelected = value;
        _updatingAll = false;
    }

    private void UpdateAllPartiesSelected()
    {
        if (_updatingAll) return;
        AllPartiesSelected = Parties.All(p => p.IsSelected);
    }

    [RelayCommand]
    private async Task ContinueAsync()
    {
        var selected = Parties.Where(p => p.IsSelected).Select(p => p.Party.Id.ToString()).ToList();

        if (selected.Count == 0)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync(
                "Geen partij geselecteerd",
                "Selecteer minimaal één partij om verder te gaan.",
                "OK");
            return;
        }

        _state.SelectedPartyIds = selected;
        _state.CurrentQuestionIndex = 0;
        _state.Answers.Clear();
        await Shell.Current.GoToAsync("question");
    }
}