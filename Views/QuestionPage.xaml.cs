using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class QuestionPage : ContentPage
{
    public QuestionPage(QuestionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is QuestionViewModel vm)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
                await vm.GoBackCommand.ExecuteAsync(null));
            return true; // event afgehandeld
        }
        return base.OnBackButtonPressed();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is QuestionViewModel vm)
            vm.InitializeAsync();
    }
}
