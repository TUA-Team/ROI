using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    internal sealed partial class ROIWorld : ModWorld
    {
        public static Point16[] voidLeaks;

        public override void Initialize()
        {
            voidLeaks = new Point16[0];
        }

        public override void Load(TagCompound tag)
        {
            voidLeaks = tag.GetList<Point16>(nameof(voidLeaks)).ToArray();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                //the constructor is possibly unnecessary
                [nameof(voidLeaks)] = new List<Point16>(voidLeaks)
            };
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if (ROIMod.DebugConfig.GenWasteland)
            {
                int index = tasks.FindIndex(x => x.Name == "Underworld");
                tasks.RemoveAll(x => x.Name == "Underworld");
                tasks.RemoveAll(x => x.Name == "Hellforge");
                if (index != -1)
                {
                    tasks.Insert(index + 1, new PassLegacy("ROI: Wasteland", WastelandGeneration));
                }
            }
        }

        public override void PostWorldGen()
        {
            var list = new List<Point16>();
            //does not work on improperly sized worlds
            for (int x = 0; x < Main.maxTilesX / 200; x++)
            {
                for (int y = (int)Main.worldSurface / 150; y < ((int)Main.rockLayer / 150) + 1; y++)
                {
                    //about 2-3 leaks per small world
                    //((200*150)/(4200*1200))*100*100*3 = 178.571428 571428 571428...
                    var genLeak = WorldGen.genRand.Next(10000) <= 178;
                    if (!genLeak) continue;
                    list.Add(new Point16(x + 100, y + 75));
                }
            }
            voidLeaks = list.ToArray();
        }
    }
}