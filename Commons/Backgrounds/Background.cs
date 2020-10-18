using API;
using Terraria.ModLoader;

namespace ROI.Commons.Backgrounds
{
    // TODO: (low prio) should eventually be removed
    public abstract class Background : IHaveId
    {
        public abstract void Load(Mod mod);

        public void Unload()
        {
        }

        public int MyId { get; set; }
    }
}
