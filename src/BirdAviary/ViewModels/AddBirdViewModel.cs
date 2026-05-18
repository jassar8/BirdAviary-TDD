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
    [ObservableProperty] private BirdStatus _selectedStatus = BirdStatus.Healthy;
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

        if (!int.TryParse(HatchYearText, out var hatchYear))
        {
            ValidationErrors.Add("Hatch year must be a valid number.");
            return;
        }

        var bird = new Bird
        {
            RingId = RingId.Trim(),
            Type = SelectedType,
            ColorMutation = ColorMutation.Trim(),
            HatchYear = hatchYear,
            Status = SelectedStatus,
            AvailableForSale = AvailableForSale
        };

        var validation = AppServices.BirdService.ValidateBird(bird);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ValidationErrors.Add(error);
            IsSuccess = false;
            StatusMessage = "Please fix validation errors.";
            return;
        }

        try
        {
            AppServices.BirdService.AddBird(bird);
            IsSuccess = true;
            StatusMessage = $"Bird {bird.RingId} added successfully!";
            ClearForm();
        }
        catch (Exception ex)
        {
            IsSuccess = false;
            StatusMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void ClearForm()
    {
        RingId = string.Empty;
        ColorMutation = string.Empty;
        HatchYearText = DateTime.Now.Year.ToString();
        SelectedType = BirdType.Cockatiel;
        SelectedStatus = BirdStatus.Healthy;
        AvailableForSale = false;
        ValidationErrors.Clear();
    }
}
