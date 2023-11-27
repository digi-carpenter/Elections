namespace ElectionComission.Interfaces;

public interface ISingleVoteBallot : IBallot
{
    IVote Vote { get; }
}
