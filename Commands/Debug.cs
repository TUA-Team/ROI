using System;
using Terraria.ModLoader;

namespace ROI.Commands
{
    internal class Debug : ModCommand
    {
        public override string Command => "debugroi";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!ModContent.GetInstance<Configs.DebugConfig>().DebugCommmand) return;
            var plr = caller.Player.GetModPlayer<Players.ROIPlayer>();
            if (plr.maxVoidAffinity == 0) plr.maxVoidAffinity = 50;
            plr.voidAffinity = Convert.ToInt32(args[0]);
        }
    }
}
