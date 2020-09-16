using ROI.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace ROI.Content.Items.Void
{
    internal class VoidFragment : ROIItem
    {
        public VoidFragment() : base("Void Affinity", "", 28, 40, 1000, -1, 3, 999) { }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofFright);
            base.SetDefaults();
        }

        private bool used;
        public override void UpdateInventory(Player player)
        {
            var plr = player.GetModPlayer<ROIPlayer>();
            if (plr.voidCollector && !used)
            {
                if (plr.VoidAffinity < plr.MaxVoidAffinity)
                {
                    plr.VoidAffinity += (short)item.stack;
                    if (plr.VoidAffinity > plr.MaxVoidAffinity) plr.VoidAffinity = plr.MaxVoidAffinity;
                }
                used = true;
            }
            if (item.stack > 0 && --item.stack == 0) item.TurnToAir();
        }

        public override TagCompound Save() => new TagCompound { [nameof(used)] = used };

        public override void Load(TagCompound tag) => used = tag.GetBool(nameof(used));
    }
}
