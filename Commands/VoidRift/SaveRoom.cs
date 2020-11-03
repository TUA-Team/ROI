#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Manager;
using Terraria.ModLoader;

namespace ROI.Commands.VoidRift
{
    /// <summary>
    /// Usage:
    /// saveRoom <roomType> <roomName>
    ///
    /// Possible room type:
    /// Normal
    /// Boss
    /// Item
    /// Blessing
    /// Planetary
    /// Hall
    /// </summary>
    class SaveRoom : ModCommand
    {
        public override bool Autoload(ref string name)
        {
            return DevManager.Instance.CheckDev();
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            
        }

        public override string Command => "SaveRoom";
        public override CommandType Type => CommandType.World;
    }
}
#endif