using Terraria;
using Terraria.ModLoader;

namespace ROI.Items
{
    public sealed partial class ROIGlobalItem : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            OnHitNPCLore(item, player, target, damage knockBack, crit);
        }
    }
}
