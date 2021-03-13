using Microsoft.Xna.Framework;

namespace ROI.Utilities.Models
{
    public struct PointS32
    {
        public int X;
        public int Y;

        public PointU8 Low8 => new PointU8((byte)X, (byte)Y);
        public PointS16 Mid16 => new PointS16((short)(X >> 8), (short)(Y >> 8));
        //public readonly PointS16 High16 => new PointS16((short)(X>>16),(short)(Y>>16));

        public PointS32(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is PointS32 that && that.X == X && that.Y == Y;
        public override int GetHashCode() => X ^ Y;
        public override string ToString() => $"X: {X}, Y: {Y}";

        public static bool operator ==(PointS32 a, PointS32 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(PointS32 a, PointS32 b) => a.X != b.X || a.Y != b.Y;

        public static bool operator >(PointS32 m, PointS32 n) => m.X > n.X && m.Y > n.Y;
        public static bool operator <(PointS32 m, PointS32 n) => m.X < n.X && m.Y < n.Y;

        public static bool operator >=(PointS32 m, PointS32 n) => m > n || m == n;
        public static bool operator <=(PointS32 m, PointS32 n) => m < n || m == n;

        public static PointS32 operator +(PointS32 a, PointS32 b) => new PointS32(a.X + b.X, a.Y + b.Y);
        public static PointS32 operator -(PointS32 a, PointS32 b) => new PointS32(a.X - b.X, a.Y - b.Y);
        public static PointS32 operator *(PointS32 a, PointS32 b) => new PointS32(a.X * b.X, a.Y * b.Y);
        public static PointS32 operator /(PointS32 a, PointS32 b) => new PointS32(a.X / b.X, a.Y / b.Y);

        public static PointS32 operator *(PointS32 p, int s) => new PointS32(p.X * s, p.Y * s);
        public static PointS32 operator /(PointS32 p, int s) => new PointS32(p.X / s, p.Y / s);

        public static PointS32 operator *(PointS32 p, double s) => new PointS32((int)(p.X * s), (int)(p.Y * s));
        public static PointS32 operator /(PointS32 p, double s) => new PointS32((int)(p.X / s), (int)(p.Y / s));

        public static PointS32 operator >>(PointS32 p, int s) => new PointS32(p.X >> s, p.Y >> s);
        public static PointS32 operator <<(PointS32 p, int s) => new PointS32(p.X << s, p.Y << s);

        public static explicit operator PointU8(PointS32 p) => new PointU8((byte)p.X, (byte)p.Y);
        public static explicit operator PointS16(PointS32 p) => new PointS16((short)p.X, (short)p.Y);

        //Legacy
        public static implicit operator Point(PointS32 p) => new Point(p.X, p.Y);
        public static implicit operator PointS32(Point p) => new PointS32(p.X, p.Y);

        public static implicit operator Vector2(PointS32 p) => new Vector2(p.X, p.Y);
        public static explicit operator PointS32(Vector2 p) => new PointS32((int)p.X, (int)p.Y);
    }
}
