using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using BirdAviaryManagement.Core.Models;
using BirdAviaryManagement.Core.Services;

namespace BirdAviaryManagement.App
{
    // Main Avalonia window: UI event handlers call Core services and refresh the grid.
    public partial class MainWindow : Window
    {
        private readonly BirdService birdService;
        private readonly BulkBirdGenerator bulkBirdGenerator;
        private readonly ReportService reportService;

        public MainWindow()
        {
            InitializeComponent();

            // Wire UI to Core services (business logic stays outside the view).
            birdService = new BirdService();
            bulkBirdGenerator = new BulkBirdGenerator();
            reportService = new ReportService();

            ColorMutationTextBox.PropertyChanged += ColorMutationTextBox_PropertyChanged;
            RingIdTextBox.PropertyChanged += RingIdTextBox_PropertyChanged;

            RefreshGrid();
        }

        private void RingIdTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property != TextBox.TextProperty)
            {
                return;
            }

            string current = RingIdTextBox.Text ?? string.Empty;
            string filtered = RingIdValidator.FilterInput(current);

            if (current == filtered)
            {
                return;
            }

            int caretIndex = RingIdTextBox.CaretIndex;
            RingIdTextBox.Text = filtered;
            RingIdTextBox.CaretIndex = Math.Min(caretIndex, filtered.Length);
        }

        private void ColorMutationTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property != TextBox.TextProperty)
            {
                return;
            }

            string current = ColorMutationTextBox.Text ?? string.Empty;
            string filtered = ColorMutationValidator.FilterInput(current);

            if (current == filtered)
            {
                return;
            }

            int caretIndex = ColorMutationTextBox.CaretIndex;
            ColorMutationTextBox.Text = filtered;
            ColorMutationTextBox.CaretIndex = Math.Min(caretIndex, filtered.Length);
        }

        // Validates form input, then asks BirdService to add the bird to inventory.
        private void AddBirdButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string ringId = (RingIdTextBox.Text ?? string.Empty).Trim();
            string colorMutation = (ColorMutationTextBox.Text ?? string.Empty).Trim();
            string hatchYearText = (HatchYearTextBox.Text ?? string.Empty).Trim();

            // UI-layer validation with clear user messages before calling the service.
            if (string.IsNullOrWhiteSpace(ringId))
            {
                ShowMessage(
                    "Ring ID is required. Please enter a unique ring number for this bird.",
                    false
                );
                return;
            }

            if (!RingIdValidator.IsValid(ringId))
            {
                ShowMessage(
                    "Ring ID must contain numbers only. Letters and symbols are not allowed.",
                    false
                );
                return;
            }

            if (RingIdExists(ringId))
            {
                ShowMessage(
                    $"This Ring ID already exists: {ringId}. Please use a different ring number.",
                    false
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(colorMutation))
            {
                ShowMessage(
                    "Color / Mutation is required. Please enter English or Hebrew letters only, such as Blue or כחול.",
                    false
                );
                return;
            }

            if (!IsColorMutationValid(colorMutation))
            {
                ShowMessage(
                    "Color / Mutation can contain English and Hebrew letters only. Numbers, symbols, and other languages are not allowed.",
                    false
                );
                return;
            }

            if (!int.TryParse(hatchYearText, out int hatchYear))
            {
                ShowMessage(
                    "Invalid hatch year. Please enter a valid year, for example 2022.",
                    false
                );
                return;
            }

            int currentYear = DateTime.Now.Year;

            if (hatchYear > currentYear)
            {
                ShowMessage(
                    $"Hatch year cannot be in the future. Please enter a year up to {currentYear}.",
                    false
                );
                return;
            }

            if (hatchYear < Bird.MinimumHatchYear)
            {
                ShowMessage(
                    $"Hatch year cannot be before {Bird.MinimumHatchYear}. Please enter a year from {Bird.MinimumHatchYear} to {currentYear}.",
                    false
                );
                return;
            }

            Bird bird = new Bird
{
    RingId = ringId,
    Type = GetSelectedBirdType(),
    ColorMutation = colorMutation,
    HatchYear = hatchYear,
    Status = GetSelectedBirdStatus(),
    IsAvailableForSale = AvailableForSaleComboBox.SelectedIndex == 0,
    IsBulkGenerated = false
};
            bool added = birdService.AddBird(bird);

            if (!added)
            {
                ShowMessage(
                    "Bird was not added. Please check the details and try again.",
                    false
                );
                return;
            }

            ClearForm();
            RefreshGrid();

            ShowMessage(
                "Bird added successfully.",
                true
            );
        }

        // Generates 10,000 random birds for performance testing and loads them into inventory.
        private void LoadBulkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            List<Bird> birds = bulkBirdGenerator.GenerateBirds(10000);
            int addedCount = birdService.AddBirds(birds);

            RefreshGrid();

            ShowMessage(
                $"Bulk load completed successfully. {addedCount} bird records were added.",
                true
            );
        }

        // Checks bird status, then calls BirdService which uses IHealthService for approval.
        private void UpdateSaleButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
{
    string ringId = (RingIdTextBox.Text ?? string.Empty).Trim();

    if (string.IsNullOrWhiteSpace(ringId))
    {
        ShowMessage(
            "Please enter a Ring ID first. For example: 1001.",
            false
        );
        return;
    }

    Bird? selectedBird = FindBirdByRingId(ringId);

    if (selectedBird == null)
    {
        ShowMessage(
            $"No bird was found with Ring ID: {ringId}. Please add the bird first or choose an existing Ring ID from the table.",
            false
        );
        return;
    }

    if (selectedBird.Status == BirdStatus.Sold)
    {
        ShowMessage(
            $"Bird {ringId} is already sold, so it cannot be marked as available for sale.",
            false
        );
        return;
    }

   if (selectedBird.Status == BirdStatus.Quarantine)
{
    ShowWarningMessage(
        $"Bird {ringId} is currently in quarantine. It cannot be approved for sale until its status changes."
    );
    return;
}

    bool approved = birdService.UpdateSaleAvailability(ringId);

    RefreshGrid();

    if (approved)
    {
        ShowMessage(
            $"Health check approved bird {ringId}. The bird is now marked as available for sale.",
            true
        );
    }
    else
{
    ShowWarningMessage(
        $"Health check did not approve bird {ringId} for sale. The bird remains unavailable for sale."
    );
}
}

        // Builds inventory report (totals, average age, sorted list) and updates the dashboard.
        private void GenerateReportButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            List<Bird> birds = birdService.GetAllBirds();
            InventoryReport report = reportService.CreateInventoryReport(birds, DateTime.Now.Year);

            BirdsDataGrid.ItemsSource = report.SortedBirds;

            ReportTextBlock.Text =
                $"Inventory Summary | Total Birds: {report.TotalBirds} | " +
                $"Average Bird Age: {report.AverageAge:F2} | " +
                $"Available For Sale: {report.AvailableForSaleCount}";

            ShowMessage(
                "Inventory report generated successfully. Birds are sorted by hatch year from newest to oldest.",
                true
            );
        }
        // Clears only bulk-generated birds; manual entries remain in the inventory.
        private void ClearInventoryButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
{
    int removedCount = birdService.ClearBulkBirds();

    RefreshGrid();

    ReportTextBlock.Text = "Click Generate Report to view inventory summary.";

    if (removedCount == 0)
    {
        ShowWarningMessage(
            "No bulk records were found. Manual bird records were not changed."
        );
        return;
    }

    ShowWarningMessage(
        $"Bulk records cleared successfully. Removed {removedCount} generated birds. Manual bird records were kept."
    );
}

        private void RefreshGrid()
        {
            BirdsDataGrid.ItemsSource = null;
            BirdsDataGrid.ItemsSource = birdService.GetAllBirds();
        }

        private bool RingIdExists(string ringId)
        {
            List<Bird> birds = birdService.GetAllBirds();

            foreach (Bird bird in birds)
            {
                if (bird.RingId == ringId)
                {
                    return true;
                }
            }

            return false;
        }
        private Bird? FindBirdByRingId(string ringId)
{
    List<Bird> birds = birdService.GetAllBirds();

    foreach (Bird bird in birds)
    {
        if (bird.RingId == ringId)
        {
            return bird;
        }
    }

    return null;
}

        private bool IsColorMutationValid(string colorMutation)
        {
            return ColorMutationValidator.IsValid(colorMutation);
        }

        private BirdType GetSelectedBirdType()
        {
            int index = BirdTypeComboBox.SelectedIndex;

            if (index == 0) return BirdType.Budgie;
            if (index == 1) return BirdType.Finch;
            if (index == 2) return BirdType.Cockatiel;
            if (index == 3) return BirdType.Canary;
            if (index == 4) return BirdType.Lovebird;

            return BirdType.Budgie;
        }

        private BirdStatus GetSelectedBirdStatus()
        {
            int index = StatusComboBox.SelectedIndex;

            if (index == 0) return BirdStatus.InAviary;
            if (index == 1) return BirdStatus.Sold;
            if (index == 2) return BirdStatus.Quarantine;

            return BirdStatus.InAviary;
        }

        private void ClearForm()
        {
            RingIdTextBox.Text = string.Empty;
            ColorMutationTextBox.Text = string.Empty;
            HatchYearTextBox.Text = string.Empty;
            BirdTypeComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;
            AvailableForSaleComboBox.SelectedIndex = 1;
        }

        // Shows success (green) or error (red) feedback in the modal overlay.
        private void ShowMessage(string message, bool isSuccess)
{
    PopupMessageTextBlock.Text = message;
    MessageOverlay.IsVisible = true;

    if (isSuccess)
    {
        PopupTitleTextBlock.Text = "Success";
        PopupIconTextBlock.Text = "✔";
        PopupHeaderBorder.Background = Avalonia.Media.Brush.Parse("#2E7D32");
        CloseMessageButton.Background = Avalonia.Media.Brush.Parse("#2E7D32");
        return;
    }

    PopupTitleTextBlock.Text = "Error";
    PopupIconTextBlock.Text = "⚠";
    PopupHeaderBorder.Background = Avalonia.Media.Brush.Parse("#C62828");
    CloseMessageButton.Background = Avalonia.Media.Brush.Parse("#C62828");
}
private void ShowWarningMessage(string message)
{
    PopupMessageTextBlock.Text = message;
    MessageOverlay.IsVisible = true;

    PopupTitleTextBlock.Text = "Not Approved";
    PopupIconTextBlock.Text = "!";
    PopupHeaderBorder.Background = Avalonia.Media.Brush.Parse("#EF6C00");
    CloseMessageButton.Background = Avalonia.Media.Brush.Parse("#EF6C00");
}

        private void CloseMessageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MessageOverlay.IsVisible = false;
        }
    }
}