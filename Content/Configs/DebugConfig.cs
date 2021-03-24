using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROI.Content.Configs
{
    [Label("Debug")]
    public class DebugConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        [Label("Download Nightly Builds")]
        [Tooltip("Opens the download link in your browser when nightly builds are released.\n" +
            "Only use if you know what you are doing!.\n" +
            "Redownload from the Mod Browser to get stable.")]
        [ReloadRequired]
        public bool Nightly { get; set; }

        [DefaultValue(false)]
        [Label("Debug Mode")]
        [Tooltip("Enables all kinds of developer features.")]
        [ReloadRequired]
        public bool DebugMode { get; set; }
    }
}