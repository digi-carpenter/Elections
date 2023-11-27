using ElectionComission.Interfaces;

namespace ElectionComission;

public static class Tallies
{
    /// <summary>
    /// Tallies the provided ballots of valid candidates by candidate.
    /// </summary>
    /// <param name="ballots">The ballots.</param>
    /// <returns><see cref="ITally"/></returns>
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

    private record CandidateTally(ICandidate Candidate, int Votes) : ITally;
}