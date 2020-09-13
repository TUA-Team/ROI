using System;
using System.Diagnostics;
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
            Debug.Assert(element != null, $"CollectionLoader: {nameof(element)} is null");

            Array.Resize(ref _objects, _nextId + 1);

            _objects[_nextId] = element;

            element.MyId = _nextId++;

            OnAdd?.Invoke(element);

            return element;
        }


        public event Action<IHaveId> OnAdd;


        public IHaveId this[byte packetType] => _objects[packetType];

        public int Count => _nextId;
    }

    public class CollectionLoader<T> : CollectionLoader where T : IHaveId
    {
        public override void Initialize(Mod mod)
        {
            var parentType = typeof(T);
            OnAdd += element => IdHolder.Register(element);

            foreach (var t in mod.Code.DefinedTypes.Where(t => parentType.IsAssignableFrom(t)))
            {
                if (t.IsAbstract) continue;
                if (t.IsEquivalentTo(parentType)) continue;

                Add((IHaveId)Activator.CreateInstance(t));
            }
        }

        public new event Action<T> OnAdd
        {
            add => base.OnAdd += instance => value((T)instance);
            remove => base.OnAdd -= instance => value((T)instance);
        }

        public T Get<TUnique>() where TUnique : T => (T)base[IdHolder<TUnique>.Id];
        public new T this[byte packetType] => (T)base[packetType];
    }
}
