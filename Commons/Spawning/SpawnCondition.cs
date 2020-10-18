using Terraria.ModLoader;

namespace ROI.Commons.Spawning
{
    public abstract class SpawnCondition : ModType
    {
        protected override void Register()
        {
        }

        public abstract bool Active(int x, int y);

        public abstract float SpawnChance { get; }
    }
}