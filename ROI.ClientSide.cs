using ROI.Content.Configs;
using ROI.Players;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod
    {
        public MusicConfig MusicConfig;
        public DebugConfig DebugConfig;

        private void LoadClient()
        {
            MusicConfig = ModContent.GetInstance<MusicConfig>();
            DebugConfig = ModContent.GetInstance<DebugConfig>();

            AddMusicBox(GetSoundSlot(SoundType.Music, "Assets/Sounds/Music/Terra"),
                ModContent.ItemType<Content.Items.Misc.TerraMusicBox>(),
                ModContent.TileType<Content.Tiles.TerraMusicBox>());

            if (DebugConfig.Nightly)
            {
                var path = Path.Combine(Main.SavePath, "ROI-beta-timestamp.txt");
                if (File.Exists(path))
                {
                    var data = File.ReadAllText(path);
                    Helpers.NightlyHelper.CheckForNightly(DateTime.Parse(data));
                }
                else
                {
                    var data = Helpers.NightlyHelper.CheckForNightly(DateTime.MinValue);
                    if (data != null)
                        File.WriteAllText(path, data);
                }
            }
        }

        private void UnloadClient()
        {
        }


        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            // TODO: (low prio) this was in EM, but it might be redundant
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return;

            // Highest to lowest priority here, just return if a condition is validated

            // TODO: wasteland
            if (Main.LocalPlayer.GetModPlayer<ROIPlayer>().ZoneWasteland &&
                MusicConfig.WastelandMusic != WastelandMusicType.Vanilla)
            {
                music = GetSoundSlot(SoundType.Music, "Assets/Sounds/Music/Wasteland_" + MusicConfig.WastelandMusic);
                priority = MusicPriority.Environment;
            }
        }
    }
}
