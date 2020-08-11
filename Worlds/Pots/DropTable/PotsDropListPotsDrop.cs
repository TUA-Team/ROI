using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI.Worlds.Pots.DropTable
{
    class PotsDropListPotsDrop : PotsDrop
    {
        private List<PotsDrop> possibleDropsList = new List<PotsDrop>();

        public PotsDropListPotsDrop(string name, Func<int, int,bool> successFunc, List<PotsDrop> possibleDropsList) : base(name, successFunc)
        {
            this.possibleDropsList = possibleDropsList;
        }

        public override void ExecuteDrop(int x, int y)
        {
            possibleDropsList[Main.rand.Next(possibleDropsList.Count - 1)].ExecuteDrop(x, y);
        }
    }
}
