using Microsoft.Xna.Framework;
using ROI.Content.Configs;
using ROI.Content.Items;
using ROI.Content.Subworlds.Wasteland;
using ROI.Players;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI
{
    public sealed partial class ROIMod
    {
        //public MusicConfig MusicConfig;
        public DebugConfig DebugConfig;

        private void LoadClient()
        {
            //MusicConfig = ModContent.GetInstance<MusicConfig>();
            DebugConfig = ModContent.GetInstance<DebugConfig>();

            AddMusicBox(GetSoundSlot(SoundType.Music, "Assets/Sounds/Music/Terra"),
                ModContent.ItemType<TerraMusicBox>(),
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

            if (Subworld.IsActive<WastelandDepthSubworld>())
            {
                music = GetSoundSlot(SoundType.Music, "Assets/Sounds/Music/WastelandDepth");
                priority = MusicPriority.Environment;
                return;
            }
        }

        public override void ModifyLightingBrightness(ref float scale)
        {
            if (ROIPlayer.Get().ZoneWasteland)
                scale *= 0.7f;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            interfaceLoader.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            interfaceLoader.ModifyInterfaceLayers(layers);
        }
    }
}
