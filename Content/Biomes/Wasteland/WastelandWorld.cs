using ROI.API.Verlet.Contexts.Chains;
using ROI.Content.Biomes.Wasteland.WorldBuilding;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Content.Biomes.Wasteland
{
    public class WastelandWorld : ModWorld
    {
        public static int wastelandTiles;
        public static VerletChainContext vineContext;


        public override void Initialize()
        {
            vineContext = new VerletChainContext("Content/Biomes/Wasteland/WorldBuilding/Vines/WastelandLushVine");
        }


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            var index = tasks.FindIndex(x => x.Name.Equals("Granite"));

            if (index != -1)
            {
                tasks.Insert(index + 1, new PassLegacy("ROI: Wasteland", new WastelandWorldMaker(mod).Make));
            }
        }

        // TODO: (low prio) generalize this?
        public override void TileCountsAvailable(int[] tileCounts)
        {
            wastelandTiles = tileCounts[ModContent.TileType<WastelandDirt>()] +
                tileCounts[ModContent.TileType<WastelandRock>()] +
                tileCounts[ModContent.TileType<WastelandGrass>()];
        }
    }
}
