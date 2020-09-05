using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.IO;

namespace ROI.Patches
{
    partial class Patch
    {
        /// <summary>
        /// Original:
        /// if (num17 < Main.maxTilesY - 200)
        /// New:
        /// if ((num17 < Main.maxTilesY - 200 && Main.ActiveWorldFileData.HasCorruption) ||
        /// (num17 < Main.maxTile - 200 && Main.ActiveWorldFileData.HasCrimson && Main.hardMode))
        /// </summary>
        /// <param name="context"></param>
        public static void ILLightingPreRenderPhase(ILContext context)
        {
            ILCursor c = new ILCursor(context);

            ILLabel label = null;
            int value = 0;

            if (c.TryGotoNext(i => i.MatchLdloc(out value),
                i => i.MatchLdsfld(out _),
                i => i.MatchLdcI4(out _),
                i => i.MatchSub(),
                i => i.MatchBle(out label)))
            {
                c.Index += 5;
                c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.ActiveWorldFileData)));
                c.Emit(OpCodes.Ldfld, typeof(WorldFileData).GetField(nameof(Main.ActiveWorldFileData.HasCorruption)));
                c.Emit(OpCodes.Brtrue_S, label);
                c.Emit(OpCodes.Ldloc_S, (byte) value);
                c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.maxTilesY)));
                c.Emit(OpCodes.Ldc_I4, 0xC8);
                c.Emit(OpCodes.Sub);
                c.Emit(OpCodes.Ble, label);
                c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.ActiveWorldFileData)));
                c.Emit(OpCodes.Callvirt, typeof(WorldFileData).GetProperty(nameof(Main.ActiveWorldFileData.HasCrimson)).GetMethod);
                c.Emit(OpCodes.Brfalse, label);
                c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.hardMode)));
                c.Emit(OpCodes.Brfalse, label);
            }
        }
    }
}
