using System.Globalization;

namespace CashBackMerge.App;

public static class SpawnRulesValidator
{
    public static void Validate(IReadOnlyList<SpawnSettingsDto>? spawnRules)
    {
        if (spawnRules is null || spawnRules.Count == 0)
        {
            throw new InvalidOperationException("Spawn rules cannot be empty");
        }

        var overallProbability = 0m;

        foreach (var spawnRule in spawnRules)
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