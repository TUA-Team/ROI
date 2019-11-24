using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROI.Configs
{
    [Label("Music")]
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        [Label("Generate Wasteland")]
        [Tooltip("Generate Wasteland terrain instead of the Underworld.")]
        public bool GenWasteland { get; set; }
    }
}
