using System;

namespace API
{
    public abstract class CollectionLoader : BaseLoader
    {
        private byte _nextId = 0;
        private IdBasedObject[] _objects = new IdBasedObject[0];


        public IdBasedObject Add(IdBasedObject element) {
            Array.Resize(ref _objects, _nextId + 1);

            _objects[_nextId] = element;

            element.MyId = _nextId++;

            IdHolder.Register(element);

            return element;
        }


        public IdBasedObject this[byte packetType] => _objects[packetType];

        public int Count => _nextId;
    }

    public abstract class CollectionLoader<T> : CollectionLoader where T : IdBasedObject
    {
        public TUnique Add<TUnique>(TUnique element) where TUnique : T => base.Add(element) as TUnique;


        public T Get() => this[IdHolder<T>.Id];

        public new T this[byte packetType] => base[packetType] as T;
    }
}
