using Elections.Interfaces;

namespace Elections;

public static class Tallies
{
    private record CandidateTally(ICandidate Candidate, int Votes) : ITally;
}