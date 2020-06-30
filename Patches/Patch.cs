using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Patches
{
    static partial class Patch
    {
        internal static void Load()
        {
            On.Terraria.Main.DrawWoF += DrawBossTongues;
        } 

        internal static void Unload()
        {
            On.Terraria.Main.DrawWoF -= DrawBossTongues;
        } 
    }
}
