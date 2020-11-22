using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ROI.API.Loot.General
{
    public sealed class GeneralPoolRule : EntryCollectionRule
    {
        public override LootEntry[] GetLoot()
        {
            var list = new List<LootEntry>();
            var avg = entries.Sum(x => x.weight) / entries.Count;
            var rand = WorldGen.genRand.NextDouble() * avg;

            foreach (var (item, weight) in entries)
            {
                if (weight > rand)
                    list.Add(item);
            }

            return list.ToArray();
        }
    }
}
