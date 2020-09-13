using System;
using Terraria.ModLoader;

namespace API
{
    public interface IHaveId
    {
        void Init(Mod mod);

        void Link(Action<byte> update);

        byte MyId { get; set; }
    }
}
