using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ROI.Items.Placeables.Hell
{
    class Rubitium_Chunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rubitium chunk");
            Tooltip.SetDefault("Shiny red radioactivity.\nWill be used in the 0.2 update - The nuclear and reforge rework update");

        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 16;
            item.height = 16;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = 1;
            item.useTurn = true;
            //item.createTile = mod.TileType("Rubitium_Ore");
            item.createTile = mod.TileType("Void_EntranceStatues");
        }
    }
}
