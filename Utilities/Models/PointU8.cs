﻿using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;

namespace ROI.Utilities.Models
{
    public struct PointU8
    {
        public byte X;
        public byte Y;

        public PointU8(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is PointU8 that && that.X == X && that.Y == Y;
        public override int GetHashCode() => X << 8 | Y;
        public override string ToString() => $"X:{X}, Y:{Y}";

        private static readonly Regex Parser = new Regex("X:(?<X>[0-9].*), Y:(?<Y>[0-9].*)", RegexOptions.Compiled);
        public static bool TryParse(string input, out PointU8 p)
        {
            Match match = Parser.Match(input);

            if (match.Success)
            {
                p = new PointU8(
                    byte.Parse(match.Groups["X"].Value),
                    byte.Parse(match.Groups["Y"].Value)
                );

                return true;
            }

            p = default;
            return false;
        }
        public static PointU8 Parse(string input)
        {
            Match match = Parser.Match(input);
            if (match.Success)
            {
                return new PointU8(
                    byte.Parse(match.Groups["X"].Value),
                    byte.Parse(match.Groups["Y"].Value));
            }

            throw new FormatException("Could not parse " + nameof(PointU8));
        }

        public static bool operator ==(PointU8 a, PointU8 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(PointU8 a, PointU8 b) => a.X != b.X || a.Y != b.Y;

        public static bool operator >(PointU8 m, PointU8 n) => m.X > n.X && m.Y > n.Y;
        public static bool operator <(PointU8 m, PointU8 n) => m.X < n.X && m.Y < n.Y;

        public static bool operator >=(PointU8 m, PointU8 n) => m > n || m == n;
        public static bool operator <=(PointU8 m, PointU8 n) => m < n || m == n;

        public static PointU8 operator +(PointU8 a, PointU8 b) => new PointU8((byte)(a.X + b.X), (byte)(a.Y + b.Y));
        public static PointU8 operator -(PointU8 a, PointU8 b) => new PointU8((byte)(a.X - b.X), (byte)(a.Y - b.Y));
        public static PointU8 operator *(PointU8 a, PointU8 b) => new PointU8((byte)(a.X * b.X), (byte)(a.Y * b.Y));
        public static PointU8 operator /(PointU8 a, PointU8 b) => new PointU8((byte)(a.X / b.X), (byte)(a.Y / b.Y));

        public static PointU8 operator *(PointU8 p, int s) => new PointU8((byte)(p.X * s), (byte)(p.Y * s));
        public static PointU8 operator /(PointU8 p, int s) => new PointU8((byte)(p.X / s), (byte)(p.Y / s));

        public static implicit operator PointS16(PointU8 p) => new PointS16(p.X, p.Y);
        public static implicit operator PointS32(PointU8 p) => new PointU8(p.X, p.Y);

        public static implicit operator Point(PointU8 p) => new Point(p.X, p.Y);
        public static explicit operator PointU8(Point p) => new PointU8((byte)p.X, (byte)p.Y);

        public static implicit operator Vector2(PointU8 p) => new Vector2(p.X, p.Y);
        public static explicit operator PointU8(Vector2 p) => new PointU8((byte)p.X, (byte)p.Y);
    }
}
