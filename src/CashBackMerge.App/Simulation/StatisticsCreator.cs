using CashBackMerge.App.Entities;
using CashBackMerge.App.Enums;

namespace CashBackMerge.App.Simulation;

public static class StatisticsCreator
{
    public static SimulationStatistics CreateStatistics(SimulationResult simulationResults)
    {
        var config = simulationResults.Config;
        var seed = simulationResults.Seed;
        var playersCount = simulationResults.GameResults.Count;
        var strategy = simulationResults.Strategy;
        var maxTileDistribution = CalculateMaxTileDistribution(simulationResults);
        var rewardDistribution = CalculateRewardDistribution(simulationResults);
        var averageScore = simulationResults.GameResults.Average(result => (decimal)result.Score);
        var maxScore = simulationResults.GameResults
            .Max(result => result.Score);
        var maxReward = RewardResolver.Resolve(config.MaxRewardCashback, config.RewardTable);
        var maxRewardPlayersCount = simulationResults.GameResults
            .Count(result => result.Reward == maxReward);
        var actualMaxRewardRate = (decimal)maxRewardPlayersCount / playersCount;
        var status = actualMaxRewardRate <= config.MaxRewardAllowedRate
            ? StatisticsStatus.Ok
            : StatisticsStatus.Failed;

        return new SimulationStatistics(
            config,
            seed,
            playersCount,
            strategy,
            maxTileDistribution,
            rewardDistribution,
            averageScore,
            maxScore,
            actualMaxRewardRate,
            status);
    }

    private static IReadOnlyDictionary<int, int> CalculateMaxTileDistribution(SimulationResult simulationResult)
    {
        var gameResults = simulationResult.GameResults;
        var dict = new Dictionary<int, int>();
        foreach (var result in gameResults)
        {
            var maxTile = result.MaxTile;
            dict.TryAdd(maxTile, 0);
            dict[maxTile]++;
        }

        return dict;
    }

    private static IReadOnlyDictionary<int, int> CalculateRewardDistribution(SimulationResult simulationResult)
    {
        var gameResults = simulationResult.GameResults;
        var dict = new Dictionary<int, int>();
        foreach (var result in gameResults)
        {
            var reward = result.Reward;
            dict.TryAdd(reward, 0);
            dict[reward]++;
        }

        return dict;
    }
}