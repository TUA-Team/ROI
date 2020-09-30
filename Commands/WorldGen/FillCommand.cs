using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROI.Helpers;
using Terraria.ModLoader;

namespace ROI.Commands.WorldGen
{
    class FillCommand : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            int x1 = (int) (caller.Player.position.X / 16);
            int y1 = (int) (caller.Player.position.Y / 16);

            int x2 = int.Parse(args[0]);
            int y2 = int.Parse(args[1]);

            Mod mod = ModLoader.GetMod(args[2]);
            ushort tileType = (ushort) mod.TileType(args[3]);

            ROIWorldGenHelper.FillTile(x1, y1, x2, y2, new []{tileType}, new ushort[]{1}, false);
        }

        public override string Command => "fill";
        public override CommandType Type => CommandType.World;
    }
}
