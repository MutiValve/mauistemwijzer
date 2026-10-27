using mauistemwijzer.Views;

namespace mauistemwijzer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("partyselect",   typeof(PartySelectPage));
        Routing.RegisterRoute("question",       typeof(QuestionPage));
        Routing.RegisterRoute("result",         typeof(ResultPage));
        Routing.RegisterRoute("politicalgraph", typeof(PoliticalGraph));
    }
}
