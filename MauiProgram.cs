using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection; 
using mauistemwijzer.Services;
using mauistemwijzer.ViewModels;
using mauistemwijzer.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;
using LiveChartsCore.SkiaSharpView.Maui;

namespace mauistemwijzer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseLiveCharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ── Services ──────────────────────────────────────────
        builder.Services.AddHttpClient<IStemwijzerService, HttpStemwijzerService>(client =>
        {
            // Replace with your PHP backend URL (e.g., "https://your-api-domain.com/api/")
            // For Android Emulator local testing, use "http://10.0.2.2/your-project/api/"
            client.BaseAddress = new Uri("http://localhost/programming/stemwijzer/backend/");
        });
        builder.Services.AddSingleton<StemwijzerState>();

        // ── ViewModels ────────────────────────────────────────
        builder.Services.AddTransient<StartViewModel>();
        builder.Services.AddTransient<PartySelectViewModel>();
        builder.Services.AddTransient<QuestionViewModel>();
        builder.Services.AddTransient<ResultViewModel>();
        builder.Services.AddTransient<PoliticalGraphViewModel>();

        // ── Pages ─────────────────────────────────────────────
        builder.Services.AddTransient<StartPage>();
        builder.Services.AddTransient<PartySelectPage>();
        builder.Services.AddTransient<QuestionPage>();
        builder.Services.AddTransient<ResultPage>();
        builder.Services.AddTransient<PoliticalGraph>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
