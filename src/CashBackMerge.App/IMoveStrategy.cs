namespace CashBackMerge.App;

public interface IMoveStrategy
{
    MoveDirection GetNextMove(IReadOnlyList<MoveDirection> availableMoves);
}