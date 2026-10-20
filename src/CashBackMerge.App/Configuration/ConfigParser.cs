using System.Globalization;

namespace CashBackMerge.App;

public static class ConfigParser
{
    public static GameConfig Parse(GameConfigDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var boardSize = dto.BoardSize;
        var movesLimit = dto.MovesLimit;

        var spawnSettingsDto = dto.SpawnRules!;
        var spawnRules = (from spawnRule in spawnSettingsDto
                          let probability = decimal.Parse(spawnRule.Probability, CultureInfo.InvariantCulture)
                          let cashback = int.Parse(spawnRule.Cashback)
                          select new SpawnSettings(cashback, probability))
            .ToList();

        var rewardSettingsDto = dto.RewardTable!;
        var rewardTable = (from rewardRule in rewardSettingsDto
                           let cashback = int.Parse(rewardRule.Cashback)
                           let reward = int.Parse(rewardRule.Reward)
                           select new RewardSettings(cashback, reward))
            .ToList();

        var maxRewardCashback = int.Parse(dto.MaxRewardCashback);

        var maxRewardAllowedRate = decimal.Parse(dto.MaxRewardAllowedRate, CultureInfo.InvariantCulture);

        return new GameConfig(movesLimit, boardSize, maxRewardAllowedRate, maxRewardCashback, spawnRules, rewardTable);
    }
}