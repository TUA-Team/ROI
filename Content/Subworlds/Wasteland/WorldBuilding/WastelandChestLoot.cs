using ROI.Content.Items;

namespace ROI.Content.Subworlds.Wasteland.WorldBuilding
{
    internal sealed class WastelandChestLoot : ROIChestLoot
    {
        public WastelandChestLoot(int chest) : base(chest)
        {
        }

        protected override LootEntry[] MakeTable() => new LootEntry[]
        {
            // Debug entry
            new LootEntry(0, Mod.ItemType(nameof(Poutine)), x => ROIMod.DEBUG),
        };

        protected override LootEntry[] GetChestPool() => new LootEntry[]
        {

        };

        public override void Fill()
        {
            //items[index++].SetDefaults();

            base.Fill();
        }
    }
}
