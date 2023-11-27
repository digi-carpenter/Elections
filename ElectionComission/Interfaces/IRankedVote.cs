namespace ElectionComission.Interfaces;

public interface IRankedVote : IVote
{
    int Rank { get; }
}
