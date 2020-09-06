using System;
using System.Linq;
using Terraria.ModLoader;

namespace API
{
    public abstract class CollectionLoader : BaseLoader
    {
        private byte _nextId = 0;
        private IHaveId[] _objects = new IdBasedObject[0];


        public IHaveId Add(IHaveId element) {
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
        public override void Initialize(Mod mod) {
            var parentType = typeof(T);
            foreach (var t in mod.Code.DefinedTypes.Where(t => parentType.IsAssignableFrom(t))) {
                if (t.IsAbstract) continue;
                if (t.IsEquivalentTo(parentType)) continue;

                base.Add((IHaveId)Activator.CreateInstance(t));
            }
        }

        public TUnique Add<TUnique>(TUnique element) where TUnique : T => (TUnique)base.Add(element);
        protected sealed override void OnAdd(IHaveId element) { OnAdd((T)element); }


        protected virtual void OnAdd<TUnique>(TUnique element) where TUnique : T { }


        public T Get() => this[IdHolder<T>.Id];

        public new T this[byte packetType] => (T)base[packetType];
    }
}
