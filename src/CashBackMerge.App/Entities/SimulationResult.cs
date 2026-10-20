using CashBackMerge.App.Enums;

namespace CashBackMerge.App.Entities;

public record SimulationResult(
    GameConfig Config,
    int Seed,
    StrategyType Strategy,
    List<GameResult> GameResults);