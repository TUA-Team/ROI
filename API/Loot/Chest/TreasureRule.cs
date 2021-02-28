using ROI.API.Loot.General;
using System;
using System.Linq;
using Terraria;
using Terraria.Utilities;

namespace ROI.API.Loot.Chest
{
    public class TreasureRule : EntryCollectionRule
    {
        public override void SpawnLoot(LootTarget target)
        {
            var rand = new WeightedRandom<LootRule>(WorldGen.genRand,
                theElements: entries.Select(x => Tuple.Create(x.rule, x.weight)).ToArray());

            rand.Get().SpawnLoot(target);
        }
    }
}
