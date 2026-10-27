using mauistemwijzer.ViewModels;

namespace mauistemwijzer.Views;

public partial class ResultPage : ContentPage
{
    public ResultPage(ResultViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ResultViewModel vm)
            vm.Calculate();
    }

    /// <summary>
    /// Blokkeer de hardware-terugknop op de uitslag-pagina.
    /// Gebruik de "Opnieuw beginnen" knop om te resetten.
    /// </summary>
    protected override bool OnBackButtonPressed() => true;
}
