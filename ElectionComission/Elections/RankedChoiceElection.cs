using ElectionComission.Interfaces;

namespace ElectionComission.Elections;

public class RankedChoiceElection : IElection<IRankedBallot>
{
    public ICandidate Run(IReadOnlyList<IRankedBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        throw new NotImplementedException();
    }
}
