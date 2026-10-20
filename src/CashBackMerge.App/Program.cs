using CashBackMerge.App;
using CashBackMerge.App.Parsing;

try
{
    var options = SimulationCommandParser.Parse(args);

    var dto = JsonDeserializer.DeserializeFromFile(options.ConfigPath);
    ConfigValidator.Validate(dto);
    var config = ConfigParser.Parse(dto);

    var simulator = new GameSimulator(config, options.Seed, options.Strategy);
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