using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class PoliticalGraph : ContentPage
{
    private readonly PoliticalGraphViewModel _vm;

    public PoliticalGraph(PoliticalGraphViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAndBuildSeriesAsync();
    }
}
