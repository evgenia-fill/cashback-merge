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
        var boardSize = board.GetLength(0);
        var result = new int[boardSize];

        switch (direction)
        {
            case MoveDirection.Left:
            case MoveDirection.Right:
                for (var g = 0; g < boardSize; g++)
                {
                    result[g] = board[index, g];
                }

                break;

            case MoveDirection.Up:
            case MoveDirection.Down:
                for (var g = 0; g < boardSize; g++)
                {
                    result[g] = board[g, index];
                }

                break;

            default:
                throw new InvalidOperationException(nameof(direction));
        }

        return result;
    }

    private static LineMoveResult MoveLineIfReversed(int[] line, bool ifReversed = false)
    {
        if (!ifReversed)
        {
            return MoveLine(line);
        }

        var reversedColumn = line.Reverse().ToArray();
        var lineResult = MoveLine(reversedColumn);
        var result = lineResult.Line.Reverse().ToArray();
        return lineResult with { Line = result };
    }

    private static void SetLine(int[,] board, int index, int[] line, MoveDirection direction)
    {
        var boardSize = board.GetLength(0);

        switch (direction)
        {
            case MoveDirection.Left:
            case MoveDirection.Right:
                for (var g = 0; g < boardSize; g++)
                {
                    board[index, g] = line[g];
                }

                break;

            case MoveDirection.Up:
            case MoveDirection.Down:
                for (var g = 0; g < boardSize; g++)
                {
                    board[g, index] = line[g];
                }

                break;

            default:
                throw new InvalidOperationException(nameof(direction));
        }
    }

    private static LineMoveResult MoveLine(int[] line)
    {
        var tiles = line.Where(x => x != 0).ToList();
        var result = new List<int>();
        var score = 0;

        for (var i = 0; i < tiles.Count;)
        {
            if (i + 1 < tiles.Count && tiles[i] == tiles[i + 1])
            {
                var mergedValue = tiles[i] * 2;
                result.Add(mergedValue);
                score += mergedValue;
                i += 2;
            }
            else
            {
                result.Add(tiles[i]);
                i++;
            }
        }

        while (result.Count < line.Length)
        {
            result.Add(0);
        }

        return new LineMoveResult(result.ToArray(), score);
    }
}