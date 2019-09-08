using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irrawood
{
    [AutoloadEquip(EquipType.Head)]
    class IrrawoodHelmet : ArmorBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Irrawood Helmet");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.defense = 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IrrawoodChestplate") && legs.type == mod.ItemType("IrrawoodLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+ 3 defense\n" +
                              "Slightly reduced effect against radiation in the wasteland\n" +
                              "Liquid waste have reduced effect";
            player.statDefense += 3;
            player.GetModPlayer<ROIPlayer>().irrawoodSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Sawmill);
            recipe.AddIngredient(mod.ItemType("Irrawood"), 30);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
