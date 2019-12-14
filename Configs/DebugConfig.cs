using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROI.Configs
{
    [Label("Debug")]
    public sealed class DebugConfig : ModConfig
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

        [DefaultValue(false)]
        [Label("Download Nightly Builds")]
        [Tooltip("Opens the download link in your browser when nightly builds are released.\n" +
            "Only use if you know what you are doing!.\n" +
            "Redownload from the Mod Browser to get stable.")]
        public bool Nightly { get; set; }
    }
}
