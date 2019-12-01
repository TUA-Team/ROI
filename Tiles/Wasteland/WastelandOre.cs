using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    internal class WastelandOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Toxic Ore");
            AddMapEntry(Color.YellowGreen, name);
            drop = ModContent.ItemType<Items.Materials.Wasteland.WastestoneOre>();
        }
    }
}
