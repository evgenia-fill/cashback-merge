namespace CashBackMerge.App;

public static class BoardMover
{
    public static BoardMoveResult MoveInDirection(GameSession session, MoveDirection direction)
    {
        var boardSize = session.BoardSize;
        var newBoard = new int[boardSize, boardSize];
        var score = 0;

        var shouldReverse = direction is MoveDirection.Right or MoveDirection.Down;

        for (var i = 0; i < boardSize; i++)
        {
            var line = GetLine(session.Board, i, direction);

            var lineResult = MoveLineIfReversed(line, shouldReverse);

            score += lineResult.Score;

            SetLine(newBoard, i, lineResult.Line, direction);
        }

        return new BoardMoveResult(newBoard, score);
    }

    private static int[] GetLine(int[,] board, int index, MoveDirection direction)
    {
        ArgumentNullException.ThrowIfNull(board);

        return direction switch
        {
            MoveDirection.Left or MoveDirection.Right => GetRow(board, index),
            MoveDirection.Up or MoveDirection.Down => GetColumn(board, index),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Неизвестное направление движения."),
        };
    }

    private static int[] GetRow(int[,] board, int index)
    {
        var row = new int[board.GetLength(1)];
        for (var i = 0; i < row.Length; i++)
        {
            row[i] = board[index, i];
        }

        return row;
    }

    private static int[] GetColumn(int[,] board, int index)
    {
        var column = new int[board.GetLength(0)];
        for (var i = 0; i < column.Length; i++)
        {
            column[i] = board[i, index];
        }

        return column;
    }

    private static LineMoveResult MoveLineIfReversed(int[] line, bool isReversed = false)
    {
        if (!isReversed)
        {
            return MoveLine(line);
        }

        var reversedColumn = line.Reverse().ToArray();
        var lineResult = MoveLine(reversedColumn);
        var result = lineResult.Line.Reverse().ToArray();
        return lineResult with { Line = result };
    }

    private static int[] SetLine(int[,] board, int index, int[] line, MoveDirection direction)
    {
        return direction switch
        {
            MoveDirection.Left or MoveDirection.Right => SetRow(board, line, index),
            MoveDirection.Up or MoveDirection.Down => SetColumn(board, line, index),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Неизвестное направление движения."),
        };
    }

    private static int[] SetRow(int[,] board, int[] line, int index)
    {
        var row = new int[board.GetLength(1)];
        for (var g = 0; g < row.Length; g++)
        {
            board[index, g] = line[g];
        }

        return row;
    }

    private static int[] SetColumn(int[,] board, int[] line, int index)
    {
        var column = new int[board.GetLength(0)];
        for (var i = 0; i < column.Length; i++)
        {
            board[i, index] = line[i];
        }

        return column;
    }

    private static LineMoveResult MoveLine(int[] line)
    {
        var result = new int[line.Length];
        var writtenCount = 0;
        var score = 0;
        var lastTileMerged = false;

        foreach (var tile in line)
        {
            if (tile == 0)
            {
                continue;
            }

            if (writtenCount > 0 && !lastTileMerged && result[writtenCount - 1] == tile)
            {
                var mergedValue = tile * 2;
                result[writtenCount - 1] = mergedValue;
                score += mergedValue;
                lastTileMerged = true;
            }
            else
            {
                result[writtenCount] = tile;
                writtenCount++;
                lastTileMerged = false;
            }
        }

        return new LineMoveResult(result, score);
    }
}