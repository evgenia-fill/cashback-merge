namespace CashBackMerge.App;

public class RandomMoveStrategy : IMoveStrategy
{
    private readonly Random _random;

    public RandomMoveStrategy(int seed)
    {
        _random = new Random(seed);
    }

    public MoveDirection GetNextMove(IReadOnlyList<MoveDirection> availableMoves)
    {
        return availableMoves[_random.Next(availableMoves.Count)];
    }
}