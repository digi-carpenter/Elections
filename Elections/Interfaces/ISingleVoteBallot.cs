namespace Elections.Interfaces;

public interface ISingleVoteBallot : IBallot
{
    IVote Vote { get; }
}
