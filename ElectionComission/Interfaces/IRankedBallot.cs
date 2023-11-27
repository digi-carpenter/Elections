namespace ElectionComission.Interfaces;

public interface IRankedBallot : IBallot
{
    IReadOnlyList<IRankedVote> Votes { get; }
}
