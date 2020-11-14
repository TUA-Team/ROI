using API.Terraria.Biomes;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace API.Terraria
{
    public sealed class SharedWorld : ModWorld
    {
        public override void PostDrawTiles()
        {
            var sb = Main.spriteBatch;
            sb.Begin();

            // TODO: Custom physics entities

            sb.End();
        }

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
