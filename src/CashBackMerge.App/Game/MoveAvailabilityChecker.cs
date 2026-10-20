using CashBackMerge.App.Enums;

namespace CashBackMerge.App.Game;

public static class MoveAvailabilityChecker
{
    private static readonly MoveDirection[] Directions =
    [
        MoveDirection.Down, MoveDirection.Up, MoveDirection.Left, MoveDirection.Right
    ];

    public static IReadOnlyList<MoveDirection> GetAvailableMoves(int[,] board)
    {
        var availableMoves = new List<MoveDirection>();
        foreach (var direction in Directions)
        {
            var newBoard = BoardMover.MoveInDirection(board, direction).Board;
            if (!newBoard.Cast<int>().SequenceEqual(board.Cast<int>()))
            {
                availableMoves.Add(direction);
            }
        }

        return availableMoves;
    }
}