using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr1;

public class ConfigValidationTest
{
    [Fact(DisplayName = "Валидный конфиг принимается")]
    public void ValidConfigIsAccepted()
    {
        var config = JsonDeserializer.DeserializeFromFile("test.json");
        GameEngine.ValidateConfig(config);
    }

    [Fact(DisplayName = "Сумма вероятностей spawn-правил не равна 1 — конфиг отклоняется")]
    public void ProbabilitiesNotSummingToOneAreRejected()
    {
        IReadOnlyList<SpawnSettings> invalidSpawnRules =
        [
            new SpawnSettings { Cashback = "1", Probability = "0.67" },
            new SpawnSettings { Cashback = "2", Probability = "0.67" }
        ];
        const int validMovesLimit = 3;
        var config = CreateInvalidConfig(validMovesLimit, invalidSpawnRules);
        Assert.Throws<InvalidOperationException>(() => GameEngine.ValidateConfig(config));
    }

    [Theory(DisplayName = "movesLimit <= 0 — конфиг отклоняется")]
    [InlineData(-67)]
    [InlineData(0)]
    public void NonPositiveMovesLimitIsRejected(int movesLimit)
    {
        var config = CreateInvalidConfig(movesLimit);
        Assert.Throws<InvalidOperationException>(() => GameEngine.ValidateConfig(config));
    }

    [Fact(DisplayName = "Пустой список spawn-правил — конфиг отклоняется")]
    public void EmptySpawnRulesAreRejected()
    {
        IReadOnlyList<SpawnSettings> invalidSpawnRules = [];
        const int validMovesLimit = 3;
        var config = CreateInvalidConfig(validMovesLimit, invalidSpawnRules);
        Assert.Throws<InvalidOperationException>(() => GameEngine.ValidateConfig(config));
    }

    private static GameConfig CreateInvalidConfig(
        int movesLimit = 0, IReadOnlyList<SpawnSettings>? spawnRules = null) =>
        new() { MovesLimit = movesLimit, SpawnRules = spawnRules };
}