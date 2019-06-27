using ROI.ID;
using Terraria;
using Terraria.ID;

namespace ROI.Globals
{
    internal partial class ROIGlobalItem
    {
        private void LoreOnHitNPC(Item item)
        {
            if (item.type == ItemID.CopperShortsword)
                Manager.LoreManager.Instance.TriggerLoreEntry(LoreID.CopperShortsword);
        }
    }
}
