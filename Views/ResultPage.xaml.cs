using mauistemwijzer.Models;
using mauistemwijzer.Services;
using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class ResultPage : ContentPage
{
    public ResultPage(ResultViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ResultViewModel vm)
        {
            await vm.LoadResultsAsync();
        }
    }
    protected override bool OnBackButtonPressed() => true;
}
