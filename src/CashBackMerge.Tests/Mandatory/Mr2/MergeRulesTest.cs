using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr2;

public class MergeRulesTest
{
    private const int MovesLimit = 3;
    private const int BoardSize = 4;
    private const MoveDirection Direction = MoveDirection.Left;

    [Theory(DisplayName = "Сдвиг влево: строка до -> после")]
    [InlineData("1,1,.,.", "2,.,.,.")]
    [InlineData("1,1,1,.", "2,1,.,.")]
    [InlineData("1,1,1,1", "2,2,.,.")]
    [InlineData("2,2,4,4", "4,8,.,.")]
    public void RowShiftsLeftAndMerges(string before, string expected)
    {
        AssertLeftMove(before, expected);
    }

    [Fact(DisplayName = "Одна плитка не сливается дважды за ход: [1,1,2,.] -> [2,2,.,.]")]
    public void TileDoesNotMergeTwiceInOneMove()
    {
        AssertLeftMove("1,1,2,.", "2,2,.,.");
    }

    [Theory(DisplayName = "Слияние работает во всех четырёх направлениях")]
    [InlineData(MoveDirection.Left, 0, 0)]
    [InlineData(MoveDirection.Right, 0, BoardSize - 1)]
    [InlineData(MoveDirection.Up, 0, 0)]
    [InlineData(MoveDirection.Down, BoardSize - 1, 0)]
    public void MergeWorksInAllDirections(MoveDirection direction, int expectedRow, int expectedColumn)
    {
        var session = new GameSession(MovesLimit, BoardSize);

        session.SetTile(0, 0, 1);

        var secondTilePosition = direction switch
        {
            MoveDirection.Left or MoveDirection.Right => (Row: 0, Column: 1),
            MoveDirection.Up or MoveDirection.Down => (Row: 1, Column: 0),
            _ => throw new InvalidOperationException(nameof(direction))
        };
        session.SetTile(secondTilePosition.Row, secondTilePosition.Column, 1);

        var moveResult = BoardMover.MoveInDirection(session.Board, direction);
        var board = moveResult.Board;

        Assert.Equal(2, board[expectedRow, expectedColumn]);
    }

    private static void AssertLeftMove(string before, string expected)
    {
        var arrayBefore = MakeArray(before);
        var arrayExpected = MakeArray(expected);

        var session = new GameSession(MovesLimit, BoardSize);

        for (var i = 0; i < BoardSize; i++)
        {
            session.SetTile(0, i, arrayBefore[i]);
        }

        var moveResult = BoardMover.MoveInDirection(session.Board, Direction);
        var board = moveResult.Board;

        var resultArray = new int[BoardSize];

        for (var i = 0; i < BoardSize; i++)
        {
            resultArray[i] = board[0, i];
        }

        Assert.Equal(arrayExpected, resultArray);
    }

    private static int[] MakeArray(string line)
    {
        return line.Split(",")
            .Select(x => int.TryParse(x, out var num) ? num : 0)
            .ToArray();
    }
}