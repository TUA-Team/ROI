using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;

namespace ROI.Utilities
{
    public static class SpatialUtils
    {
        public static Point16 ToTileCoordinates16(this Vector2 vector) => new Point16((int)vector.X >> 4, (int)vector.Y >> 4);

        //simple exponential cap
        public static float ExpCap(this float x, float yintercept, float plateau, float increase) => (float)(plateau - (plateau - yintercept) * Math.Exp(-increase * x));

        public static float GetRadianAngle(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Atan((point2.X - point1.X) / (point2.Y - point1.Y));
        }

        public static Vector2 GetMiddleOfTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            return new Vector2((point1.X + point2.X + point3.X) / 3, (point1.Y + point2.Y + point3.Y) / 3);
        }
    }
}
