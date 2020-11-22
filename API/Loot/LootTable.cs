namespace ROI.API.Loot
{
    /// <summary>
    /// An overall table of various possible loot values
    /// </summary>
    public abstract class LootTable
    {
        private readonly LootRule[] table;

        public LootTable()
        {
            table = MakeTable();
        }


        /// <summary>
        /// The loot rules to use
        /// </summary>
        /// <returns></returns>
        protected abstract LootRule[] MakeTable();

        /// <summary>
        /// What to do when an item is picked
        /// </summary>
        /// <param name="loot"></param>
        protected abstract void SpawnItem(LootEntry loot);


        /// <summary>
        /// Pick values from the table
        /// </summary>
        public void Populate()
        {
            foreach (var entry in table)
            {
                foreach (var item in entry.GetLoot())
                {
                    SpawnItem(item);
                }
            }
        }
    }
}
