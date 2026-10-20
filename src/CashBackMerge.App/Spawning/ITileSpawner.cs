using CashBackMerge.App.Entities;

namespace CashBackMerge.App.Spawning;

public interface ITileSpawner
{
    TileSpawnResult CreateSpawn(int[,] board, IReadOnlyList<SpawnSettings> spawnRules);
}