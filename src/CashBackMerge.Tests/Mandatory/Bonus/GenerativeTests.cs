using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;

namespace CashBackMerge.Tests.Mandatory.Bonus;

public class GenerativeTests
{
    private static readonly int[] PossibleTileValues = [0, 0, 0, 1, 2, 4, 8, 16, 32];
    private static readonly int[] MergeValues = [1, 2, 4, 8, 16];
    private static readonly MoveDirection[] Directions = Enum.GetValues<MoveDirection>();

    [Fact(DisplayName = "Сумма плиток сохраняется при движении и слиянии на сгенерированных полях")]
    public void BoardMoveSavesTileSumForGeneratedBoards()
    {
        const int seed = 67;
        const int iterations = 1000;

        var random = new Random(seed);

        for (var i = 0; i < iterations; i++)
        {
            var boardSize = random.Next(4, 6);
            var board = GenerateBoard(random, boardSize);

            var direction = Directions[random.Next(Directions.Length)];

            var sumBefore = board.Cast<int>().Sum();
            var result = BoardMover.MoveInDirection(board, direction);
            var sumAfter = result.Board.Cast<int>().Sum();

            Assert.Equal(sumBefore, sumAfter);
        }
    }

    [Fact(DisplayName = "Сгенерированная плитка не объединяется дважды за один ход")]
    public void MergedTileDoesNotMergeAgainForGeneratedValues()
    {
        const int seed = 67;
        const int iterations = 500;

        var random = new Random(seed);

        for (var i = 0; i < iterations; i++)
        {
            var value = MergeValues[random.Next(MergeValues.Length)];
            var direction = Directions[random.Next(Directions.Length)];
            var board = CreateDoubleMergeTrap(random, direction, value);

            var result = BoardMover.MoveInDirection(board, direction);

            var expectedMergedValue = value * 2;
            var forbiddenDoubleMergeValue = value * 4;

            var mergedTilesCount = result.Board.Cast<int>().Count(tile => tile == expectedMergedValue);

            Assert.Equal(2, mergedTilesCount);
            Assert.DoesNotContain(forbiddenDoubleMergeValue, result.Board.Cast<int>());
        }
    }

    private static int[,] GenerateBoard(Random random, int boardSize)
    {
        var board = new int[boardSize, boardSize];

        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                board[row, column] = PossibleTileValues[random.Next(PossibleTileValues.Length)];
            }
        }

        return board;
    }

    private static int[,] CreateDoubleMergeTrap(Random random, MoveDirection direction, int value)
    {
        const int boardSize = 4;

        var board = new int[boardSize, boardSize];
        var lineIndex = random.Next(boardSize);

        switch (direction)
        {
            case MoveDirection.Left:
                board[lineIndex, 0] = value;
                board[lineIndex, 1] = value;
                board[lineIndex, 2] = value * 2;
                break;

            case MoveDirection.Right:
                board[lineIndex, 1] = value * 2;
                board[lineIndex, 2] = value;
                board[lineIndex, 3] = value;
                break;

            case MoveDirection.Up:
                board[0, lineIndex] = value;
                board[1, lineIndex] = value;
                board[2, lineIndex] = value * 2;
                break;

            case MoveDirection.Down:
                board[1, lineIndex] = value * 2;
                board[2, lineIndex] = value;
                board[3, lineIndex] = value;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return board;
    }
}