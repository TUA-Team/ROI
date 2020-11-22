namespace ROI.API.Loot
{
    /// <summary>
    /// A single item to spawn in a loot table
    /// </summary>
    public struct LootEntry : ILootRule
    {
        public readonly int Type;
        public readonly int Min;
        public readonly int Max;

        public LootEntry(int type) : this(type, 0)
        {
        }

        public LootEntry(int type, int stack) : this(type, stack, 0)
        {
        }

        public LootEntry(int type, int min, int max)
        {
            Type = type;
            Min = min;
            Max = max;
        }

        public void SpawnLoot(ILootTarget target)
        {
            target.Spawn(this);
        }
    }
}