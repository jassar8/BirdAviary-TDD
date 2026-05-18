using BirdAviary.Core.Enums;

namespace BirdAviary.Helpers;

public static class EnumDisplayHelper
{
    public static string GetBirdStatusDisplay(BirdStatus status) => status switch
    {
        BirdStatus.InAviary => "In Aviary",
        BirdStatus.Sold => "Sold",
        BirdStatus.Isolation => "Isolation",
        _ => status.ToString()
    };
}
