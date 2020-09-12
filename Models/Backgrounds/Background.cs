using API;
using System;
using Terraria.ModLoader;

namespace ROI.Models.Backgrounds
{
    // TODO: (low prio) should eventually be removed
    public abstract class Background : IHaveId
    {
        public abstract void Init(Mod mod);


        public void Link(Action<byte> update)
        {
            update(MyId);
        }

        public byte MyId { get; set; }
    }
}
