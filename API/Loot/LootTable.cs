namespace ROI.API.Loot
{
    /// <summary>
    /// An overall table of various possible loot values
    /// </summary>
    public abstract class LootTable
    {
        private readonly ILootTarget Target;
        private readonly ILootRule[] Rules;

        public LootTable(ILootTarget target)
        {
            Target = target;
            Rules = GetRules();
        }


        /// <summary>
        /// The loot rules to use
        /// </summary>
        /// <returns></returns>
        protected abstract ILootRule[] GetRules();


        /// <summary>
        /// Pick values from the table
        /// </summary>
        public void Populate()
        {
            foreach (var rule in Rules)
            {
                rule.SpawnLoot(Target);
            }
        }
    }
}
