using Terraria;

namespace ROI.API.Loot.Chest
{
    /// <summary>
    /// A table of items to spawn inside a chest
    /// </summary>
    public abstract class ChestLootTable : LootTable
    {
        private int index = 0;
        private readonly Item[] items;

        public ChestLootTable(int chest)
        {
            items = Main.chest[chest].item;
        }

        protected sealed override void SpawnItem(LootEntry loot)
        {
            var chestItem = items[index++];
            chestItem.SetDefaults(loot.Type);
            chestItem.stack = loot.Stack;
        }
    }
}
