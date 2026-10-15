namespace CashBackMerge.App;

public interface ITileSpawner
{
    TileSpawnResult CreateSpawn(GameSession session, IReadOnlyList<SpawnSettings> spawnRules);
}