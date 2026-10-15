namespace CashBackMerge.App;

public class GameEngine
{
    private const int InitialTilesCount = 2;
    private readonly GameConfig _config;
    private readonly ITileSpawner _tileSpawner;
    public GameSession Session { get; }

    public GameEngine(GameConfig config, ITileSpawner tileSpawner)
    {
        _config = config;
        ConfigValidation.ValidateConfig(_config);

        _tileSpawner = tileSpawner;
        Session = new GameSession(_config.MovesLimit, _config.BoardSize);

        for (var i = 0; i < InitialTilesCount; i++)
        {
            _tileSpawner.SpawnTile(Session, _config.SpawnRules);
        }
    }

    public MoveResult ApplyMove(MoveDirection direction)
    {
        if (Session.Status == GameStatus.Finished)
        {
            throw new InvalidOperationException("Game is already finished");
        }

        var moveResult = MoveInDirection(Session, direction);

        var isFieldChanged = !Session.Field.Cast<int>().SequenceEqual(moveResult.Field.Cast<int>());
        if (!isFieldChanged)
        {
            return new MoveResult(isFieldChanged);
        }

        Session.AddScore(moveResult.Score);
        Session.ApplyField(moveResult.Field);
        _tileSpawner.SpawnTile(Session, _config.SpawnRules);
        Session.RegisterChangedMove();

        return new MoveResult(isFieldChanged);
    }

    internal static FieldMoveResult MoveInDirection(GameSession session, MoveDirection direction)
    {
        var fieldSize = session.BoardSize;
        var newField = new int[fieldSize, fieldSize];
        var score = 0;

        switch (direction)
        {
            case MoveDirection.Left:
                for (var i = 0; i < fieldSize; i++)
                {
                    var row = new int[fieldSize];
                    for (var g = 0; g < fieldSize; g++)
                    {
                        row[g] = session.Field[i, g];
                    }

                    var lineResult = MoveLine(row);
                    score += lineResult.Score;

                    for (var g = 0; g < fieldSize; g++)
                    {
                        newField[i, g] = lineResult.Line[g];
                    }
                }

                break;

            case MoveDirection.Right:
                for (var i = 0; i < fieldSize; i++)
                {
                    var row = new int[fieldSize];
                    for (var g = 0; g < fieldSize; g++)
                    {
                        row[g] = session.Field[i, g];
                    }

                    var reversedRow = row.Reverse().ToArray();
                    var lineResult = MoveLine(reversedRow);
                    score += lineResult.Score;
                    var resultRow = lineResult.Line.Reverse().ToArray();

                    for (var g = 0; g < fieldSize; g++)
                    {
                        newField[i, g] = resultRow[g];
                    }
                }

                break;

            case MoveDirection.Up:
                for (var i = 0; i < fieldSize; i++)
                {
                    var column = new int[fieldSize];
                    for (var g = 0; g < fieldSize; g++)
                    {
                        column[g] = session.Field[g, i];
                    }

                    var lineResult = MoveLine(column);
                    score += lineResult.Score;

                    for (var g = 0; g < fieldSize; g++)
                    {
                        newField[g, i] = lineResult.Line[g];
                    }
                }

                break;

            case MoveDirection.Down:
                for (var i = 0; i < fieldSize; i++)
                {
                    var column = new int[fieldSize];
                    for (var g = 0; g < fieldSize; g++)
                    {
                        column[g] = session.Field[g, i];
                    }

                    var reversedColumn = column.Reverse().ToArray();
                    var lineResult = MoveLine(reversedColumn);
                    score += lineResult.Score;
                    var resultColumn = lineResult.Line.Reverse().ToArray();

                    for (var g = 0; g < fieldSize; g++)
                    {
                        newField[g, i] = resultColumn[g];
                    }
                }

                break;

            default:
                throw new InvalidOperationException("Invalid MoveDirection");
        }

        return new FieldMoveResult(newField, score);
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