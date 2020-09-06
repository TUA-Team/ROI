using System;

namespace API
{
    public interface IHaveId
    {
        void Link(Action<byte> update);

        byte MyId { get; set; }
    }
}
