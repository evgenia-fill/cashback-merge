namespace CashBackMerge.App.Parsing;

public static class SimulationCommandParser
{
    public static SimulationCommandOptions Parse(string[] args)
    {
        if (args.Length == 0 || args[0] != "simulate")
        {
            throw new ArgumentException("Expected command: simulate");
        }

        var options = new Dictionary<string, string>();

        for (var i = 1; i < args.Length; i += 2)
        {
            if (i + 1 >= args.Length)
            {
                throw new ArgumentException($"Missing value for {args[i]}");
            }

            options[args[i]] = args[i + 1];
        }

        var configPath = GetRequired(options, "--config");
        var playersText = GetRequired(options, "--players");
        var strategyText = GetRequired(options, "--strategy");
        var seedText = GetRequired(options, "--seed");
        var outputPath = GetRequired(options, "--output");

        if (!int.TryParse(playersText, out var players) || players <= 0)
        {
            throw new ArgumentException("--players must be a positive integer");
        }

        if (!int.TryParse(seedText, out var seed))
        {
            throw new ArgumentException("--seed must be an integer");
        }

        var strategy = strategyText.ToLowerInvariant() switch
        {
            "random" => StrategyType.Random,
            _ => throw new ArgumentException($"Unsupported strategy: {strategyText}")
        };

        return new SimulationCommandOptions(
            configPath,
            players,
            strategy,
            seed,
            outputPath);
    }

    private static string GetRequired(IReadOnlyDictionary<string, string> options, string name)
    {
        return !options.TryGetValue(name, out var value) ? throw new ArgumentException($"Missing required option {name}") : value;
    }
}