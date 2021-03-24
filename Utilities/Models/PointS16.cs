using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;

namespace ROI.Utilities.Models
{
    public struct PointS16
    {
        public short X;
        public short Y;

        public PointS16(short x, short y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is PointS16 that && that.X == X && that.Y == Y;
        public override int GetHashCode() => (ushort)X << 16 | (ushort)Y;
        public override string ToString() => $"X:{X}, Y:{Y}";

        private static readonly Regex Parser = new Regex("X:(?<X>[0-9].*), Y:(?<Y>[0-9].*)", RegexOptions.Compiled);
        public static bool TryParse(string input, out PointS16 p)
        {
            Match match = Parser.Match(input);

            if (match.Success)
            {
                p = new PointS16(
                    short.Parse(match.Groups["X"].Value),
                    short.Parse(match.Groups["Y"].Value)
                );

                return true;
            }

            p = default;
            return false;
        }
        public static PointS16 Parse(string input)
        {
            Match match = Parser.Match(input);
            if (match.Success)
            {
                return new PointS16(
                    short.Parse(match.Groups["X"].Value),
                    short.Parse(match.Groups["Y"].Value));
            }

            throw new FormatException("Could not parse " + nameof(PointS16));
        }

        public static bool operator ==(PointS16 a, PointS16 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(PointS16 a, PointS16 b) => a.X != b.X || a.Y != b.Y;

        public static bool operator >(PointS16 m, PointS16 n) => m.X > n.X && m.Y > n.Y;
        public static bool operator <(PointS16 m, PointS16 n) => m.X < n.X && m.Y < n.Y;

        public static bool operator >=(PointS16 m, PointS16 n) => m > n || m == n;
        public static bool operator <=(PointS16 m, PointS16 n) => m < n || m == n;

        public static PointS16 operator +(PointS16 a, PointS16 b) => new PointS16((short)(a.X + b.X), (short)(a.Y + b.Y));
        public static PointS16 operator -(PointS16 a, PointS16 b) => new PointS16((short)(a.X - b.X), (short)(a.Y - b.Y));
        public static PointS16 operator *(PointS16 a, PointS16 b) => new PointS16((short)(a.X * b.X), (short)(a.Y * b.Y));
        public static PointS16 operator /(PointS16 a, PointS16 b) => new PointS16((short)(a.X / b.X), (short)(a.Y / b.Y));

        public static PointS16 operator *(PointS16 p, int s) => new PointS16((short)(p.X * s), (short)(p.Y * s));
        public static PointS16 operator /(PointS16 p, int s) => new PointS16((short)(p.X / s), (short)(p.Y / s));

        public static PointS16 operator *(PointS16 p, double s) => new PointS16((short)(p.X * s), (short)(p.Y * s));
        public static PointS16 operator /(PointS16 p, double s) => new PointS16((short)(p.X / s), (short)(p.Y / s));

        public static implicit operator PointU8(PointS16 p) => new PointU8((byte)p.X, (byte)p.Y);
        public static implicit operator PointS32(PointS16 p) => new PointS32(p.X, p.Y);

        public static implicit operator Point(PointS16 p) => new Point(p.X, p.Y);
        public static explicit operator PointS16(Point p) => new PointS16((short)p.X, (short)p.Y);

        public static implicit operator Vector2(PointS16 p) => new Vector2(p.X, p.Y);
        public static explicit operator PointS16(Vector2 p) => new PointS16((short)p.X, (short)p.Y);
    }
}
