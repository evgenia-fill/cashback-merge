using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Simulation;
using CashBackMerge.App.Spawning;

namespace CashBackMerge.App.ConsoleGame;

public class ConsoleGameRunner
{
    private readonly GameConfig _config;
    private readonly int _seed;

    public ConsoleGameRunner(GameConfig config, int seed)
    {
        _config = config;
        _seed = seed;
    }

    public void Run()
    {
        var tileSpawner = new TileSpawner(_seed);
        var engine = new GameEngine(_config, tileSpawner);
        var session = engine.Session;

        while (session.Status == GameStatus.InProgress)
        {
            PrintState(session);

            var availableMoves = MoveAvailabilityChecker.GetAvailableMoves(session.Board);
            if (availableMoves.Count == 0)
            {
                session.Finish();
                Console.WriteLine("Доступных ходов больше нет");
                break;
            }

            Console.Write("Ход [W/A/S/D], Q — выход: ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input == "q")
            {
                return;
            }

            if (!TryParseDirection(input, out var direction))
            {
                Console.WriteLine("Неизвестное направление");
                continue;
            }

            var result = engine.ApplyMove(direction);
            if (!result.IsChanged)
            {
                Console.WriteLine("Ход не изменил поле и не был засчитан");
            }
        }

        PrintState(session);
        Console.WriteLine("Игра завершена");
    }

    private void PrintState(GameSession session)
    {
        var board = session.Board;

        Console.WriteLine();
        ConsoleGame.PrintBoard(board);

        var movesLeft = session.MovesLimit - session.UsedMoves;
        var maxTile = board.Cast<int>().Max();
        var reward = maxTile < _config.RewardTable.Min(x => x.Cashback) ? 0 : RewardResolver.Resolve(maxTile, _config.RewardTable);

        Console.WriteLine($"Осталось ходов: {movesLeft}");
        Console.WriteLine($"Score: {session.Score}");
        Console.WriteLine($"Максимальная плитка: {maxTile}");
        Console.WriteLine($"Текущая награда: {reward}");
        Console.WriteLine();
    }

    private static bool TryParseDirection(string? input, out MoveDirection direction)
    {
        switch (input)
        {
            case "w":
            case "up":
                direction = MoveDirection.Up;
                return true;

            case "s":
            case "down":
                direction = MoveDirection.Down;
                return true;

            case "a":
            case "left":
                direction = MoveDirection.Left;
                return true;

            case "d":
            case "right":
                direction = MoveDirection.Right;
                return true;

            default:
                direction = default;
                return false;
        }
    }
}