using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items
{
    internal class Poutine : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 20;
            item.rare = 99;
            item.lavaWet = true;
            item.consumable = true;
            item.maxStack = 30;
        }

        public override bool UseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness))
                return false;

            player.HealEffect(500, true);
            player.AddBuff(BuffID.PotionSickness, 1800, true);

            return true;
        }
    }
}