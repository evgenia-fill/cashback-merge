using CashBackMerge.App;
using CashBackMerge.App.Configuration;
using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;
using CashBackMerge.App.Game;
using CashBackMerge.App.Spawning;
using CashBackMerge.App.Validation;

namespace CashBackMerge.Tests.Mandatory.Mr2;

public class MoveEngineTest
{
    private const int Seed = 67;

    private static readonly GameConfig Config = CreateConfig();

    [Fact(DisplayName = "Невалидный ход не меняет поле, не тратит ход и не создаёт плитку")]
    public void InvalidMoveChangesNothing()
    {
        var tileSpawner = new TileSpawner(Seed);
        var engine = new GameEngine(Config, tileSpawner);
        var session = engine.Session;

        var board = new int[session.BoardSize, session.BoardSize];
        board[0, 0] = 1;
        session.ApplyBoard(board);

        var boardBefore = session.Board.Cast<int>().ToArray();
        var usedMovesBefore = session.UsedMoves;

        var moveResult = engine.ApplyMove(MoveDirection.Left);

        Assert.False(moveResult.IsChanged);
        Assert.Equal(usedMovesBefore, session.UsedMoves);
        Assert.Equal(boardBefore, session.Board.Cast<int>().ToArray());
    }

    [Fact(DisplayName = "После валидного хода появляется ровно одна новая плитка")]
    public void ValidMoveSpawnsExactlyOneTile()
    {
        var tileSpawner = new TileSpawner(Seed);
        var engine = new GameEngine(Config, tileSpawner);
        var session = engine.Session;

        var board = new int[session.BoardSize, session.BoardSize];
        board[0, 1] = 1;
        session.ApplyBoard(board);

        var countTilesBefore = session.Board.Cast<int>().Count(x => x != 0);

        var moveResult = engine.ApplyMove(MoveDirection.Left);

        var countTilesAfter = session.Board.Cast<int>().Count(x => x != 0);

        var difference = countTilesAfter - countTilesBefore;

        Assert.True(moveResult.IsChanged);
        Assert.Equal(1, difference);
    }

    [Fact(DisplayName = "После N валидных ходов игра завершена")]
    public void GameEndsAfterMovesLimit()
    {
        const int movesLimit = 3;

        var config = Config with { MovesLimit = movesLimit };
        var tileSpawner = new TileSpawner(Seed);
        var engine = new GameEngine(config, tileSpawner);
        var session = engine.Session;

        for (var i = 0; i < movesLimit; i++)
        {
            var board = new int[session.BoardSize, session.BoardSize];
            board[0, 1] = 1;
            session.ApplyBoard(board);

            engine.ApplyMove(MoveDirection.Left);
        }

        Assert.Equal(movesLimit, session.UsedMoves);
        Assert.Equal(GameStatus.Finished, session.Status);
    }

    [Fact(DisplayName = "Достижение плитки 32% не заканчивает игру — доигрываем до лимита ходов")]
    public void ReachingMaxTileDoesNotEndGame()
    {
        var tileSpawner = new TileSpawner(Seed);
        var engine = new GameEngine(Config, tileSpawner);
        var session = engine.Session;

        var board = new int[session.BoardSize, session.BoardSize];
        board[0, 0] = 16;
        board[0, 1] = 16;
        session.ApplyBoard(board);

        var moveResult = engine.ApplyMove(MoveDirection.Left);

        Assert.True(moveResult.IsChanged);
        Assert.Contains(32, session.Board.Cast<int>());
        Assert.Equal(GameStatus.InProgress, session.Status);
    }

    [Fact(DisplayName = "Score считается по правилам и растёт только при слияниях")]
    public void ScoreIsCalculatedOnMerges()
    {
        var tileSpawner = new TileSpawner(Seed);
        var engine = new GameEngine(Config, tileSpawner);
        var session = engine.Session;

        var boardWithoutMerge =
            new int[session.BoardSize, session.BoardSize];

        boardWithoutMerge[0, 1] = 1;
        session.ApplyBoard(boardWithoutMerge);

        engine.ApplyMove(MoveDirection.Left);

        Assert.Equal(0, session.Score);

        var boardWithMerge = new int[session.BoardSize, session.BoardSize];

        boardWithMerge[0, 0] = 1;
        boardWithMerge[0, 1] = 1;
        session.ApplyBoard(boardWithMerge);

        engine.ApplyMove(MoveDirection.Left);

        Assert.Equal(2, session.Score);

        var boardWithTwoMerges = new int[session.BoardSize, session.BoardSize];

        boardWithTwoMerges[0, 0] = 2;
        boardWithTwoMerges[0, 1] = 2;
        boardWithTwoMerges[0, 2] = 4;
        boardWithTwoMerges[0, 3] = 4;
        session.ApplyBoard(boardWithTwoMerges);

        engine.ApplyMove(MoveDirection.Left);

        Assert.Equal(14, session.Score);
    }

    private static GameConfig CreateConfig()
    {
        var dto = JsonDeserializer.DeserializeFromFile("campaign.exemple.json");
        ConfigValidator.Validate(dto);
        return ConfigParser.Parse(dto);
    }
}