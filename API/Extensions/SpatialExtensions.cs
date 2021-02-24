using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace ROI.API.Extensions
{
    public static class SpatialExtensions
    {
        public static Point16 ToTileCoordinates16(this Vector2 vector) => new Point16((int)vector.X >> 4, (int)vector.Y >> 4);
    }
}
