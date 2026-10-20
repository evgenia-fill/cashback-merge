using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Spawning;
using CashBackMerge.App.Strategies;

namespace CashBackMerge.App.Simulation;

public class GameSimulator
{
    private readonly GameConfig _config;
    private readonly int _seed;
    private readonly StrategyType _strategyType;
    private readonly Func<int, IMoveStrategy> _strategyFactory;

    public GameSimulator(GameConfig config, int seed, StrategyType strategyType, Func<int, IMoveStrategy> strategyFactory)
    {
        _config = config;
        _seed = seed;
        _strategyType = strategyType;
        _strategyFactory = strategyFactory;
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

            var strategy = _strategyFactory(strategySeed);

            var gameResult = gamePlayer.PlayOneGame(engine, strategy);

            gameResults.Add(gameResult);
        }

        return new SimulationResult(_config, _seed, _strategyType, gameResults);
    }
}