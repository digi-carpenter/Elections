namespace ElectionComission.Interfaces;

/// <summary>
/// Maintains a tally of votes for a candidate.
/// </summary>
public interface ITally
{
    ICandidate Candidate { get; }

    int Votes { get; }
}