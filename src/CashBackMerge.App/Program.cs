using CashBackMerge.App.Configuration;
using CashBackMerge.App.ConsoleGame;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Parsing;
using CashBackMerge.App.Reporting;
using CashBackMerge.App.Simulation;
using CashBackMerge.App.Strategies;
using CashBackMerge.App.Validation;

if (args.Length == 0)
{
    Console.WriteLine("Commands: simulate, play");
    return;
}

try
{
    switch (args[0].ToLowerInvariant())
    {
        case "simulate":
            {
                var options = SimulationCommandParser.Parse(args);
                var dto = JsonDeserializer.DeserializeFromFile(options.ConfigPath);
                ConfigValidator.Validate(dto);
                var config = ConfigParser.Parse(dto);

                Func<int, IMoveStrategy> strategyFactory =
                    options.Strategy switch
                    {
                        StrategyType.Random =>
                            seed => new RandomMoveStrategy(seed),

                        StrategyType.Greedy =>
                            _ => new GreedyScoreStrategy(),

                        _ => throw new ArgumentOutOfRangeException()
                    };

                var simulator = new GameSimulator(config, options.Seed, options.Strategy, strategyFactory);
                var simulation = simulator.Simulate(options.Players);
                var statistics = StatisticsCreator.CreateStatistics(simulation);
                JsonReportWriter.Write(statistics, options.OutputPath);
                Console.WriteLine($"Report saved to {options.OutputPath}");

                break;
            }

        case "play":
            {
                var options = PlayCommandParser.Parse(args);
                var dto = JsonDeserializer.DeserializeFromFile(options.ConfigPath);
                ConfigValidator.Validate(dto);
                var config = ConfigParser.Parse(dto);

                var game = new ConsoleGameRunner(config, options.Seed);
                game.Run();

                break;
            }

        default:
            throw new ArgumentException($"Unknown command: {args[0]}");
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception.Message);
    Environment.ExitCode = 1;
}