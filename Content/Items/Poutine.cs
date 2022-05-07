using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items
{
    public class Poutine : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.rare = 99;
            Item.lavaWet = true;
            Item.consumable = true;
            Item.maxStack = 30;
        }

        public override bool? UseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness))
                return false;

            player.HealEffect(500, true);
            player.AddBuff(BuffID.PotionSickness, 1800, true);

            return true;
        }
    }
}