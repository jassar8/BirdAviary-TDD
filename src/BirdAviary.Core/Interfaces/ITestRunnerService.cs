using BirdAviary.Core.Models;

namespace BirdAviary.Core.Interfaces;

public interface ITestRunnerService
{
    IReadOnlyList<TestRunResult> RunAllTests();
    (int Passed, int Failed) GetSummary(IReadOnlyList<TestRunResult> results);
}
