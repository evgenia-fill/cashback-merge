namespace CashBackMerge.App.Parsing;

public record SimulationCommandOptions(
    string ConfigPath,
    int Players,
    StrategyType Strategy,
    int Seed,
    string OutputPath);