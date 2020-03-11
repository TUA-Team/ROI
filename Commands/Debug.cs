using ROI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace ROI.Commands
{
    internal class Debug : ModCommand
    {
        public override string Command => "debugroi";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!ROIMod.DebugConfig.DebugCommmand) return;
            switch (args[0])
            {
                case "voidaff":
                    var plr = caller.Player.GetModPlayer<Players.ROIPlayer>();
                    if (plr.maxVoidAffinity == 0) plr.maxVoidAffinity = 50;
                    plr.voidAffinity = Convert.ToInt32(args[1]);
                    break;
                case "localize":
                    Mod mod = ModLoader.GetMod("ROI");
                    var dictionary = (Dictionary<string, ModTranslation>)mod.GetField<IDictionary<string, ModTranslation>>("translations");
                    var result = mod.GetField<IDictionary<string, ModItem>>("items").Where(x => !dictionary.ContainsValue(x.Value.DisplayName)).Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetDefault()}")
                       .Concat(mod.GetField<IDictionary<string, ModItem>>("items").Where(x => !dictionary.ContainsValue(x.Value.Tooltip)).Select(x => $"{x.Value.Tooltip.Key}={x.Value.Tooltip.GetDefault()}"))
                       .Concat(mod.GetField<IDictionary<string, ModNPC>>("npcs").Where(x => !dictionary.ContainsValue(x.Value.DisplayName)).Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetDefault()}"))
                       .Concat(mod.GetField<IDictionary<string, ModBuff>>("buffs").Where(x => !dictionary.ContainsValue(x.Value.DisplayName)).Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetDefault()}"))
                       .Concat(mod.GetField<IDictionary<string, ModBuff>>("buffs").Where(x => !dictionary.ContainsValue(x.Value.Description)).Select(x => $"{x.Value.Description.Key}={x.Value.Description.GetDefault()}"))
                       .Concat(mod.GetField<IDictionary<string, ModProjectile>>("projectiles").Where(x => !dictionary.ContainsValue(x.Value.DisplayName)).Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetDefault()}"));
                    int index = $"Mods.ROI.".Length;
                    result = result.Select(x => x.Remove(0, index));
                    ReLogic.OS.Platform.Current.Clipboard = string.Join("\n", result);
                    break;
            }
        }
    }
}