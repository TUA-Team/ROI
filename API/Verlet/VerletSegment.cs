namespace ROI.API.Verlet
{
    public class VerletSegment
    {
        public VerletPoint start;
        public VerletPoint end;
        public float size;

        public VerletSegment(VerletPoint start, VerletPoint end, float size)
        {
            this.start = start;
            this.end = end;
            this.size = size;
        }
    }
}
