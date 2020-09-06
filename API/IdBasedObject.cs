using System;

namespace API
{
    public abstract class IdBasedObject : IHaveId
    {
        public void Link(Action<byte> update) {
            update(MyId);
        }

        public byte MyId { get; set; }
    }
}
