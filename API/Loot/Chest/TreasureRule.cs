using ROI.API.Loot.General;
using System;
using System.Linq;
using Terraria;
using Terraria.Utilities;

namespace ROI.API.Loot.Chest
{
    public sealed class TreasureRule : EntryCollectionRule, ILootRule
    {
        public override void SpawnLoot(ILootTarget target)
        {
            var rand = new WeightedRandom<ILootRule>(WorldGen.genRand,
                theElements: entries.Select(x => Tuple.Create(x.rule, x.weight)).ToArray());

            rand.Get().SpawnLoot(target);
        }
    }
}
