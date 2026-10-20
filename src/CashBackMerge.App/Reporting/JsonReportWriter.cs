using System.Text.Json;

namespace CashBackMerge.App;

public static class JsonReportWriter
{
    public static void Write(SimulationStatistics statistics, string outputPath)
    {
        var jsonReport = CreateReport(statistics);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(jsonReport, options);
        File.WriteAllText(outputPath, json);
    }

    private static JsonReport CreateReport(SimulationStatistics statistics)
    {
        return new JsonReport(
            statistics.Config,
            statistics.Seed,
            statistics.PlayersCount,
            statistics.Strategy.ToString().ToLowerInvariant(),
            statistics.MaxTileDistribution,
            statistics.RewardDistribution,
            statistics.AverageScore,
            statistics.MaxScore,
            statistics.ActualMaxRewardRate,
            statistics.Status.ToString().ToUpperInvariant());
    }
}