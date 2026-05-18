namespace BirdAviary.Core.Models;

public class TestRunResult
{
    public string TestName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Message { get; set; } = string.Empty;
    public long DurationMs { get; set; }
}
