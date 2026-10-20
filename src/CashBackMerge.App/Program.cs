using CashBackMerge.App;

var filename = "campaign.exemple.json";
var seed = 67;
var attemptsInSimulation = 4;

var dto = JsonDeserializer.DeserializeFromFile(filename);
ConfigValidator.Validate(dto);
var config = ConfigParser.Parse(dto);

var tileSpawner = new TileSpawner(seed);

var gameSimulator = new GameSimulator(config, seed);
var simulations = gameSimulator.Simulate(attemptsInSimulation);