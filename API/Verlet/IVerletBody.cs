using System.Collections.Generic;

namespace ROI.API.Verlet
{
    public interface IVerletBody
    {
        IList<VerletPoint> Points { get; }

        IList<VerletSegment> Segments { get; }
    }
}
