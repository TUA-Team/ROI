using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Worlds.Pots.DropTable;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Worlds.Pots
{
    abstract class ModPots : ModTile
    {
        public virtual bool CustomAutoLoad(ref string name, ref string texture)
        {
            return true;
        }

        public Point16 dimension;

        /// <summary>
        /// Send a list of item that can be modified,
        /// Add a new array of int[] to make your pots drop more than 1 item
        /// Setting an item to -1 will add a chance that your pots won't drop anything
        /// You can add to an array the same item multiple time to increase drop chance of said item 
        /// </summary>
        /// <returns></returns>
        public void SetDropTable(List<PotsDrop> drops)
        {
            
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}
