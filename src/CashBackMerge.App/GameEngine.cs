namespace CashBackMerge.App;

public class GameEngine
{
    private const int InitialTilesCount = 2;

    public GameSession Start(GameConfig config, int seed)
    {
        ConfigValidation.ValidateConfig(config);

        var tileSpawner = new TileSpawner(seed);
        var session = new GameSession(config.MovesLimit);
        for (var i = 0; i < InitialTilesCount; i++)
        {
            tileSpawner.SpawnTile(session, config.SpawnRules);
        }

        return session;
    }
}