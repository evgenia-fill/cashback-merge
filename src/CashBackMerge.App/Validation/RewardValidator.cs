using System.Globalization;
using CashBackMerge.App.Configuration;

namespace CashBackMerge.App.Validation;

public static class RewardValidator
{
    public static void Validate(GameConfigDto configDto)
    {
        var rewardTable = configDto.RewardTable;

        if (rewardTable is null || rewardTable.Count == 0)
        {
            throw new InvalidOperationException("Reward table cannot be empty");
        }

        foreach (var rewardSetting in rewardTable)
        {
            if (!int.TryParse(rewardSetting.Cashback, out var cashback) || cashback <= 0)
            {
                throw new InvalidOperationException("Invalid reward cashback value");
            }

            if (!int.TryParse(rewardSetting.Reward, out var reward) || reward < 0)
            {
                throw new InvalidOperationException("Invalid reward value");
            }
        }

        if (!int.TryParse(configDto.MaxRewardCashback, out var maxRewardCashback) || maxRewardCashback <= 0)
        {
            throw new InvalidOperationException("Invalid max reward cashback");
        }

        if (!rewardTable.Any(x => int.TryParse(x.Cashback, out var cashback) && cashback == maxRewardCashback))
        {
            throw new InvalidOperationException("Max reward cashback must exist in reward table");
        }

        if (!decimal.TryParse(configDto.MaxRewardAllowedRate, CultureInfo.InvariantCulture, out var maxRewardAllowedRate))
        {
            throw new InvalidOperationException("Invalid max reward allowed rate");
        }

        if (maxRewardAllowedRate is < 0 or > 1)
        {
            throw new InvalidOperationException("Max reward allowed rate must be between 0 and 1");
        }
    }
}