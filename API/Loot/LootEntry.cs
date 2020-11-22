namespace ROI.API.Loot
{
    /// <summary>
    /// A single item to spawn in a loot table
    /// </summary>
    public struct LootEntry
    {
        public readonly int Type;
        public readonly int Stack;

        public LootEntry(int type) : this(type, 0)
        {
        }

        public LootEntry(int type, int stack)
        {
            Type = type;
            Stack = stack;
        }
    }
}
