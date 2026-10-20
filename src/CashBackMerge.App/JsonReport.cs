namespace CashBackMerge.App;

public record JsonReport(
    GameConfig Config,
    int Seed,
    int Players,
    string Strategy,
    IReadOnlyDictionary<int, int> MaxTileDistribution,
    IReadOnlyDictionary<int, int> RewardDistribution,
    decimal AverageScore,
    int MaxScore,
    decimal ActualMaxRewardRate,
    string Status);