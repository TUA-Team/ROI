using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Worlds.Pots.DropTable
{


    public abstract class PotsDrop
    {
        public string Name { get; private set; }

        public abstract void ExecuteDrop(int x, int y);

        /// <summary>
        /// If return true, this drop is executed
        /// </summary>
        /// <returns></returns>
        public readonly Func<int, int, bool> successFunc = (int x, int y) => { return false; };

        protected PotsDrop(string name, Func<int, int, bool> successFunc)
        {
            this.successFunc = successFunc;
        }
    }
}
