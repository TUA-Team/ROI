using System;
using System.Linq;
using Terraria.ModLoader;

namespace API
{
    public abstract class CollectionLoader : BaseLoader
    {
        private byte _nextId = 0;
        private IHaveId[] _objects = new IHaveId[0];


        public IHaveId Add(IHaveId element)
        {
            Array.Resize(ref _objects, _nextId + 1);

            _objects[_nextId] = element;

            element.MyId = _nextId++;

            IdHolder.Register(element);

            OnAdd(element);

            return element;
        }


        protected virtual void OnAdd(IHaveId element) { }


        public IHaveId this[byte packetType] => _objects[packetType];

        public int Count => _nextId;
    }

    public class CollectionLoader<T> : CollectionLoader where T : IHaveId
    {
        public override void Initialize(Mod mod)
        {
            var parentType = typeof(T);
            foreach (var t in mod.Code.DefinedTypes.Where(t => parentType.IsAssignableFrom(t)))
            {
                if (t.IsAbstract) continue;
                if (t.IsEquivalentTo(parentType)) continue;

                Add((IHaveId)Activator.CreateInstance(t));
            }
        }


        public event Action<T> OnAddEvent;

        protected sealed override void OnAdd(IHaveId element)
        {
            OnAddEvent((T)element);
        }


        public T Get<TUnique>() where TUnique : T => this[IdHolder<TUnique>.Id];

        public new T this[byte packetType] => (T)base[packetType];
    }
}
