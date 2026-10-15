namespace CashBackMerge.App;

public static class ConfigValidator
{
    public static void Validate(GameConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        if (config.MovesLimit <= 0)
        {
            throw new InvalidOperationException("Moves limit must be greater than zero");
        }

        if (config.BoardSize <= 0)
        {
            throw new InvalidOperationException("Board size must be greater than zero");
        }

        SpawnRulesValidator.Validate(config.SpawnRules);
    }
}