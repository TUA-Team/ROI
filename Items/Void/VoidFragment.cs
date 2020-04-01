using ROI.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Items.Void
{
    internal class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofFright);
            item.width = 28;
            item.height = 40;
            item.maxStack = 999;
            item.value = 1000;
            item.rare = 3;
        }

        private bool used;
        public override void UpdateInventory(Player player)
        {
            var plr = player.GetModPlayer<ROIPlayer>();
            if (plr.voidCollector && !used)
            {
                if (plr.voidAffinity < plr.maxVoidAffinity)
                {
                    plr.voidAffinity += item.stack;
                    if (plr.voidAffinity > plr.maxVoidAffinity) plr.voidAffinity = plr.maxVoidAffinity;
                }
                used = true;
            }
            if (item.stack > 0 && --item.stack == 0) item.TurnToAir();
        }

        public override TagCompound Save() => new TagCompound { [nameof(used)] = used };

        public override void Load(TagCompound tag)
        {
            used = tag.GetBool(nameof(used));
        }
    }
}
