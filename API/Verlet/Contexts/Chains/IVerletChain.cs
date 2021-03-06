using System.Collections.Generic;

namespace ROI.API.Verlet.Contexts.Chains
{
    public interface IVerletChain : IVerletBody
    {
        IList<ChainDrawData> DrawData { get; set; }
    }
}
