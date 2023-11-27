namespace ElectionComission;

internal class TiedRankedElectionException : Exception
{
    public TiedRankedElectionException()
    {
    }

    public TiedRankedElectionException(string message)
        : base(message)
    {
    }

    public TiedRankedElectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
