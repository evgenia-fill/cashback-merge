namespace CashBackMerge.App;

public record SimulationResult(
    GameConfig Config,
    int Seed,
    StrategyType Strategy,
    List<GameResult> GameResults);