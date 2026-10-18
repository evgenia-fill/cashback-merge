namespace CashBackMerge.App;

public interface ITileSpawner
{
    TileSpawnResult CreateSpawn(int[,] board, IReadOnlyList<SpawnSettings> spawnRules);
}