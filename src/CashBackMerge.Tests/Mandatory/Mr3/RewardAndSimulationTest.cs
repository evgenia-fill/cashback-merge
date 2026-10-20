using System.Text.Json;
using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr3;

public class RewardAndSimulationTest
{
    private static readonly GameConfig Config = CreateConfig();
    private const int Seed = 67;

    [Fact(DisplayName = "Максимальная плитка 16% даёт награду соответствующего tier")]
    public void RewardMatchesHighestTile()
    {
        const int maxTile = 16;
        var reward = RewardResolver.Resolve(maxTile, Config.RewardTable);

        const int expected = 500;
        Assert.Equal(expected, reward);
    }

    [Fact(DisplayName = "Weighted random учитывает вероятности из конфига")]
    public void SpawnRespectsConfiguredProbabilities()
    {
        var tileSpawner = new TileSpawner(Seed);

        const int iterations = 10_000;
        var board = new int[Config.BoardSize, Config.BoardSize];
        var countCashBackOne = 0;
        var countCashBackTwo = 0;

        for (var i = 0; i < iterations; i++)
        {
            var tileSpawnResult =
                tileSpawner.CreateSpawn(board, Config.SpawnRules);

            if (tileSpawnResult.Value == 1)
            {
                countCashBackOne++;
            }
            else
            {
                countCashBackTwo++;
            }
        }

        var probOne = (decimal)countCashBackOne / iterations;
        var probTwo = (decimal)countCashBackTwo / iterations;

        Assert.InRange(probOne, 0.82m, 0.88m);
        Assert.InRange(probTwo, 0.12m, 0.18m);
    }

    [Fact(DisplayName = "Отчёт содержит распределение максимальных плиток и наград")]
    public void ReportContainsDistributions()
    {
        var statistics = CreateTestStatistics();
        var outputPath = Path.GetTempFileName();

        try
        {
            JsonReportWriter.Write(statistics, outputPath);

            var json = File.ReadAllText(outputPath);
            using var document = JsonDocument.Parse(json);

            var root = document.RootElement;

            Assert.True(root.TryGetProperty("maxTileDistribution", out var tileDistribution));
            Assert.True(root.TryGetProperty("rewardDistribution", out var rewardDistribution));

            Assert.Equal(1, tileDistribution.GetProperty("16").GetInt32());
            Assert.Equal(2, tileDistribution.GetProperty("32").GetInt32());
            Assert.Equal(1, tileDistribution.GetProperty("64").GetInt32());
            Assert.Equal(1, rewardDistribution.GetProperty("500").GetInt32());
            Assert.Equal(3, rewardDistribution.GetProperty("2000").GetInt32());
        }
        finally
        {
            File.Delete(outputPath);
        }
    }

    [Fact(DisplayName = "Отчёт содержит долю игроков с максимальной наградой")]
    public void ReportContainsMaxRewardRate()
    {
        var statistics = CreateTestStatistics();
        var outputPath = Path.GetTempFileName();

        try
        {
            JsonReportWriter.Write(statistics, outputPath);

            var json = File.ReadAllText(outputPath);
            using var document = JsonDocument.Parse(json);

            var actualMaxRewardRate = document.RootElement
                .GetProperty("actualMaxRewardRate")
                .GetDecimal();

            Assert.Equal(0.75m, actualMaxRewardRate);
        }
        finally
        {
            File.Delete(outputPath);
        }
    }

    [Fact(DisplayName = "Статус OK, если actualMaxRewardRate <= maxRewardAllowedRate, иначе FAILED")]
    public void ReportStatusReflectsBusinessConstraint()
    {
        var gameResults = CreateGameResults();

        var failedConfig = Config with { MaxRewardAllowedRate = 0.20m };
        var failedSimulation = new SimulationResult(failedConfig, Seed, StrategyType.Random, gameResults);
        var failedStatistics = StatisticsCreator.CreateStatistics(failedSimulation);

        Assert.Equal(StatisticsStatus.Failed, failedStatistics.Status);

        var okConfig = Config with { MaxRewardAllowedRate = 0.75m };
        var okSimulation = new SimulationResult(okConfig, Seed, StrategyType.Random, gameResults);
        var okStatistics = StatisticsCreator.CreateStatistics(okSimulation);

        Assert.Equal(StatisticsStatus.Ok, okStatistics.Status);
    }

    private static SimulationStatistics CreateTestStatistics()
    {
        var simulation = new SimulationResult(Config, Seed, StrategyType.Random, CreateGameResults());
        return StatisticsCreator.CreateStatistics(simulation);
    }

    private static List<GameResult> CreateGameResults()
    {
        return
        [
            new GameResult(16, 100, 500),
            new GameResult(32, 200, 2000),
            new GameResult(32, 300, 2000),
            new GameResult(64, 400, 2000)
        ];
    }

    private static GameConfig CreateConfig()
    {
        const string filename = "campaign.exemple.json";

        var dto = JsonDeserializer.DeserializeFromFile(filename);
        ConfigValidator.Validate(dto);

        return ConfigParser.Parse(dto);
    }
}