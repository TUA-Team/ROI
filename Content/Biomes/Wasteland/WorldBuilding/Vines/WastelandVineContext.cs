using Microsoft.Xna.Framework;
using ROI.API.Verlet.Contexts;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Vines
{
    public class WastelandVineContext : VerletChainContext
    {
        protected override string Tex => "Content/Biomes/Wasteland/WorldBuilding/Vines/WastelandLushVine";
        protected override int UpdateCount => 2;

        private const int TEX_LENGTH = 16;
        private const int TEX_WIDTH = 16;

        /// <summary>
        /// Create a vine
        /// </summary>
        /// <param name="position">The position of the root</param>
        /// <param name="len">The number of segments</param>
        /// <returns>The id of the vine (anchor id, segment id)</returns>
        public (int, int) AddVine(Vector2 position, int len)
        {
            var vertices = new Vector2[len];
            for (int i = 0; i < len; i++)
            {
                vertices[i] = position;
                vertices[i].Y += TEX_LENGTH * i;

                drawData.Add(new ChainDrawData
                {
                    // 18 is the length of each sprite plus padding
                    // Add 36 because the first two sprites are for the ends
                    source = new Rectangle(0, 36 + 18 * Main.rand.Next(4), 16, 16),
                    origin = new Vector2(TEX_WIDTH * 0.5f, 0)
                });
            }

            // The very last segment should use an end sprite
            drawData[drawData.Count - 1].source.Y = 18 * Main.rand.Next(2);

            var anchor = points.Count;
            var seg = AddBody(vertices, FillSizes(len, 14));

            anchors.Add(segments[seg].start);

            return (anchor, seg);
        }

        public void KillVine(int anchorID, int segID)
        {
            // Ensure that it isn't the very last segment
            if (segID < segments.Count - 1)
            {
                return;
            }

            bool flag = true;
            // Start with one, to offset the fact that the number of points in a vine
            // will always be one more than the number of segments
            int pointCount = 1 + anchorID;

            while (flag)
            {
                // Check if the current and next segment are connected
                flag = segments[segID].end == segments[segID].start;

                if (flag)
                {
                    // No need to increment segID since the list will just shrink
                    segments.RemoveAt(segID);
                    pointCount++;
                }
            }

            // Then remove all associated points
            for (int i = anchorID; i < pointCount; i++)
            {
                // Again, no need to use an increment
                points.RemoveAt(anchorID);
            }
        }


        public static WastelandVineContext Deserialize(TagCompound tag)
        {
            var ctx = new WastelandVineContext();
            BaseDeserialize(ctx, tag);
            return ctx;
        }
    }
}
