namespace ROI.API.Loot
{
    /// <summary>
    /// A object that retrieves some items to spawn in a loot table
    /// </summary>
    public abstract class LootRule
    {
        public abstract LootEntry[] GetLoot();
    }
}
