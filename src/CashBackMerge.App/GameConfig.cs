namespace CashBackMerge.App;

public record GameConfig(
    int MovesLimit,
    int BoardSize,
    string MaxRewardAllowedRate,
    string MaxRewardCashback,
    IReadOnlyList<SpawnSettings>? SpawnRules,
    IReadOnlyList<RewardSettings>? RewardTable);