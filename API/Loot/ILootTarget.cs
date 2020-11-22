namespace ROI.API.Loot
{
    /// <summary>
    /// Defines some behavior for spawning an item
    /// </summary>
    public interface ILootTarget
    {
        void Spawn(LootEntry item);
    }
}
