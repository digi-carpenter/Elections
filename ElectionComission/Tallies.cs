using ElectionComission.Interfaces;

namespace ElectionComission;

public static class Tallies
{
    /// <summary>
    /// Tallies the provided ballots of valid candidates by candidate.
    /// </summary>
    /// <param name="ballots">The ballots.</param>
    public static IEnumerable<ITally> TallyValidVotes(this IReadOnlyList<ISingleVoteBallot> ballots, IEnumerable<ICandidate> validCandidates)
    {
        var tallies = ballots
                        .GroupBy(ballot => ballot.Vote.Candidate)
                        .Select(
                            ballotGroup => new CandidateTally(ballotGroup.Key, ballotGroup.Count()))
                        .ForSpecificCandidates(validCandidates);

        return tallies;
    }
    
    /// <summary>
    /// Selects the candidate with the majority of votes, albeit not necessarily absolute majority.
    /// </summary>
    /// <param name="tallies">The tally.</param>
    /// <returns><see cref="ICandidate"/></returns>
    public static ICandidate SelectPluralityWinner(this IEnumerable<ITally> tallies)
    {
        return tallies
                .OrderByDescending(candidate => candidate.Votes)
                .First()
                .Candidate;
    }

    /// <summary>
    /// Tallies the first round votes.
    /// </summary>
    /// <param name="ballots">The ballots.</param>
    /// <param name="currentCandidates">The current candidates.</param>
    public static IEnumerable<ITally> TallyFirstRoundVotes(this IReadOnlyList<IRankedBallot> ballots, IEnumerable<ICandidate> currentCandidates)
    {
        var tallies = ballots
                .Select(ballot => ballot.Votes[Constants.RankedChoice.FirstRound])
                .GroupBy(vote => vote.Candidate)
                .Select(
                    voteGroup => new CandidateTally(voteGroup.Key, voteGroup.Count()))
                .ForSpecificCandidates(currentCandidates);

        return tallies;
    }

    /// <summary>
    /// Tallies the votes for additional ranked rounds.
    /// </summary>
    /// <param name="ballots">The ballots.</param>
    /// <param name="previousRound">The previous ranked round.</param>
    public static IEnumerable<ITally> TallyAdditionalRoundVotes(this IReadOnlyList<IRankedBallot> ballots, RankedChoiceRound previousRound)
    {
        var currentRound = previousRound.RoundNumber + 1;
        var currentCandidates = previousRound.RemainingCandidates;

        return ballots
                .Where(ballot => ballot.Votes.Count > currentRound)
                .Select(ballot => ballot.Votes[currentRound])
                .GroupBy(vote => vote.Candidate)
                .Select(
                    voteGroup => new CandidateTally(voteGroup.Key, voteGroup.Count()))
                .ForSpecificCandidates(currentCandidates)
                .WithPreviousRoundResults(previousRound);
    }

	/// <summary>
    /// Removes invalid candidates from the collection of tallies.
    /// </summary>
    /// <param name="tallies">The tallies.</param>
    /// <param name="validCandidates">The valid candidates.</param>
    private static IEnumerable<ITally> ForSpecificCandidates(this IEnumerable<ITally> tallies, IEnumerable<ICandidate> validCandidates)
    {
        return tallies.Join(validCandidates,
                            tally => tally.Candidate,
                            candidate => candidate,
                            (tally, candidate) => tally);
    }

    /// <summary>
    /// Adds the previous round results to the current round.
    /// </summary>
    /// <param name="tallies">The tallies.</param>
    /// <param name="previousRound">The previous round.</param>
    private static IEnumerable<ITally> WithPreviousRoundResults(this IEnumerable<ITally> tallies, RankedChoiceRound previousRound)
    {
        return tallies.Join(previousRound.TalliedVotes,
                            currentRoundTally => currentRoundTally.Candidate,
                            previousRoundTally => previousRoundTally.Candidate,
                            (currentRoundTally, previousRoundTally) =>
                            {
                                return new CandidateTally(currentRoundTally.Candidate, (currentRoundTally.Votes + previousRoundTally.Votes));
                            });
    }

    private record CandidateTally(ICandidate Candidate, int Votes) : ITally;
}