namespace ROI.Core.Loot
{
    /// <summary>
    /// Defines some behavior for spawning an item
    /// </summary>
    public abstract class LootTarget
    {
        public abstract void Spawn(LootEntry item);
    }
}
