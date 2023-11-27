using ElectionComission.Interfaces;

namespace ElectionComission.Elections;

public sealed class RankedChoiceElection : IElection<IRankedBallot>
{
    public ICandidate Run(IReadOnlyList<IRankedBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        var maxRounds = candidates.Count - 1;
        var roundResults = new List<RankedChoiceRound>(maxRounds);

        for (var roundNumber = 0; roundNumber < candidates.Count; roundNumber++)
        {
            var roundResult = roundResults.Count > 0
                ? RankedChoiceRound.StartRound(
                    roundNumber,
                    CombinedRoundTally(ballots, roundResults[roundNumber - 1]))

                : RankedChoiceRound.StartRound(
                    Constants.RankedChoice.FirstRound,
                    FilteredFirstRoundTally(ballots, candidates));

            roundResults.Add(roundResult);

            if (roundResult.TryGetWinner(out ICandidate winner))
            {
                return winner;
            }
        }

        throw new TiedRankedElectionException($"A conclusive winner could not be determined after {maxRounds} rounds.");
    }

    private static IReadOnlyList<ITally> FilteredFirstRoundTally(IReadOnlyList<IRankedBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        return ballots
            .TallyFirstRoundVotes(candidates)
            .ToList();
    }

    private static IReadOnlyList<ITally> CombinedRoundTally(IReadOnlyList<IRankedBallot> ballots, RankedChoiceRound previousRound)
    {
        return ballots
            .TallyAdditionalRoundVotes(previousRound)
            .ToList();
    }
}