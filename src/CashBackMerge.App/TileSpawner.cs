using System.Globalization;

namespace CashBackMerge.App;

public class TileSpawner
{
    private readonly Random _random;

    public TileSpawner(int seed)
    {
        _random = new Random(seed);
    }

    public void SpawnTile(GameSession session, IReadOnlyList<SpawnSettings> spawnRules)
    {
        var value = ChooseTileValue(spawnRules);
        var position = ChooseTilePosition(session);
        session.Field[position.Row, position.Column] = value;
    }

    private int ChooseTileValue(IReadOnlyList<SpawnSettings> spawnRules)
    {
        var randomValue = _random.NextDouble();
        var overallProbability = 0m;

        foreach (var spawnRule in spawnRules)
        {
            overallProbability += decimal.Parse(spawnRule.Probability, CultureInfo.InvariantCulture);
            if ((decimal)randomValue < overallProbability)
            {
                return int.Parse(spawnRule.Cashback);
            }
        }

        return int.Parse(spawnRules[^1].Cashback);
    }

    private (int Row, int Column) ChooseTilePosition(GameSession session)
    {
        var field = session.Field;
        var freeCoordinates = new List<(int Row, int Column)>();

        for (var i = 0; i < GameSession.FieldSize; i++)
        {
            for (var g = 0; g < GameSession.FieldSize; g++)
            {
                if (field[i, g] != 0)
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