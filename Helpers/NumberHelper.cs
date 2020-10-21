using Microsoft.Xna.Framework;
using System;

namespace ROI.Helpers
{
    public static class NumberHelper
    {
        public static Color Mix(this Color color, Color other)
        {
            var r = (color.R + other.R) / 2;
            var g = (color.G + other.G) / 2;
            var b = (color.B + other.B) / 2;
            var a = (color.A + other.A) / 2;
            return new Color(r, g, b, a);
        }

        //simple exponential cap
        public static float ExpCap(this float x, float yintercept, float plateau, float increase) => (float)(plateau - (plateau - yintercept) * Math.Exp(-increase * x));

        public static float GetRadiantAngle(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Atan((point2.X - point1.X) / (point2.Y - point1.Y));
        }

        public static Vector2 GetMiddleOfTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            return new Vector2((point1.X + point2.X + point3.X) / 3, (point1.Y + point2.Y + point3.Y) / 3);
        }
    }
}