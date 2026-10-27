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
    private bool _updatingAll;

    public ObservableCollection<SelectableParty> Parties { get; } = [];

    [ObservableProperty]
    private bool allPartiesSelected = true;

    public PartySelectViewModel(StemwijzerState state)
    {
        _state = state;
    }


    public void Initialize()
    {
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
        var selected = Parties.Where(p => p.IsSelected).Select(p => p.Party.Id).ToList();

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
