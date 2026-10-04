namespace CashBackMerge.App;

public class GameSession
{
    public int[,] Field { get; private set; }
    public int MovesLimit { get; private set; }
    public GameStatus Status { get; private set; }

    public const int FieldSize = 4;

    public GameSession(int movesLimit)
    {
        if (movesLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(movesLimit), "MovesLimit must be greater than 0");
        }

        MovesLimit = movesLimit;
        Status = GameStatus.InProgress;
        Field = new int[FieldSize, FieldSize];

        for (var i = 0; i < FieldSize; i++)
        {
            for (var g = 0; g < FieldSize; g++)
            {
                Field[i, g] = 0;
            }
        }
    }
}