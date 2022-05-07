using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    //TODO: MP for this
    public class WastelandGrassSeed : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 22;
            Item.height = 18;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileType == ModContent.TileType<Tiles.WastelandDirt>())
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<Tiles.WastelandGrass>();
                WorldGen.TileFrame(Player.tileTargetX, Player.tileTargetY);
                Dust.NewDust(new Vector2(Player.tileTargetX, Player.tileTargetY), 5, 5, DustID.Grass, 0, 0.2f, 255, new Color(152, 208, 113), 1f);
                return true;
            }
            return false;
        }
    }
}