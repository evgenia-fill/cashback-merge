namespace CashBackMerge.App;

public static class RewardResolver
{
    public static int Resolve(int maxCashback, IReadOnlyList<RewardSettings> rewardTable)
    {
        var maxRewardSetting = rewardTable.MaxBy(setting => setting.Cashback)!;

        if (maxCashback >= maxRewardSetting.Cashback)
        {
            return maxRewardSetting.Reward;
        }

        foreach (var rewardSetting in rewardTable)
        {
            if (maxCashback == rewardSetting.Cashback)
            {
                return rewardSetting.Reward;
            }
        }

        throw new KeyNotFoundException($"There's no {maxCashback} in reward table");
    }
}