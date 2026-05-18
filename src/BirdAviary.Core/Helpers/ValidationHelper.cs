using System.Text.RegularExpressions;

namespace BirdAviary.Core.Helpers;

public static partial class ValidationHelper
{
  private const int MinHatchYear = 1990;

  [GeneratedRegex(@"^[\p{L}\s]+$", RegexOptions.Compiled)]
  private static partial Regex ColorLettersOnlyRegex();

  public static bool IsValidColorMutation(string? color)
  {
    if (string.IsNullOrWhiteSpace(color))
      return false;

    var trimmed = color.Trim();
    if (!ColorLettersOnlyRegex().IsMatch(trimmed))
      return false;

    return trimmed.Any(c => char.IsLetter(c));
  }

  public static bool IsValidHatchYear(int hatchYear, int? currentYear = null)
  {
    var year = currentYear ?? DateTime.Now.Year;
    return hatchYear >= MinHatchYear && hatchYear <= year;
  }

  public static bool IsValidRingId(string? ringId) =>
    !string.IsNullOrWhiteSpace(ringId) && ringId.Trim().Length >= 3;
}
