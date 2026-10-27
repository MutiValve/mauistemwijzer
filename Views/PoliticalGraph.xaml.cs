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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Re-build series every time the page appears so fresh answer data is used
        _vm.BuildSeries();
    }
}