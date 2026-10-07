using System.Globalization;

namespace CashBackMerge.App;

public static class ConfigValidation
{
    public static void ValidateConfig(GameConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (config.MovesLimit <= 0)
        {
            throw new InvalidOperationException("Moves limit must be greater than zero");
        }

        if (config.SpawnRules is null || config.SpawnRules.Count == 0)
        {
            throw new InvalidOperationException("Spawn rules cannot be empty");
        }

        var overallProbability = 0m;

        foreach (var spawnRule in config.SpawnRules)
        {
            if (!decimal.TryParse(spawnRule.Probability, CultureInfo.InvariantCulture, out var probability))
            {
                throw new InvalidOperationException("Invalid spawn rule probability");
            }

            if (probability is <= 0 or > 1)
            {
                throw new InvalidOperationException("Spawn rule probability must be between 0 and 1");
            }

            if (!int.TryParse(spawnRule.Cashback, out var cashback) || cashback <= 0)
            {
                throw new InvalidOperationException("Invalid cashback value");
            }

            overallProbability += probability;
        }

        if (overallProbability != 1m)
        {
            throw new InvalidOperationException("Spawn rules probabilities must sum to 1");
        }
    }
}