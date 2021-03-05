using Microsoft.Xna.Framework;
namespace ROI.API.Verlet
{
    // TODO: This should really be a struct, but that would unfortunately require unsafe
    public class VerletPoint
    {
        public Vector2 old;
        public Vector2 pos;

        public VerletPoint(Vector2 old, Vector2 pos)
        {
            this.old = old;
            this.pos = pos;
        }
    }
}
