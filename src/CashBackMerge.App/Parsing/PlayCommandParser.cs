namespace CashBackMerge.App.Parsing;

public static class PlayCommandParser
{
    public static PlayCommandOptions Parse(string[] args)
    {
        if (args.Length == 0 || args[0] != "play")
        {
            throw new ArgumentException($"Unexpected command {args[0]}");
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

        if (!options.TryGetValue("--config", out var configPath))
        {
            throw new ArgumentException("Missing required option --config");
        }

        if (!options.TryGetValue("--seed", out var seedText) || !int.TryParse(seedText, out var seed))
        {
            throw new ArgumentException("--seed must be an integer");
        }

        return new PlayCommandOptions(configPath, seed);
    }
}