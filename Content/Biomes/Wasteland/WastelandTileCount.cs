
using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using System;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland
{
    public class WastelandTileCount : ModSystem
    {
        public int tileCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            tileCount = tileCounts[ModContent.TileType<WastelandDirt>()] +
                tileCounts[ModContent.TileType<WastelandRock>()] +
                tileCounts[ModContent.TileType<WastelandGrass>()];
        }
    }
}
