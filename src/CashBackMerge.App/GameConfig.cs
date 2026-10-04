namespace CashBackMerge.App;

public class GameConfig
{
    public int MovesLimit { get; init; }
    public IReadOnlyList<SpawnSettings> SpawnRules { get; init; } = [];
}