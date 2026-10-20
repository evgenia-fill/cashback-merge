namespace CashBackMerge.App;

public class GameSimulator
{
    private readonly GameConfig _config;
    private readonly int _seed;

    public GameSimulator(GameConfig config, int seed)
    {
        _config = config;
        _seed = seed;
    }

    public List<GameResult> Simulate(int attempts)
    {
        var results = new List<GameResult>();
        var gamePlayer = new GamePlayer(_config);
        var simulationRandom = new Random(_seed);

        for (var i = 0; i < attempts; i++)
        {
            var tileSpawnerSeed = simulationRandom.Next();
            var strategySeed = simulationRandom.Next();

            var tileSpawner = new TileSpawner(tileSpawnerSeed);
            var engine = new GameEngine(_config, tileSpawner);

            var strategy = new RandomMoveStrategy(strategySeed);

            var gameResult = gamePlayer.PlayOneGame(engine, strategy);

            results.Add(gameResult);
        }

        return results;
    }
}