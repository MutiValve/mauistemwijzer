using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using mauistemwijzer.Models;
using mauistemwijzer.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace mauistemwijzer.ViewModels;

public partial class PoliticalGraphViewModel : ObservableObject
{
    private readonly StemwijzerState _state;
    private readonly IStemwijzerService _httpService;

    private (double x, double y) _apiUserPosition = (0, 0);

    // Chart series (one for User, one for each party)
    public ISeries[] Series { get; private set; } = [];

    // Axes (Graph) 
    public Axis[] XAxes { get; } =
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
            Stroke = new SolidColorPaint(SKColor.Parse("#444444"), 2)
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

    public PoliticalGraphViewModel(StemwijzerState state, IStemwijzerService httpService)
    {
        _state = state;
        _httpService = httpService;
    }

    /// <summary>
    /// Fetches coordinates from action=resultaat and builds the graph series.
    /// </summary>
    public async Task LoadAndBuildSeriesAsync()
    {
        try
        {
            // 1. Ensure parties exist
            if (_state.Parties == null || _state.Parties.Count == 0)
            {
                _state.Parties = await _httpService.GetPartiesAsync();
            }

            // 2. Fetch coordinates from backend resultaat API
            var result = await _httpService.GetResultaatAsync(_state.Answers);
            if (result != null)
            {
                // Assign coordinates to each party
                foreach (var partyResult in result.Partijen)
                {
                    var existing = _state.Parties.FirstOrDefault(p => p.Id == partyResult.Id);
                    if (existing != null)
                    {
                        existing.Position = partyResult.Position;
                        if (string.IsNullOrWhiteSpace(existing.ImagePath) && !string.IsNullOrWhiteSpace(partyResult.ImagePath))
                        {
                            existing.ImagePath = partyResult.ImagePath;
                        }
                    }
                }

                // Assign user position
                if (result.UserPosition != null)
                {
                    _apiUserPosition = (result.UserPosition.X, result.UserPosition.Y);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[PoliticalGraphViewModel] Error loading resultaat: {ex.Message}");
        }

        BuildSeries();
    }

    /// <summary>
    /// Builds all chart series.
    /// </summary>
    public void BuildSeries()
    {
        var seriesList = new List<ISeries>();

        // Party dots
        var selectedParties = _state.Parties
            .Where(p => _state.SelectedPartyIds != null && _state.SelectedPartyIds.Contains(p.Id.ToString()))
            .ToList();

        // If nothing selected, show all parties
        if (selectedParties.Count == 0)
            selectedParties = _state.Parties;

        foreach (var party in selectedParties)
        {
            var color = ParseHexColor(party.Color);

            seriesList.Add(new ScatterSeries<ObservablePoint>
            {
                Name = party.DisplayName,
                Values = [new ObservablePoint(party.PoliticalX, party.PoliticalY)],
                GeometrySize = 22,
                Fill = new SolidColorPaint(color),
                Stroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 1.5f },
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Bottom,
                DataLabelsFormatter = _ => party.Abbreviation,
            });
        }

        // User dot
        var (userX, userY) = _apiUserPosition;
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
