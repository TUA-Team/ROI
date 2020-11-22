using System.Collections;
using System.Collections.Generic;

namespace ROI.API.Loot.General
{
    /// <summary>
    /// Base type for rules that have multiple entries
    /// </summary>
    public abstract class EntryCollectionRule : ILootRule, ICollection<(ILootRule rule, double weight)>
    {
        protected readonly HashSet<(ILootRule rule, double weight)> entries = new HashSet<(ILootRule rule, double weight)>();

        public abstract void SpawnLoot(ILootTarget target);

        public int Count => ((ICollection<(ILootRule rule, double weight)>)entries).Count;

        public bool IsReadOnly => ((ICollection<(ILootRule rule, double weight)>)entries).IsReadOnly;

        public void Add((ILootRule rule, double weight) item)
        {
            ((ICollection<(ILootRule rule, double weight)>)entries).Add(item);
        }

        public void Clear()
        {
            ((ICollection<(ILootRule rule, double weight)>)entries).Clear();
        }

        public bool Contains((ILootRule rule, double weight) item)
        {
            return ((ICollection<(ILootRule rule, double weight)>)entries).Contains(item);
        }

        public void CopyTo((ILootRule rule, double weight)[] array, int arrayIndex)
        {
            ((ICollection<(ILootRule rule, double weight)>)entries).CopyTo(array, arrayIndex);
        }

        public IEnumerator<(ILootRule rule, double weight)> GetEnumerator()
        {
            return ((IEnumerable<(ILootRule rule, double weight)>)entries).GetEnumerator();
        }

        public bool Remove((ILootRule rule, double weight) item)
        {
            return ((ICollection<(ILootRule rule, double weight)>)entries).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)entries).GetEnumerator();
        }
    }
}