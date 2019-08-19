using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Placeable.Wasteland
{
    //TODO: MP for this
    class Wasteland_Grass_Seed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eradied seed");
            Tooltip.SetDefault("\"Is it even safe to plant?\"");
        }

        public override void SetDefaults()
        {
            item.maxStack = 9999;
            item.consumable = true;
            item.width = 22;
            item.height = 18;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.useTurn = true;
            
        }

        public override bool UseItem(Player player)
        {

            if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == mod.TileType("Wasteland_Dirt"))
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].type = (ushort)mod.TileType("Wasteland_Grass");
                WorldGen.TileFrame(Player.tileTargetX, Player.tileTargetY);
                Dust.NewDust(new Vector2(Player.tileTargetX, Player.tileTargetY), 5, 5, DustID.Grass, 0, 0.2f, 255, new Color(152, 208, 113), 1f);
                return true;
            }
            return false;
        }

        
    }
}
