namespace CashBackMerge.App;

public interface ITileSpawner
{
    void SpawnTile(GameSession session, IReadOnlyList<SpawnSettings> spawnRules);
}