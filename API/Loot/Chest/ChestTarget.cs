using Terraria;

namespace ROI.API.Loot.Chest
{
    public class ChestTarget : LootTarget
    {
        private readonly Item[] items;

        public ChestTarget(int chest)
        {
            items = Main.chest[chest].item;
        }

        public override void Spawn(LootEntry item)
        {
            int index;
            do
            {
                index = WorldGen.genRand.Next(items.Length);
            }
            while (!items[index].IsAir);

            Item chestItem = items[index];
            chestItem.SetDefaults(item.Type);
            chestItem.stack = item.Min > item.Max ? item.Min : WorldGen.genRand.Next(item.Min, item.Max);
        }
    }
}
