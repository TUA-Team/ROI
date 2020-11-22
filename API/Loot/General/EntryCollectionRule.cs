using System.Collections;
using System.Collections.Generic;

namespace ROI.API.Loot.General
{
    /// <summary>
    /// Base type for all rules that have multiple entries
    /// </summary>
    public abstract class EntryCollectionRule : LootRule, ICollection<(LootEntry item, double weight)>
    {
        protected readonly HashSet<(LootEntry item, double weight)> entries = new HashSet<(LootEntry item, double weight)>();

        public int Count => ((ICollection<(LootEntry item, double weight)>)entries).Count;

        public bool IsReadOnly => ((ICollection<(LootEntry item, double weight)>)entries).IsReadOnly;

        public void Add((LootEntry item, double weight) item)
        {
            ((ICollection<(LootEntry item, double weight)>)entries).Add(item);
        }

        public void Clear()
        {
            ((ICollection<(LootEntry item, double weight)>)entries).Clear();
        }

        public bool Contains((LootEntry item, double weight) item)
        {
            return ((ICollection<(LootEntry item, double weight)>)entries).Contains(item);
        }

        public void CopyTo((LootEntry item, double weight)[] array, int arrayIndex)
        {
            ((ICollection<(LootEntry item, double weight)>)entries).CopyTo(array, arrayIndex);
        }

        public IEnumerator<(LootEntry item, double weight)> GetEnumerator()
        {
            return ((IEnumerable<(LootEntry item, double weight)>)entries).GetEnumerator();
        }

        public bool Remove((LootEntry item, double weight) item)
        {
            return ((ICollection<(LootEntry item, double weight)>)entries).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)entries).GetEnumerator();
        }
    }
}
