using CashBackMerge.App.Configuration;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Parsing;
using CashBackMerge.App.Reporting;
using CashBackMerge.App.Simulation;
using CashBackMerge.App.Strategies;
using CashBackMerge.App.Validation;

try
{
    var options = SimulationCommandParser.Parse(args);

    var dto = JsonDeserializer.DeserializeFromFile(options.ConfigPath);
    ConfigValidator.Validate(dto);
    var config = ConfigParser.Parse(dto);

    Func<int, IMoveStrategy> strategyFactory = options.Strategy switch
    {
        StrategyType.Random =>
            seed => new RandomMoveStrategy(seed),

        StrategyType.Greedy =>
            _ => new GreedyScoreStrategy(),

        _ => throw new ArgumentOutOfRangeException()
    };

    var simulator = new GameSimulator(config, options.Seed, options.Strategy, strategyFactory);
    var simulationResult = simulator.Simulate(options.Players);

    var statistics = StatisticsCreator.CreateStatistics(simulationResult);
    JsonReportWriter.Write(statistics, options.OutputPath);
    Console.WriteLine($"Report saved to {options.OutputPath}");
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    Environment.ExitCode = 1;
}