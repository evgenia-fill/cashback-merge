namespace CashBackMerge.App;

public class GameSession
{
    private int[,] _board;
    public int[,] Board => (int[,])_board.Clone();
    public int MovesLimit { get; }
    public GameStatus Status { get; private set; }
    public int UsedMoves { get; private set; }
    public int Score { get; private set; }
    public int BoardSize { get; }

    public GameSession(int movesLimit, int boardSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(movesLimit);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(boardSize);

        MovesLimit = movesLimit;
        BoardSize = boardSize;
        _board = new int[BoardSize, BoardSize];
        Status = GameStatus.InProgress;
    }

    internal void SetTile(int row, int column, int value)
    {
        if (row < 0 || row >= BoardSize || column < 0 || column >= BoardSize)
        {
            throw new ArgumentOutOfRangeException();
        }

        _board[row, column] = value;
    }

    internal void ApplyBoard(int[,] board)
    {
        if (board.GetLength(0) != BoardSize || board.GetLength(1) != BoardSize)
        {
            throw new ArgumentException("BoardSize does not match session BoardSize", nameof(board));
        }

        _board = (int[,])board.Clone();
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