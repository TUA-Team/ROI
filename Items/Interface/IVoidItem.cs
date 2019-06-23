using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Items.Interface
{
    internal interface IVoidItem
    {
        /// <summary>
        /// 2 essential variable
        /// </summary>
        int VoidTier { get; set; }
        int VoidCost { get; set; }

        /// <summary>
        /// To use with void tier
        /// </summary>
        void VoidTierEffect();
    }
}
