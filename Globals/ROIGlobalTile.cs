using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Worlds.Pots;
using ROI.Worlds.Pots.DropTable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals
{
    class ROIGlobalTile : GlobalTile
    {
        public override void SetDefaults()
        {
            List<PotsDrop> drop = PotsRegistery.ModifyPotsDrop((int) PotsTypeID.Lihzahrd);
            drop.Insert(0, new ItemPotsDrop("brick", ItemID.LihzahrdBrick, 10, (i, i1) => Main.rand.NextBool()));
            drop = PotsRegistery.ModifyPotsDrop((int) PotsTypeID.Hell);
            drop.Insert(0, new ItemPotsDrop("Obsidian", ItemID.Obsidian, 7, (i, i1) => Main.rand.Next(25) == 0));
            drop.Insert(1, new ItemPotsDrop("Hellstone", ItemID.Hellstone, 3, (i, i1) => Main.rand.Next(35) == 0));
        }

        public override bool Drop(int i, int j, int type)
        {
            
            if (type == 28)
            {
                
                int potsType = Main.tile[i, j].type;
                int potsIndex = Main.tile[i, j].frameY / 18;
                //Divide by 2 to get the right potion sprite and then -1 to get the right index
                if (potsIndex > 1)
                    potsType -= 1;
                    potsIndex = (int) (Math.Floor(potsType / 3f));
                //Get the actual pots type
                //Then get the coin modifier based on what type it is
                PotsRegistery.ExecuteDrop(i, j, potsIndex);
                return false;
            }
            return base.Drop(i, j, type);
        }
    }
}
