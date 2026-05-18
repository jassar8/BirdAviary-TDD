using System.Collections.ObjectModel;
using BirdAviary.Core.Enums;
using BirdAviary.Core.Models;
using BirdAviary.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BirdAviary.ViewModels;

public partial class AddBirdViewModel : BaseViewModel
{
    [ObservableProperty] private string _ringId = string.Empty;
    [ObservableProperty] private BirdType _selectedType = BirdType.Cockatiel;
    [ObservableProperty] private string _colorMutation = string.Empty;
    [ObservableProperty] private string _hatchYearText = DateTime.Now.Year.ToString();
    [ObservableProperty] private BirdStatus _selectedStatus = BirdStatus.InAviary;
    [ObservableProperty] private bool _availableForSale;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isSuccess;

    public ObservableCollection<string> ValidationErrors { get; } = [];
    public Array BirdTypes => Enum.GetValues(typeof(BirdType));
    public Array BirdStatuses => Enum.GetValues(typeof(BirdStatus));

    [RelayCommand]
    private void SaveBird()
    {
        ValidationErrors.Clear();
        StatusMessage = string.Empty;
        IsSuccess = false;

        if (!int.TryParse(HatchYearText?.Trim(), out var hatchYear))
        {
            ValidationErrors.Add("Hatch year must be a valid number.");
            return;
        }

        var bird = new Bird
        {
            RingId = RingId?.Trim() ?? string.Empty,
            Type = SelectedType,
            ColorMutation = ColorMutation?.Trim() ?? string.Empty,
            HatchYear = hatchYear,
            Status = SelectedStatus,
            AvailableForSale = AvailableForSale
        };

        var result = AppServices.BirdService.TryAddBird(bird);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ValidationErrors.Add(error);
            StatusMessage = "Please fix validation errors.";
            return;
        }

        IsSuccess = true;
        if (AvailableForSale && !bird.AvailableForSale)
            StatusMessage = $"Bird {bird.RingId} added. Health check did not approve sale listing.";
        else
            StatusMessage = $"Bird {bird.RingId} added successfully!";

        ClearForm();
    }

    [RelayCommand]
    private void ClearForm()
    {
        RingId = string.Empty;
        ColorMutation = string.Empty;
        HatchYearText = DateTime.Now.Year.ToString();
        SelectedType = BirdType.Cockatiel;
        SelectedStatus = BirdStatus.InAviary;
        AvailableForSale = false;
        ValidationErrors.Clear();
        StatusMessage = string.Empty;
    }
}
