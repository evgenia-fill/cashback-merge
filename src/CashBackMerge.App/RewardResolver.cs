namespace CashBackMerge.App;

public static class RewardResolver
{
    public static int Resolve(int maxTile, IReadOnlyList<RewardSettings> rewardTable)
    {
        foreach (var rewardSetting in rewardTable)
        {
            if (maxTile != rewardSetting.Cashback)
            {
                continue;
            }

            return rewardSetting.Reward;
        }

        throw new KeyNotFoundException($"There's no {maxTile} in reward table");
    }
}