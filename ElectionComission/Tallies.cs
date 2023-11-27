using ElectionComission.Interfaces;

namespace ElectionComission;

public static class Tallies
{
    private record CandidateTally(ICandidate Candidate, int Votes) : ITally;
}