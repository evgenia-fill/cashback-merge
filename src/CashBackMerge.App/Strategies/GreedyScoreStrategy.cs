using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;

namespace CashBackMerge.App.Strategies;

public class GreedyScoreStrategy : IMoveStrategy
{
    public MoveDirection GetNextMove(IReadOnlyList<MoveDirection> availableMoves, int[,] board)
    {
        if (availableMoves.Count == 0)
        {
            throw new ArgumentException("Available moves collection must not be empty", nameof(availableMoves));
        }

        var bestDirection = availableMoves[0];
        var bestScore = BoardMover.MoveInDirection(board, bestDirection).Score;

        foreach (var direction in availableMoves.Skip(1))
        {
            var score = BoardMover.MoveInDirection(board, direction).Score;
            if (score <= bestScore)
            {
                continue;
            }

            bestScore = score;
            bestDirection = direction;
        }

        return bestDirection;
    }
}