namespace CashBackMerge.App;

public record GameConfig(
    int MovesLimit,
    int BoardSize,
    decimal MaxRewardAllowedRate,
    int MaxRewardCashback,
    IReadOnlyList<SpawnSettings> SpawnRules,
    IReadOnlyList<RewardSettings> RewardTable);