using CashBackMerge.App;

namespace CashBackMerge.Tests.Mandatory.Mr2;

public class MoveEngineTest
{
    private const int Seed = 67;
    private static readonly GameConfig Config = JsonDeserializer.DeserializeFromFile("campaign.exemple.json");
    private static readonly TileSpawner TileSpawner = new TileSpawner(Seed);
    private readonly GameEngine _engine = new(Config, TileSpawner);
    private const MoveDirection Direction = MoveDirection.Left;

    [Fact(DisplayName = "Невалидный ход не меняет поле, не тратит ход и не создаёт плитку")]
    public void InvalidMoveChangesNothing()
    {
        var session = _engine.Session;
        var moveResult = _engine.ApplyMove(Direction);

        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "После валидного хода появляется ровно одна новая плитка",
        Skip = "MR2: реализуй тест и удали эту строку")]
    public void ValidMoveSpawnsExactlyOneTile()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "После N валидных ходов игра завершена",
        Skip = "MR2: реализуй тест и удали эту строку")]
    public void GameEndsAfterMovesLimit()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Достижение плитки 32% не заканчивает игру — доигрываем до лимита ходов",
        Skip = "MR2: реализуй тест и удали эту строку")]
    public void ReachingMaxTileDoesNotEndGame()
    {
        Assert.Fail("Тест не реализован");
    }

    [Fact(DisplayName = "Score считается по правилам и растёт только при слияниях",
        Skip = "MR2: реализуй тест и удали эту строку")]
    public void ScoreIsCalculatedOnMerges()
    {
        Assert.Fail("Тест не реализован");
    }
}