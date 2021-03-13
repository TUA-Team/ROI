using System.Collections.Generic;

namespace ROI.Core.Verlet.Contexts.Chains
{
    public interface IVerletChain : IVerletBody
    {
        IList<ChainDrawData> DrawData { get; set; }
    }
}
