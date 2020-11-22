namespace ROI.API.Loot.Chest
{
    public abstract class ChestLootTable : LootTable
    {
        public ChestLootTable(int chest) : base(new ChestTarget(chest))
        {
        }
    }
}
