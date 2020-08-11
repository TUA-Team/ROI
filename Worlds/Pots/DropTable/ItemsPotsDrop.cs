using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI.Worlds.Pots.DropTable
{
    public class ItemsPotsDrop : PotsDrop
    {
        private readonly int[] _itemsDrop;
        private readonly int[] _maxQuantity;

        // new ItemPotsDrop()

        public ItemsPotsDrop(string name, int[] itemsDrop, int[] maxQuantity, Func<int, int, bool> successFunc) : base(name, successFunc)
        {
            this._itemsDrop = itemsDrop;
            this._maxQuantity = maxQuantity;
        }  

        public override void ExecuteDrop(int x, int y)
        {
            int itemIndex = Main.rand.Next(_itemsDrop.Length);
            Item.NewItem(x * 16, y * 16, 16, 16, _itemsDrop[itemIndex], Main.rand.Next(1, _maxQuantity[itemIndex]));
        }
    }

    public sealed class LayerBasedItemsPotsDrop : PotsDrop
    {

        private List<int[]> itemID;
        private List<int[]> itemQuantity;

        /// <summary>
        /// List order important<br />
        /// [0] = Surface
        /// [1] = Underground
        /// [2] = Cavern
        /// [3] = Underworld
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itemID"></param>
        /// <param name="itemQuantity"></param>
        /// <param name="successFunc"></param>
        public LayerBasedItemsPotsDrop(string name, List<int[]> itemID, List<int[]> itemQuantity, Func<int, int, bool> successFunc) : base(name, successFunc)
        {
            this.itemID = itemID;
            this.itemQuantity = itemQuantity;
        }

        public override void ExecuteDrop(int x, int y)
        {
            if ((double) y < Main.worldSurface)
            {
                int itemIndex = Main.rand.Next(itemID[0].Length);
                Item.NewItem(x * 16, y * 16, 16, 16, itemID[0][itemIndex], Main.rand.Next(1, itemQuantity[0][itemIndex]));
            } 
            else if ((double) y < Main.rockLayer)
            {
                int itemIndex = Main.rand.Next(itemID[1].Length);
                Item.NewItem(x * 16, y * 16, 16, 16, itemID[1][itemIndex], Main.rand.Next(1, itemQuantity[1][itemIndex]));
            }
            else if (y < Main.maxTilesY - 200)
            {
                int itemIndex = Main.rand.Next(itemID[2].Length);
                Item.NewItem(x * 16, y * 16, 16, 16, itemID[2][itemIndex], Main.rand.Next(1, itemQuantity[2][itemIndex]));
            }
            else
            {
                int itemIndex = Main.rand.Next(itemID[3].Length);
                Item.NewItem(x * 16, y * 16, 16, 16, itemID[3][itemIndex], Main.rand.Next(1, itemQuantity[3][itemIndex]));
            }
        }
    }
}
