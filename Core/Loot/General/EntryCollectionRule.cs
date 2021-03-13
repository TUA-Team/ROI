using System.Collections;
using System.Collections.Generic;

namespace ROI.Core.Loot.General
{
    /// <summary>
    /// Base type for rules that have multiple entries
    /// </summary>
    public abstract class EntryCollectionRule : LootRule, ICollection<(LootRule rule, double weight)>
    {
        protected readonly HashSet<(LootRule rule, double weight)> entries = new HashSet<(LootRule rule, double weight)>();

        public int Count => ((ICollection<(LootRule rule, double weight)>)entries).Count;

        public bool IsReadOnly => ((ICollection<(LootRule rule, double weight)>)entries).IsReadOnly;

        public void Add((LootRule rule, double weight) item)
        {
            ((ICollection<(LootRule rule, double weight)>)entries).Add(item);
        }

        public void Clear()
        {
            ((ICollection<(LootRule rule, double weight)>)entries).Clear();
        }

        public bool Contains((LootRule rule, double weight) item)
        {
            return ((ICollection<(LootRule rule, double weight)>)entries).Contains(item);
        }

        public void CopyTo((LootRule rule, double weight)[] array, int arrayIndex)
        {
            ((ICollection<(LootRule rule, double weight)>)entries).CopyTo(array, arrayIndex);
        }

        public IEnumerator<(LootRule rule, double weight)> GetEnumerator()
        {
            return ((IEnumerable<(LootRule rule, double weight)>)entries).GetEnumerator();
        }

        public bool Remove((LootRule rule, double weight) item)
        {
            return ((ICollection<(LootRule rule, double weight)>)entries).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)entries).GetEnumerator();
        }
    }
}