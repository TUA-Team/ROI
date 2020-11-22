using ROI.API.Loot;
using ROI.API.Loot.Chest;
using ROI.Content.Items;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding
{
    internal sealed class WastelandChestLoot : ChestLootTable
    {
        public WastelandChestLoot(int chest) : base(chest)
        {
        }


        protected override LootRule[] MakeTable() => new LootRule[]
        {
            new ChestTreasureRule
            {
                (new LootEntry(ROIMod.Instance.ItemType(nameof(Poutine))), 0.1)
            }
        };
    }
}
