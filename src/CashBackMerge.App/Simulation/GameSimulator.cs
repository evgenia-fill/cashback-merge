namespace CashBackMerge.App;

public class GameSimulator
{
    private readonly GameConfig _config;
    private readonly int _seed;
    private readonly StrategyType _strategyType;

    public GameSimulator(GameConfig config, int seed, StrategyType strategyType)
    {
        _config = config;
        _seed = seed;
        _strategyType = strategyType;
    }

    public SimulationResult Simulate(int playersCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(playersCount);

        var gameResults = new List<GameResult>();
        var gamePlayer = new GamePlayer(_config);
        var simulationRandom = new Random(_seed);

        for (var i = 0; i < playersCount; i++)
        {
            var tileSpawnerSeed = simulationRandom.Next();
            var strategySeed = simulationRandom.Next();

            var tileSpawner = new TileSpawner(tileSpawnerSeed);
            var engine = new GameEngine(_config, tileSpawner);

            var strategy = CreateStrategy(strategySeed);

            var gameResult = gamePlayer.PlayOneGame(engine, strategy);

            gameResults.Add(gameResult);
        }

        return new SimulationResult(_config, _seed, _strategyType, gameResults);
    }

    private IMoveStrategy CreateStrategy(int seed)
    {
        return _strategyType switch
        {
            StrategyType.Random => new RandomMoveStrategy(seed),
            _ => throw new InvalidOperationException($"Unsupported strategy: {_strategyType}")
        };
    }
}