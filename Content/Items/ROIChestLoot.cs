using System.Linq;
using Terraria;

namespace ROI.Content.Items
{
    internal abstract class ROIChestLoot : ROILootPool
    {
        public ROIChestLoot(int chest) : base(Main.chest[chest].item)
        {
        }

        protected abstract LootEntry[] GetChestPool();

        public override void Fill()
        {
            var table = GetChestPool();
            float totalWeight = table.Sum(x => x.weight);
            // Just a percentage of the full weight
            var num = WorldGen.genRand.NextFloat() * totalWeight;
            foreach (var entry in table)
            {
                if (!entry.valid(num))
                {
                    num -= entry.weight;
                    continue;
                }

                items[index++].SetDefaults(entry.type);
                break;
            }

            base.Fill();
        }
    }
}
