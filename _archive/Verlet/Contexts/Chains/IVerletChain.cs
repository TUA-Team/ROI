using System.Collections.Generic;

namespace ROI.Core.Verlet.Contexts.Chains
{
    public interface IVerletChain : IVerletBody
    {
        // TODO: i have no clue where this went
        IList<ChainDrawData> DrawData { get; set; }
    }
}
