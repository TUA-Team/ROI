using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI.Worlds.Pots.DropTable
{
    public class ItemPotsDrop : PotsDrop
    {
        private int itemID;
        private int itemQuantity;

        public ItemPotsDrop(string name, int itemID, int itemQuantity, Func<int, int, bool> successFunc) : base(name, successFunc)
        {
            this.itemID = itemID;
            this.itemQuantity = itemQuantity;
        }

        public override void ExecuteDrop(int x, int y)
        {
            Item.NewItem(x * 16, y * 16, 16, 16, itemID, itemQuantity);
        }

        
    }
}
