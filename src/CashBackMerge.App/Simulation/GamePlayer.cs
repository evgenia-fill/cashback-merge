using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Strategies;

namespace CashBackMerge.App.Simulation;

public class GamePlayer
{
    private readonly GameConfig _config;

    public GamePlayer(GameConfig config)
    {
        _config = config;
    }

    public GameResult PlayOneGame(GameEngine engine, IMoveStrategy strategy)
    {
        var session = engine.Session;

        while (session.Status == GameStatus.InProgress)
        {
            var availableMoves = MoveAvailabilityChecker.GetAvailableMoves(session.Board);
            if (availableMoves.Count == 0)
            {
                session.Finish();
                break;
            }

            engine.ApplyMove(strategy.GetNextMove(availableMoves, session.Board));
        }

        var maxTile = session.Board.Cast<int>().Max();
        var reward = RewardResolver.Resolve(maxTile, _config.RewardTable);
        return new GameResult(maxTile, session.Score, reward);
    }
}