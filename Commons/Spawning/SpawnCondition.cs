using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Commons.Spawning.Biomes;
using ROI.Commons.Spawning.Walls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Commons.Spawning
{
    abstract class SpawnCondition
    {
        protected abstract bool Active(int x, int y);
        protected abstract float SpawnChance { get; }


        protected static Mod mod => ROIMod.instance;

        public static Wasteland Wasteland = new Wasteland();
        public static Unsafe_Wasteland_Dirt_Wall UnsafeWastelandDirtWall = new Unsafe_Wasteland_Dirt_Wall();
        public static Unsafe_Wasteland_Rock_Wall UnsafeWastelandRockWall = new Unsafe_Wasteland_Rock_Wall();
    }
}
