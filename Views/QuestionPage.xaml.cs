using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class QuestionPage : ContentPage
{
    public QuestionPage(QuestionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    /// <summary>
    /// Onderschep de Android hardware-terugknop zodat die hetzelfde doet
    /// als de "Ga terug" knop in de UI (vorige vraag, niet shell-pop).
    /// </summary>
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
}
