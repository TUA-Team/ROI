using ROI.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Irrawood
{
    [AutoloadEquip(EquipType.Head)]
    class IrrawoodHelmet : IrrawoodArmor
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

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+ 3 defense\n" +
                              "Slightly reduced effect against radiation in the wasteland\n" +
                              "Liquid waste have reduced effect";
            player.statDefense += 3;
            //TODO: player.GetModPlayer<ROIPlayer>().irrawoodSet = true;
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
