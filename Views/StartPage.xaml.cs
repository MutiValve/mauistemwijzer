using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class StartPage : ContentPage
{
    public StartPage(StartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
