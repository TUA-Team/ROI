using Microsoft.Xna.Framework;
using ROI.Core.Verlet;
using ROI.Core.Verlet.Contexts.Chains;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Vines
{
    public class TEWastelandVine : ModTileEntity, IVerletChain
    {
        public IList<ChainDrawData> DrawData { get; set; } = new List<ChainDrawData>();
        public IList<VerletPoint> Points { get; } = new List<VerletPoint>();
        public IList<VerletSegment> Segments { get; } = new List<VerletSegment>();

        public void GenerateDrawData(int length)
        {
            for (int i = 0; i < length; i++)
            {
                DrawData.Add(new ChainDrawData
                {
                    // 18 is the length of each sprite plus padding
                    // Add 36 because the first two sprites are for the ends
                    source = new Rectangle(0, 36 + 18 * Main.rand.Next(4), 16, 16),
                    origin = new Vector2(8, 0)
                });
            }

            // The very last segment should use an end sprite
            DrawData[DrawData.Count - 1].source.Y = 18 * Main.rand.Next(2);
        }
        public void GeneratePoints()
        {
            // `Position` is the top left of the tile, so
            Vector2 position = Position.ToWorldCoordinates() + new Vector2(8, 8);
            for (int i = 0; i < DrawData.Count + 1; i++)
            {
                Vector2 p = position;
                p.Y += 16 * i;
                Points.Add(new VerletPoint(p, p));
            }
        }
        public void GenerateSegments()
        {
            for (int i = 0; i < DrawData.Count; i++)
            {
                Segments.Add(new VerletSegment(Points[i], Points[i + 1], 14));
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                [nameof(DrawData)] = DrawData,
            };
        }
        public override void Load(TagCompound tag)
        {
            DrawData = tag.GetList<ChainDrawData>(nameof(DrawData));
            GeneratePoints();
            GenerateSegments();
        }

        public override bool ValidTile(int i, int j)
        {
            return Main.tile[i, j].type == ModContent.TileType<WastelandGrass>();
        }
    }
}
