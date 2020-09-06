using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ROI.Configs
{
    [Label("Music")]
    public sealed class MusicConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(WastelandMusicType.Techno)]
        [Label("Wasteland Music Type")]
        [Tooltip("Techno - Just more techno than legacy\n" +
            "Legacy - The old version of wasteland.mp3\n" +
            "Vanilla - Default Underworld music")]
        public WastelandMusicType WastelandMusic { get; set; }
    }

    public enum WastelandMusicType : byte
    {
        Techno,
        Legacy,
        Vanilla
    }
}