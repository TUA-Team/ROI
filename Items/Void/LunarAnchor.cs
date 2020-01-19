using ROI.Players;
using Terraria;

namespace ROI.Items.Void
{
    internal abstract class LunarAnchor : VoidItem
    {
        public override void UpdateInventory(Player player)
        {
            var plr = player.GetModPlayer<ROIPlayer>();
            plr.lunarAmassMax = System.Math.Max(plr.lunarAmassMax, Affinity);
        }
    }
}