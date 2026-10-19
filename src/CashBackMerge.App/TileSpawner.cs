namespace CashBackMerge.App;

public class TileSpawner : ITileSpawner
{
    private readonly Random _random;

    public TileSpawner(int seed)
    {
        _random = new Random(seed);
    }

    public TileSpawnResult CreateSpawn(int[,] board, IReadOnlyList<SpawnSettings> spawnRules)
    {
        var value = ChooseTileValue(spawnRules);
        var position = ChooseTilePosition(board);

        return new TileSpawnResult(position.Row, position.Column, value);
    }

    private int ChooseTileValue(IReadOnlyList<SpawnSettings> spawnRules)
    {
        var randomValue = _random.NextDouble();
        var overallProbability = 0m;

        foreach (var spawnRule in spawnRules)
        {
            overallProbability += spawnRule.Probability;

            if ((decimal)randomValue < overallProbability)
            {
                return spawnRule.Cashback;
            }
        }

        return spawnRules[^1].Cashback;
    }

    private (int Row, int Column) ChooseTilePosition(int[,] board)
    {
        var freeCoordinates = new List<(int Row, int Column)>();

        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var g = 0; g < board.GetLength(1); g++)
            {
                if (board[i, g] != 0)
                {
                    continue;
                }

                freeCoordinates.Add((i, g));
            }
        }

        return freeCoordinates.Count == 0
            ? throw new InvalidOperationException("No free cells available")
            : freeCoordinates[_random.Next(freeCoordinates.Count)];
    }
}