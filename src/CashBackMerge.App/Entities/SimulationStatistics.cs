using CashBackMerge.App.Enums;

namespace CashBackMerge.App.Entities;

public record SimulationStatistics(
    GameConfig Config,
    int Seed,
    int PlayersCount,
    StrategyType Strategy,
    IReadOnlyDictionary<int, int> MaxTileDistribution,
    IReadOnlyDictionary<int, int> RewardDistribution,
    decimal AverageScore,
    int MaxScore,
    decimal ActualMaxRewardRate,
    StatisticsStatus Status);