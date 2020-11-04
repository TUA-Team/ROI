using API.Terraria.EntityComponents;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace API.Terraria.Biomes
{
    public abstract class BiomeBase:EntityComponent,IHaveId
    {
        public void Load(Mod mod)
        {
            IdHookLookup<BiomeBase>.Register(this);
        }

        public void Unload()
        {
            
        }


        public virtual void CopyCustomBiomesTo(Player other)
        {
            var modPlayer = other.GetModPlayer<StandardPlayer>();
            var registry = modPlayer.BiomeRegistry;
            var biome = registry.RegisteredComponents.GetComponent(GetType());
            biome.Active = Active;
        }

        public abstract void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight);

        protected virtual bool IsBiomeActive()
        {
            return false;
        }

        public sealed override void UpdateComponent()
        {
            Active = IsBiomeActive();
        }


        public bool Active { get; private set; }


        public int MyId { get; set; }
    }
}
