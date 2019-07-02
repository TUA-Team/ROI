using Terraria;
using Terraria.ModLoader;

namespace ROI.Globals
{
    internal partial class ROIGlobalItem : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            LoreOnHitNPC(item);
        }
    }
}
