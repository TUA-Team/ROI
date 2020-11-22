using Terraria;
using Terraria.ModLoader;

namespace ROI.Commons.Spawning
{
    public abstract class SpawnCondition : ModType
    {
        protected override void Register()
        {
        }

        public abstract bool Active(Player player, int x, int y);

        public abstract float SpawnChance { get; }
    }
}