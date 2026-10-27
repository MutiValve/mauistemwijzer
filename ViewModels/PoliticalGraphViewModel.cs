using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using mauistemwijzer.Models;
using mauistemwijzer.Services;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace mauistemwijzer.ViewModels;

public partial class PoliticalGraphViewModel : ObservableObject
{
    private readonly StemwijzerState _state;

    // Chart series (one for User, one for each party)

    public ISeries[] Series { get; private set; } = [];

    // Axes (Graph) 

    public Axis[] XAxes { get; } =
    [
        new Axis
        {
            MinLimit = -1.1,
            MaxLimit =  1.1,
            IsVisible = false,          // hide tick labels; we draw our own axis labels in XAML
            SeparatorsPaint = new SolidColorPaint(new SKColor(255, 255, 255, 30)),
            CrosshairPaint = null,
            ZeroPaint = new SolidColorPaint(new SKColor(255, 255, 255, 120)) { StrokeThickness = 1.5f },
        }
    ];

    public Axis[] YAxes { get; } =
    [
        new Axis
        {
            MinLimit = -1.1,
            MaxLimit =  1.1,
            IsVisible = false,
            SeparatorsPaint = new SolidColorPaint(new SKColor(255, 255, 255, 30)),
            CrosshairPaint = null,
            ZeroPaint = new SolidColorPaint(new SKColor(255, 255, 255, 120)) { StrokeThickness = 1.5f },
        }


    ];

    public ObservableCollection<RectangularSection> Sections { get; set; } = new()
    {
        // Vertical axis line at X = 0 (Left / Rechts divider)
        new RectangularSection
        {
            Xi = 0,
            Xj = 0,
            Stroke = new SolidColorPaint(SKColor.Parse("#444444"), 2) // Adjust color and thickness as desired
        },
        
        // Horizontal axis line at Y = 0 (Progressief / Conservatief divider)
        new RectangularSection
        {
            Yi = 0,
            Yj = 0,
            Stroke = new SolidColorPaint(SKColor.Parse("#444444"), 2)
        }
    };

    // User position (for display label)

    [ObservableProperty]
    private string userPositionText = string.Empty;

    public PoliticalGraphViewModel(StemwijzerState state)
    {
        _state = state;
    }

    /// Builds all chart series. Call from PoliticalGraph.OnAppearing.

    public void BuildSeries()
    {
        var seriesList = new List<ISeries>();

        // Party dots
        var selectedParties = _state.Parties
            .Where(p => _state.SelectedPartyIds.Contains(p.Id))
            .ToList();

        // If nothing selected, show all parties
        if (selectedParties.Count == 0)
            selectedParties = _state.Parties;

        foreach (var party in selectedParties)
        {
            var color = ParseHexColor(party.Color);

            seriesList.Add(new ScatterSeries<ObservablePoint>
            {
                Name = party.Abbreviation,
                Values = [new ObservablePoint(party.PoliticalX, party.PoliticalY)],
                GeometrySize = 22,
                Fill = new SolidColorPaint(color),
                Stroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 1.5f },
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 9,
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Bottom,
                DataLabelsFormatter = _ => party.Abbreviation,
            });
        }

        // User dot
        var (userX, userY) = ComputeUserPosition();
        UserPositionText = $"Jij: ({userX:F2}, {userY:F2})";

        seriesList.Add(new ScatterSeries<ObservablePoint>
        {
            Name = "Jij",
            Values = [new ObservablePoint(userX, userY)],
            GeometrySize = 30,
            Fill = new SolidColorPaint(new SKColor(220, 220, 220, 230)),
            Stroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2f },
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsSize = 11,
            DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Middle,
            DataLabelsFormatter = _ => "Jij",
            ZIndex = 100,
        });

        Series = [.. seriesList];
        OnPropertyChanged(nameof(Series));
    }

    // Scoring 

    /// Computes the user's (X, Y) position on the compass from their answers.
    /// Each non-skipped answer contributes the question's weight in the direction
    /// of the answer: Eens = +weight, Oneens = -weight, GeenVanBeide = 0.
    /// The accumulated scores are normalised to [-1, 1].
    private (double x, double y) ComputeUserPosition()
    {
        double rawX = 0, rawY = 0;
        double totalEcon = 0, totalSocial = 0;

        foreach (var answer in _state.Answers)
        {
            if (answer.Answer == AnswerType.Overgeslagen) continue;

            var question = _state.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question is null) continue;

            double multiplier = answer.Answer switch
            {
                AnswerType.Eens        =>  1.0,
                AnswerType.Oneens      => -1.0,
                AnswerType.GeenVanBeide => 0.0,
                _                      =>  0.0,
            };

            rawX    += multiplier * question.EconomicWeight;
            rawY    += multiplier * question.SocialWeight;
            totalEcon   += Math.Abs(question.EconomicWeight);
            totalSocial += Math.Abs(question.SocialWeight);
        }

        double x = totalEcon   > 0 ? Math.Clamp(rawX   / totalEcon,   -1, 1) : 0;
        double y = totalSocial > 0 ? Math.Clamp(rawY   / totalSocial, -1, 1) : 0;
        return (x, y);
    }

    // Navigation

    [RelayCommand]
    private async Task GoForwardAsync()
    {
        _state.Reset();
        await Shell.Current.GoToAsync("//start");
    }

    // Helpers

    private static SKColor ParseHexColor(string hex)
    {
        try
        {
            hex = hex.TrimStart('#');
            if (hex.Length == 6)
            {
                byte r = Convert.ToByte(hex[..2], 16);
                byte g = Convert.ToByte(hex[2..4], 16);
                byte b = Convert.ToByte(hex[4..6], 16);
                return new SKColor(r, g, b);
            }
        }
        catch { /* fall through */ }
        return SKColors.Gray;
    }
}
