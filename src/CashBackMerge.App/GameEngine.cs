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
        ConfigValidator.Validate(_config);

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

        var moveResult = BoardMover.MoveInDirection(Session, direction);

        var isBoardChanged = !Session.Board.Cast<int>().SequenceEqual(moveResult.Board.Cast<int>());
        if (!isBoardChanged)
        {
            return new MoveResult(isBoardChanged);
        }

        Session.AddScore(moveResult.Score);
        Session.ApplyBoard(moveResult.Board);
        _tileSpawner.SpawnTile(Session, _config.SpawnRules);
        Session.RegisterChangedMove();

        return new MoveResult(isBoardChanged);
    }
}