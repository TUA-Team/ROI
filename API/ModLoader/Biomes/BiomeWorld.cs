using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace API.ModLoader.Biomes
{
    public sealed class BiomeWorld : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            for (int i = 0; i < IdHookLookup<BiomeBase>.Instances.Count; i++)
            {
                var biome = IdHookLookup<BiomeBase>.Instances[i];
                biome.ModifyWorldGenTasks(tasks, ref totalWeight);
            }
        }
    }
}
