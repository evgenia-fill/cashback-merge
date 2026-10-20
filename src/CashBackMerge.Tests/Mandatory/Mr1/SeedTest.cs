using CashBackMerge.App;
using CashBackMerge.App.Configuration;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Reporting;
using CashBackMerge.App.Simulation;
using CashBackMerge.App.Spawning;
using CashBackMerge.App.Strategies;
using CashBackMerge.App.Validation;

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

    [Fact(DisplayName = "Одинаковый конфиг, seed и стратегия дают одинаковый отчёт симуляции")]
    public void SameInputsProduceSameSimulationReport()
    {
        const int seed = 67;
        const int playersCount = 100;
        const string filename = "campaign.exemple.json";

        var dto = JsonDeserializer.DeserializeFromFile(filename);
        ConfigValidator.Validate(dto);

        var config = ConfigParser.Parse(dto);

        Func<int, IMoveStrategy> strategyFactory = strategySeed => new RandomMoveStrategy(strategySeed);

        var firstSimulator = new GameSimulator(config, seed, StrategyType.Random, strategyFactory);
        var secondSimulator = new GameSimulator(config, seed, StrategyType.Random, strategyFactory);

        var firstStatistics = StatisticsCreator.CreateStatistics(firstSimulator.Simulate(playersCount));
        var secondStatistics = StatisticsCreator.CreateStatistics(secondSimulator.Simulate(playersCount));

        var firstPath = Path.GetTempFileName();
        var secondPath = Path.GetTempFileName();

        try
        {
            JsonReportWriter.Write(firstStatistics, firstPath);
            JsonReportWriter.Write(secondStatistics, secondPath);

            var firstReport = File.ReadAllText(firstPath);
            var secondReport = File.ReadAllText(secondPath);

            Assert.Equal(firstReport, secondReport);
        }
        finally
        {
            File.Delete(firstPath);
            File.Delete(secondPath);
        }
    }
}