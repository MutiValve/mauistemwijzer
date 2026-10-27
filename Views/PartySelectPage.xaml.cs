using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class PartySelectPage : ContentPage
{
    public PartySelectPage(PartySelectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PartySelectViewModel vm)
            vm.Initialize();
    }
}
