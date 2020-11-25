using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.API.WorldBuilding
{
    public abstract class GenStep
    {
        protected Mod Mod { get; }

        protected GenerationProgress Progress { get; }

        public GenStep(Mod mod, GenerationProgress progress)
        {
            Mod = mod;
            Progress = progress;
        }

        public abstract void Make();
    }
}
