using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Tiles.Wasteland
{
    class Wasteland_Brick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            ModTranslation tileTranslation = CreateMapEntryName();
            tileTranslation.SetDefault("Wasteland Brick");
            drop = ModContent.ItemType<ROI.Items.Placeables.Wasteland.Wastebrick>();
            AddMapEntry(new Color(173, 255, 47));
        }
    }
}
