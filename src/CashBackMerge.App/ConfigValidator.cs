namespace CashBackMerge.App;

public static class ConfigValidator
{
    public static void Validate(GameConfigDto configDto)
    {
        ArgumentNullException.ThrowIfNull(configDto);

        if (configDto.MovesLimit <= 0)
        {
            throw new InvalidOperationException("Moves limit must be greater than zero");
        }

        if (configDto.BoardSize <= 0)
        {
            throw new InvalidOperationException("Board size must be greater than zero");
        }

        SpawnRulesValidator.Validate(configDto.SpawnRules);
        RewardValidator.Validate(configDto);
    }
}