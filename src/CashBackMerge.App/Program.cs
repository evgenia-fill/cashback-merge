using CashBackMerge.App;
using static CashBackMerge.App.JsonReportWriter;

var filename = "campaign.exemple.json";
var seed = 74390;
var attemptsInSimulation = 10000;

var dto = JsonDeserializer.DeserializeFromFile(filename);
ConfigValidator.Validate(dto);
var config = ConfigParser.Parse(dto);

var gameSimulator = new GameSimulator(config, seed, StrategyType.Random);
var simulationResults = gameSimulator.Simulate(attemptsInSimulation);

var stat = StatisticsCreator.CreateStatistics(simulationResults);

Write(stat, "report.json");