using ROI.API.Loot.General;
using System;
using System.Linq;
using Terraria;
using Terraria.Utilities;

namespace ROI.API.Loot.Chest
{
    /// <summary>
    /// A weighted collection of possible treasure items for a chest
    /// </summary>
    public sealed class ChestTreasureRule : EntryCollectionRule
    {
        public override LootEntry[] GetLoot()
        {
            var rand = new WeightedRandom<LootEntry>(WorldGen.genRand,
                theElements: entries.Select(x => Tuple.Create(x.item, x.weight)).ToArray());

            return new LootEntry[] { rand.Get() };
        }
    }
}
