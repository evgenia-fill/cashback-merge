using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr1;

public class SeedTest
{
    [Fact(DisplayName = "Одинаковый конфиг и seed дают одинаковое начальное поле")]
    public void SameConfigAndSeedProduceSameInitialBoard()
    {
        const int seed = 67;
        var config = JsonDeserializer.DeserializeFromFile("campaign.exemple.json");
        var firstTileSpawner = new TileSpawner(seed);
        var secondTileSpawner = new TileSpawner(seed);
        var firstResult = new GameEngine(config, firstTileSpawner);
        var secondResult = new GameEngine(config, secondTileSpawner);
        Assert.Equal(firstResult.Session.Field.Cast<int>(), secondResult.Session.Field.Cast<int>());
    }

    [Fact(DisplayName = "Одинаковый конфиг, seed и стратегия дают одинаковый отчёт симуляции",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void SameInputsProduceSameSimulationReport()
    {
        Assert.Fail("Тест не реализован");
    }
}