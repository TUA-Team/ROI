using ROI.Content.Items;
using ROI.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod
    {
        //public MusicConfig MusicConfig;

        private void LoadClient()
        {
            //MusicConfig = ModContent.GetInstance<MusicConfig>();

            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Terra"),
                ModContent.ItemType<TerraMusicBox>(),
                ModContent.TileType<Content.Tiles.TerraMusicBox>());

            LoadDebug();
        }

        private void UnloadClient()
        {
        }


        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            // TODO: (low prio) this was in EM, but it might be redundant
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return;

            var plr = ROIPlayer.Get();

            // Highest to lowest priority here, just return if a condition is validated

            if (plr.ZoneWasteland)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/WastelandDepth");
                priority = MusicPriority.Environment;
                return;
            }
        }
    }
}
