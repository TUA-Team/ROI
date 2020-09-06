using System;

namespace API
{
    public abstract class IdBasedObject
    {
        public byte MyId { get; internal set; }

        internal void Link(Action<byte> update) {
            update(MyId);
        }
    }
}
