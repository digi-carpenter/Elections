using System.Collections.Generic;

namespace Elections.Interfaces;

public interface IBallot
{
    IVoter Voter { get; }
}
