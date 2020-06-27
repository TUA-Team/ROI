using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ROI.Helpers
{
    class ROIMathHelper
    {
        // TO DO: Move to Math helper
        public static float GetRadiantAngle(Vector2 point1, Vector2 point2)
        {
            return (float) Math.Atan((point2.X - point1.X) / (point2.Y - point1.Y));
        }

        // TO DO: Move to Math helper
        public static Vector2 GetMiddleOfTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            return new Vector2((point1.X + point2.X + point3.X) / 3, (point1.Y + point2.Y + point3.Y) / 3);
        }
    }
}
