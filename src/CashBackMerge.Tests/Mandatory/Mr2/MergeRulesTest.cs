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

    [Theory(DisplayName = "Одна плитка не сливается дважды за ход: [1,1,2,.] -> [2,2,.,.]")]
    [InlineData("1,1,2,.", "2,2,.,.")]
    public void TileDoesNotMergeTwiceInOneMove(string before, string expected)
    {
        AssertLeftMove(before, expected);
    }

    [Fact(DisplayName = "Слияние работает во всех четырёх направлениях")]
    public void MergeWorksInAllDirections()
    {
        var directions = new[] { MoveDirection.Left, MoveDirection.Right, MoveDirection.Up, MoveDirection.Down, };

        foreach (var direction in directions)
        {
            var session = new GameSession(MovesLimit, BoardSize);

            if (direction is MoveDirection.Left or MoveDirection.Right)
            {
                session.SetTile(0, 0, 1);
                session.SetTile(0, 1, 1);
            }
            else
            {
                session.SetTile(0, 0, 1);
                session.SetTile(1, 0, 1);
            }

            var newBoard = BoardMover.MoveInDirection(session, direction);
            var board = newBoard.Board;

            switch (direction)
            {
                case MoveDirection.Left:
                    Assert.Equal(2, board[0, 0]);
                    break;

                case MoveDirection.Right:
                    Assert.Equal(2, board[0, BoardSize - 1]);
                    break;

                case MoveDirection.Up:
                    Assert.Equal(2, board[0, 0]);
                    break;

                case MoveDirection.Down:
                    Assert.Equal(2, board[BoardSize - 1, 0]);
                    break;
            }
        }
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

        var newBoard = BoardMover.MoveInDirection(session, Direction);

        var resultArray = new int[BoardSize];

        for (var i = 0; i < BoardSize; i++)
        {
            resultArray[i] = newBoard.Board[0, i];
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