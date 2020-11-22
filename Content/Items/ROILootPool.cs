using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ROI.Content.Items
{
    public abstract class ROILootPool
    {
		protected ROIMod Mod => ModContent.GetInstance<ROIMod>();

        protected struct LootEntry
        {
            public readonly float weight;
            public readonly int type;
			public Predicate<float> valid;

			public LootEntry(float weight, int type) : this(weight, type, x => weight > x)
            {
            }

			public LootEntry(float weight, int type, Predicate<float> valid)
            {
				this.weight = weight;
				this.type = type;
				this.valid = valid;
            }
		}

		protected int index = 0;
        protected readonly Item[] items;
        protected readonly LootEntry[] table;

        public ROILootPool(Item[] chest)
        {
			items = chest;
			table = MakeTable();
        }


        protected abstract LootEntry[] MakeTable();


        public virtual void Fill()
        {
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
            }
        }
	}
}
