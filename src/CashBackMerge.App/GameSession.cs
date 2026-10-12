namespace CashBackMerge.App;

public class GameSession
{
    public int[,] Field { get; private set; }
    public int MovesLimit { get; }
    public GameStatus Status { get; private set; }
    public int UsedMoves { get; private set; } = 0;
    public int Score { get; private set; }
    public int BoardSize { get; }

    public GameSession(int movesLimit, int boardSize)
    {
        if (movesLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(movesLimit), "MovesLimit must be greater than 0");
        }

        MovesLimit = movesLimit;
        BoardSize = boardSize;
        Status = GameStatus.InProgress;
        Field = new int[BoardSize, BoardSize];
    }

    internal void ApplyField(int[,] field)
    {
        Field = field;
    }

    internal void RegisterChangedMove()
    {
        UsedMoves++;

        if (UsedMoves >= MovesLimit)
        {
            Status = GameStatus.Finished;
        }
    }

    internal void AddScore(int score)
    {
        Score += score;
    }
}