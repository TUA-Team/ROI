using ROI.Content.Biomes.Wasteland.WorldBuilding;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using ROI.Content.Biomes.Wasteland.WorldBuilding.Vines;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace ROI.Content.Biomes.Wasteland
{
    public class WastelandWorld : ModWorld
    {
        public static int wastelandTiles;
        public static WastelandVineContext vineContext;


        public override void Initialize()
        {
            vineContext = new WastelandVineContext();
        }

        public override TagCompound Save() => new TagCompound
        {
            [nameof(vineContext)] = vineContext.SerializeData()
        };

        public override void Load(TagCompound tag)
        {
            vineContext = WastelandVineContext.Deserialize(tag.GetCompound(nameof(vineContext)));
        }


        public override void PostUpdate()
        {
            vineContext.Update();
        }

        public override void PostDrawTiles()
        {
            vineContext.Draw(Main.spriteBatch);
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
