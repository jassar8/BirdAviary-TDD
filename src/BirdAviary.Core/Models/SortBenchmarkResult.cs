namespace BirdAviary.Core.Models;

public class SortBenchmarkResult
{
    public string AlgorithmName { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public int ItemCount { get; set; }
}
