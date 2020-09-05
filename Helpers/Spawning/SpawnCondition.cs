using Terraria.ModLoader;

namespace ROI.Helpers.Spawning
{
    public abstract class SpawnCondition
    {
        public abstract bool Active(int x, int y);

        public Mod Mod { get; internal set; }

        public abstract float SpawnChance { get; }
    }
}