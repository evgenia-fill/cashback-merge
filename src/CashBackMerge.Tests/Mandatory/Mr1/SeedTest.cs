using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr1;

public class SeedTest
{
    [Fact(DisplayName = "Одинаковый конфиг и seed дают одинаковое начальное поле")]
    public void SameConfigAndSeedProduceSameInitialBoard()
    {
        const int seed = 67;

        var dto = JsonDeserializer.DeserializeFromFile("campaign.exemple.json");
        ConfigValidator.Validate(dto);
        var config = ConfigParser.Parse(dto);

        var firstTileSpawner = new TileSpawner(seed);
        var secondTileSpawner = new TileSpawner(seed);

        var firstResult = new GameEngine(config, firstTileSpawner);
        var secondResult = new GameEngine(config, secondTileSpawner);

        Assert.Equal(
            firstResult.Session.Board.Cast<int>(),
            secondResult.Session.Board.Cast<int>());
    }

    [Fact(
        DisplayName = "Одинаковый конфиг, seed и стратегия дают одинаковый отчёт симуляции",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void SameInputsProduceSameSimulationReport()
    {
        Assert.Fail("Тест не реализован");
    }
}