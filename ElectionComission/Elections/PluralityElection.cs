using ElectionComission.Interfaces;

namespace ElectionComission.Elections;

public sealed class PluralityElection : IElection<ISingleVoteBallot>
{
    public ICandidate Run(IReadOnlyList<ISingleVoteBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        // NOTE: My choice here is to go with a DSL based approach based on LINQ since the calculation is straight forward. The benifit is that
        // it's succinct and easy to read in the context of the election, but it's also easy to test outside of the election class.
        return ballots
            .TallyValidVotes(candidates)
            .SelectPluralityWinner();
    }
}
