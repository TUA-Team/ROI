namespace ROI.API.Loot
{
    /// <summary>
    /// A object that spawns some items for a target
    /// </summary>
    public abstract class LootRule
    {
        public abstract void SpawnLoot(LootTarget target);
    }
}
