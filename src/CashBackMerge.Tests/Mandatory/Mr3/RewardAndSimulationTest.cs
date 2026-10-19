using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr3;

public class RewardAndSimulationTest
{
    private static readonly GameConfig Config = CreateConfig();

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
        const int seed = 67;
        var tileSpawner = new TileSpawner(seed);

        const int iterations = 10_000;
        var board = new int[Config.BoardSize, Config.BoardSize];
        var countCashBackOne = 0;
        var countCashBackTwo = 0;
        for (var _ = 0; _ < iterations; _++)
        {
            var tileSpawnResult = tileSpawner.CreateSpawn(board, Config.SpawnRules);
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

    [Fact(DisplayName = "Отчёт содержит распределение максимальных плиток и наград",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void ReportContainsDistributions()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Отчёт содержит долю игроков с максимальной наградой",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void ReportContainsMaxRewardRate()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Статус OK, если actualMaxRewardRate <= maxRewardAllowedRate, иначе FAILED",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void ReportStatusReflectsBusinessConstraint()
    {
        Assert.Fail("Тест не реализован");
    }

    private static GameConfig CreateConfig()
    {
        const string filename = "campaign.exemple.json";

        var dto = JsonDeserializer.DeserializeFromFile(filename);
        ConfigValidator.Validate(dto);

        return ConfigParser.Parse(dto);
    }
}