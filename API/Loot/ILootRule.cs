namespace ROI.API.Loot
{
    /// <summary>
    /// A object that spawns some items for a target
    /// </summary>
    public interface ILootRule
    {
        void SpawnLoot(ILootTarget target);
    }
}
