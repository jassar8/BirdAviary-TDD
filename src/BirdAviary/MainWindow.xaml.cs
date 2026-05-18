using System.Windows;
using BirdAviary.ViewModels;

namespace BirdAviary;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
