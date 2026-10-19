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
        IReadOnlyList<SpawnSettingsDto> invalidSpawnRules =
        [
            new SpawnSettingsDto("1", "0.67"),
            new SpawnSettingsDto("2", "0.67")
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
        IReadOnlyList<SpawnSettingsDto> invalidSpawnRules = [];
        var config = CreateValidConfig() with { SpawnRules = invalidSpawnRules };
        Assert.Throws<InvalidOperationException>(() => ConfigValidator.Validate(config));
    }

    private static GameConfigDto CreateValidConfig() =>
        new(
            MovesLimit: 3,
            BoardSize: 4,
            MaxRewardAllowedRate: "0.20",
            MaxRewardCashback: "32",
            SpawnRules:
            [
                new SpawnSettingsDto("1", "0.85"),
                new SpawnSettingsDto("2", "0.15")
            ],
            RewardTable:
            [
                new RewardSettingsDto("2", "0"),
                new RewardSettingsDto("4", "50"),
                new RewardSettingsDto("8", "150"),
                new RewardSettingsDto("16", "500"),
                new RewardSettingsDto("32", "2000")
            ]);
}