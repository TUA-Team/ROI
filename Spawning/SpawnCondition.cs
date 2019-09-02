using ROI.Spawning.Wasteland;
using ROI.Spawning.Wasteland.Walls;
using Terraria.ModLoader;

namespace ROI.Spawning
{
    public abstract class SpawnCondition
    {
        public abstract bool Active(int x, int y);

        public Mod Mod { get; internal set; }

        public abstract float SpawnChance { get; }
    }
}