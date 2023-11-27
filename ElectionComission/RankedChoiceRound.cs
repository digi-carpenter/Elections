using ElectionComission.Interfaces;

namespace ElectionComission;

/// <summary>
/// This class represents a round of a ranked choice election. It encapsulates the data and only provides get access making it idempotent
/// another side effect is clean and complete serialization whether from deconstruction or JSON marshaling.
/// </summary>
public record RankedChoiceRound
{
    private readonly IEnumerable<ITally> _talliedVotes;
    private readonly int _totalVotes;

    private RankedChoiceRound(int roundNumber, IEnumerable<ITally> talliedVotes)
    {
        RoundNumber = roundNumber;
        _talliedVotes = talliedVotes;
        _totalVotes = talliedVotes.Sum(candidate => candidate.Votes);
    }

    /// <summary>
    /// Starts the round.
    /// </summary>
    /// <param name="currentRound">The current round.</param>
    /// <param name="talliedVotes">The tallied votes.</param>
    /// <returns><see cref="RankedChoiceRound"/></returns>
    public static RankedChoiceRound StartRound(int currentRound, IReadOnlyList<ITally> talliedVotes)
    {
        return new RankedChoiceRound(currentRound, talliedVotes);
    }

    /// <summary>
    /// Gets the round number.
    /// </summary>
    /// <value>
    /// The round number.
    /// </value>
    public int RoundNumber { get; }

    /// <summary>
    /// Gets the tallied votes.
    /// </summary>
    /// <value>
    /// The tallied votes.
    /// </value>
    public IReadOnlyList<ITally> TalliedVotes => _talliedVotes.ToList();

    /// <summary>
    /// The candidate that was eliminated this round.
    /// </summary>
    /// <value>
    /// The eliminated candidate.
    /// </value>
    public ICandidate EliminatedCandidate => _talliedVotes
                                .OrderBy(candidate => candidate.Votes)
                                .First()
                                .Candidate;

    /// <summary>
    /// The candidates moving on to the next round.
    /// </summary>
    /// <value>
    /// The remaining candidates.
    /// </value>
    public IReadOnlyList<ICandidate> RemainingCandidates => _talliedVotes
                                .OrderByDescending(candidate => candidate.Votes)
                                .Select(candidate => candidate.Candidate)
                                .Except(new[] { EliminatedCandidate })
                                .ToList();

    /// <summary>
    /// Determines if this round produced a winner.
    /// </summary>
    /// <param name="winner">The winner.</param>
    public bool TryGetWinner(out ICandidate winner)
    {
        if (_talliedVotes.Any(candidate => HasMajorityVote(_totalVotes, candidate.Votes)))
        {
            winner = _talliedVotes
                    .First(v => HasMajorityVote(_totalVotes, v.Votes))
                    .Candidate;

            return true;
        }

        winner = null!;
        return false;
    }

    private static bool HasMajorityVote(int totalVotes, int votes)
    {
        var majority = totalVotes / 2;
        return votes > majority;
    }
}