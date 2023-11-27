namespace ElectionComission.Interfaces;

public interface IElection<TBallot>
    where TBallot : IBallot
{
    ICandidate Run(IReadOnlyList<TBallot> ballots, IReadOnlyList<ICandidate> candidates);
}