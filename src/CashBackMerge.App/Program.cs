using CashBackMerge.App;

var filename = "campaign.exemple.json";
var seed = 67;

var dto = JsonDeserializer.DeserializeFromFile(filename);
ConfigValidator.Validate(dto);
var config = ConfigParser.Parse(dto);

var tileSpawner = new TileSpawner(seed);

var engine = new GameEngine(config, tileSpawner);

var randomStrategy = new RandomMoveStrategy(seed);

var gamePlayer = new GamePlayer(config);
var resultOfOneGame = gamePlayer.PlayOneGame(engine, randomStrategy);