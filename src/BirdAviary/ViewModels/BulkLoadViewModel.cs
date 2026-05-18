using System.Diagnostics;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BirdAviary.ViewModels;

public partial class BulkLoadViewModel : BaseViewModel
{
    [ObservableProperty] private double _progress;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _statusText = "Ready to generate 10,000 birds";
    [ObservableProperty] private string _elapsedTime = "—";
    [ObservableProperty] private int _birdsGenerated;

    [RelayCommand]
    private async Task GenerateBirdsAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        Progress = 0;
        StatusText = "Generating birds...";
        var sw = Stopwatch.StartNew();

        await Task.Run(() =>
        {
            var progress = new Progress<int>(p =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Progress = p;
                    StatusText = $"Generating... {p}%";
                });
            });

            BirdsGenerated = AppServices.BirdService.GenerateBulkBirds(10_000, progress);
        });

        sw.Stop();
        Progress = 100;
        ElapsedTime = $"{sw.ElapsedMilliseconds:N0} ms ({sw.Elapsed.TotalSeconds:F2}s)";
        StatusText = $"Successfully generated {BirdsGenerated:N0} birds";
        IsLoading = false;
    }
}
