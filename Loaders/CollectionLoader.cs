using ROI.Models;
using System;
using Terraria.ModLoader;

namespace ROI.Loaders
{
    public abstract class CollectionLoader<T> : BaseLoader where T : IdBasedObject
    {
        private byte _nextId = 0;
        private T[] _objects = new T[0];


        public T Add(T networkPacket) {
            Array.Resize(ref _objects, _nextId + 1);

            _objects[_nextId] = networkPacket;

            networkPacket.MyId = _nextId;
            _nextId++;

            ContentInstance.Register(networkPacket);

            return networkPacket;
        }


        public TUnique Get<TUnique>() where TUnique : T {
            return this[ContentInstance<T>.Instance.MyId] as TUnique;
        }

        public T this[byte packetType] => _objects[packetType];

        public int Count => _nextId;
    }
}
