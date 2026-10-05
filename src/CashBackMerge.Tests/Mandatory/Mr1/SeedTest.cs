using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr1;

public class SeedTest
{
    [Fact(DisplayName = "Одинаковый конфиг и seed дают одинаковое начальное поле")]
    public void SameConfigAndSeedProduceSameInitialBoard()
    {
        const int seed = 67;
        var config = JsonDeserializer.DeserializeFromFile("test.json");
        var engine = new GameEngine();
        var firstResult = engine.Start(config, seed);
        var secondResult = engine.Start(config, seed);
        Assert.Equal(firstResult.Field.Cast<int>(), secondResult.Field.Cast<int>());
    }

    [Fact(DisplayName = "Одинаковый конфиг, seed и стратегия дают одинаковый отчёт симуляции",
        Skip = "MR3: реализуй тест и удали эту строку")]
    public void SameInputsProduceSameSimulationReport()
    {
        Assert.Fail("Тест не реализован");
    }
}