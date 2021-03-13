using System.Linq;
using Terraria;

namespace ROI.Core.Loot.General
{
    public class GeneralPoolRule : EntryCollectionRule
    {
        public override void SpawnLoot(LootTarget target)
        {
            var sum = entries.Sum(x => x.weight);
            var rand = WorldGen.genRand.NextDouble() * sum;

            foreach (var (item, weight) in entries)
            {
                if (weight <= rand)
                {
                    item.SpawnLoot(target);
                    continue;
                }

                rand -= weight;
            }
        }
    }
}
