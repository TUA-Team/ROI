using ROI.API.Loot;
using ROI.API.Loot.Chest;
using ROI.API.Loot.General;
using ROI.Content.Items;
using Terraria.ID;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding
{
    internal sealed class WastelandChestLoot : ChestLootTable
    {
        public WastelandChestLoot(int chest) : base(chest)
        {
        }


        protected override ILootRule[] GetRules() => new ILootRule[]
        {
            new TreasureRule
            {
                (new LootEntry(ROIMod.Instance.ItemType(nameof(Poutine))), 0.1),
                (new LootEntry(ItemID.SuspiciousLookingEye), 2)
            },
            
            new GeneralPoolRule
            {
                (new TreasureRule
                {
                    (new LootEntry(ItemID.IronBar, 4, 11), 5),
                    (new LootEntry(ItemID.GoldBar, 3, 7), 1),
                    (new LootEntry(ItemID.SilverBar, 6, 8), 6),
                    (new LootEntry(ItemID.CopperBar, 2, 16), 3),
                }, 5),
                (new LootEntry(ItemID.IronOre, 20, 31), 5),
                (new LootEntry(ItemID.Torch, 7, 12), 7)
            }
        };
    }
}
