using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles
{
    public sealed class WastelandOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Toxic Ore");
            AddMapEntry(Color.YellowGreen, name);
            drop = ModContent.ItemType<Materials.WastestoneOre>();
        }
    }
}