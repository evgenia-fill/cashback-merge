using CashBackMerge.App.Configuration;
using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Spawning;
using CashBackMerge.App.Validation;

namespace CashBackMerge.Tests.Mandatory.Bonus;

public class BoardSizeTest
{
    [Fact(DisplayName = "Размер поля берётся из конфигурации")]
    public void BoardSizeComesFromConfig()
    {
        var config = CreateConfig() with { BoardSize = 5 };
        var engine = new GameEngine(config, new TileSpawner(67));
        var board = engine.Session.Board;

        Assert.Equal(5, board.GetLength(0));
        Assert.Equal(5, board.GetLength(1));
    }

    [Fact(DisplayName = "Merge работает на поле 5x5")]
    public void MergeWorksOnFiveByFiveBoard()
    {
        var board = new int[5, 5];

        board[0, 0] = 1;
        board[0, 1] = 1;

        var result = BoardMover.MoveInDirection(board, MoveDirection.Left);

        Assert.Equal(2, result.Board[0, 0]);
        Assert.Equal(0, result.Board[0, 1]);

        Assert.Equal(5, result.Board.GetLength(0));
        Assert.Equal(5, result.Board.GetLength(1));
    }

    private static GameConfig CreateConfig()
    {
        const string filename = "campaign.5x5.json";

        var dto = JsonDeserializer.DeserializeFromFile(filename);
        ConfigValidator.Validate(dto);

        return ConfigParser.Parse(dto);
    }
}