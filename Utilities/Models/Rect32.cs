using Microsoft.Xna.Framework;

namespace ROI.Utilities.Models
{
    public struct Rect32
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public PointS32 Pos => new PointS32(X, Y);
        public PointS32 Size => new PointS32(Width, Height);

        public PointS32 Max => Pos + Size;

        public Rect32(PointS32 pos, PointS32 size)
        {
            X = pos.X;
            Y = pos.Y;
            Width = size.X;
            Height = size.Y;
        }

        public Rect32(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(in PointS32 p) => p >= Pos && p <= Max;
        public bool Intersects(in Rect32 r) => r.Pos <= Max && r.Max >= Pos;

        public static implicit operator Rect32(Rectangle r) => new Rect32(new PointS32(r.X, r.Y), new PointS32(r.Width, r.Height));
    }
}
