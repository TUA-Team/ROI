using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROI.Configs
{
    [Label("Debug")]
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        [Label("Generate Wasteland")]
        [Tooltip("Generate Wasteland terrain instead of the Underworld.")]
        public bool GenWasteland { get; set; }

        [DefaultValue(false)]
        [Label("Debug Command")]
        [Tooltip("Allows you to use /debugroi, although there usually won't be any effect.")]
        public bool DebugCommmand { get; set; }
    }
}
