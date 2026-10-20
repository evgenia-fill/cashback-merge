namespace CashBackMerge.App.Strategies;

public interface IMoveStrategy
{
    MoveDirection GetNextMove(IReadOnlyList<MoveDirection> availableMoves, int[,] board);
}