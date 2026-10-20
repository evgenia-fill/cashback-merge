namespace CashBackMerge.App;

public record GameConfigDto(
    int MovesLimit,
    int BoardSize,
    string MaxRewardAllowedRate,
    string MaxRewardCashback,
    IReadOnlyList<SpawnSettingsDto>? SpawnRules,
    IReadOnlyList<RewardSettingsDto>? RewardTable);