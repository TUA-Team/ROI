using System.Collections.Generic;

namespace ROI.Core.Verlet
{
    public interface IVerletBody
    {
        IList<VerletPoint> Points { get; }

        IList<VerletSegment> Segments { get; }
    }
}
