using Microsoft.Xna.Framework;
using ROI.API.Verlet.Contexts;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Vines
{
    public class WastelandVineContext : VerletChainContext
    {
        public override string Tex => "Content/Biomes/Wasteland/WorldBuilding/Vines/WastelandLushVine";
        protected override int UpdateCount => 2;

        private const int TEX_LENGTH = 16;
        private const int TEX_WIDTH = 16;

        public void AddVine(Vector2 position, int len)
        {
            anchors.Add(points.Count);

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

            AddSegments(vertices, FillSizes(len, 14));
        }


        public static readonly System.Func<TagCompound, WastelandVineContext> DESERIALIZER = tag =>
        {
            var ctx = new WastelandVineContext();
            BaseDeserialize(ctx, tag);
            return ctx;
        };
    }
}
