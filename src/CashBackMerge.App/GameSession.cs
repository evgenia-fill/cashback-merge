namespace CashBackMerge.App;

public class GameSession
{
    public int[,] Board { get; private set; }
    public int MovesLimit { get; }
    public GameStatus Status { get; private set; }
    public int UsedMoves { get; private set; } = 0;
    public int Score { get; private set; }
    public int BoardSize { get; }

    public GameSession(int movesLimit, int boardSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(movesLimit);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(boardSize);

        MovesLimit = movesLimit;
        BoardSize = boardSize;
        Status = GameStatus.InProgress;
        Board = new int[BoardSize, BoardSize];
    }

    internal void ApplyBoard(int[,] Board)
    {
        Board = Board;
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