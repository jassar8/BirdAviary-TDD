using System.Windows;
using BirdAviary.Services;

namespace BirdAviary;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        AppServices.Initialize();
        base.OnStartup(e);
    }
}
