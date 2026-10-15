using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr1;

public class ConfigValidationTest
{
    [Fact(DisplayName = "Валидный конфиг принимается")]
    public void ValidConfigIsAccepted()
    {
        var config = JsonDeserializer.DeserializeFromFile("campaign.exemple.json");
        ConfigValidator.Validate(config);
    }

    [Fact(DisplayName = "Сумма вероятностей spawn-правил не равна 1 — конфиг отклоняется")]
    public void ProbabilitiesNotSummingToOneAreRejected()
    {
        IReadOnlyList<SpawnSettings> invalidSpawnRules =
        [
            new SpawnSettings("1", "0.67"),
            new SpawnSettings("2", "0.67")
        ];
        var config = CreateValidConfig() with { SpawnRules = invalidSpawnRules };
        Assert.Throws<InvalidOperationException>(() => ConfigValidator.Validate(config));
    }

    [Theory(DisplayName = "movesLimit <= 0 — конфиг отклоняется")]
    [InlineData(-67)]
    [InlineData(0)]
    public void NonPositiveMovesLimitIsRejected(int movesLimit)
    {
        var config = CreateValidConfig() with { MovesLimit = movesLimit };
        Assert.Throws<InvalidOperationException>(() => ConfigValidator.Validate(config));
    }

    [Fact(DisplayName = "Пустой список spawn-правил — конфиг отклоняется")]
    public void EmptySpawnRulesAreRejected()
    {
        IReadOnlyList<SpawnSettings> invalidSpawnRules = [];
        var config = CreateValidConfig() with { SpawnRules = invalidSpawnRules };
        Assert.Throws<InvalidOperationException>(() => ConfigValidator.Validate(config));
    }

    private static GameConfig CreateValidConfig() =>
        new(
            MovesLimit: 3,
            BoardSize: 4,
            MaxRewardAllowedRate: "0.20",
            MaxRewardCashback: "32",
            SpawnRules:
            [
                new SpawnSettings("1", "0.85"),
                new SpawnSettings("2", "0.15")
            ],
            RewardTable:
            [
                new RewardSettings("2", "0"),
                new RewardSettings("4", "50"),
                new RewardSettings("8", "150"),
                new RewardSettings("16", "500"),
                new RewardSettings("32", "2000")
            ]);
}