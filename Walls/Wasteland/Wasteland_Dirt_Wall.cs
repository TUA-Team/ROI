using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Walls.Wasteland
{
    class Wasteland_Dirt_Wall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(104, 91, 87));
        }
    }

    class Wasteland_Dirt_Wall_Safe : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ROI/Walls/Wasteland/Wasteland_Dirt_Wall";
            return true;
        }

        public override void SetDefaults()
        {
            drop = mod.ItemType("Wasteland_Dirt_Wall");
            AddMapEntry(new Color(104, 91, 87));
        }
    }
}
