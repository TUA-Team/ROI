using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Configs;
using ROI.Effects;
using ROI.GUI;
using ROI.Players;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI
{
    public class ROIMod : Mod
    {
        public static MusicConfig MusicConfig;
        public static DebugConfig DebugConfig;

        public UserInterface radInterface;
        internal RadiationMeter radState;
        public UserInterface buffListInterface;
        internal VoidBuffList buffListState;

        public override void Load()
        {
            if (!Main.dedServ) ClientLoad();
        }

        public override void Unload()
        {
            if (!Main.dedServ) ClientUnload();
        }

        private void ClientLoad()
        {
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Terra"),
                ModContent.ItemType<Items.Misc.TerraMusicBox>(),
                ModContent.TileType<Tiles.TerraMusicBox>());

            UnderworldDarkness.Load();
            loadFilter("UnderworldFilter");

            radState = new RadiationMeter();
            radState.Activate();
            radInterface = new UserInterface();
            radInterface.SetState(radState);

            buffListState = new VoidBuffList();
            buffListState.Activate();
            buffListInterface = new UserInterface();
            buffListInterface.SetState(buffListState);

            VoidAffinity.Load();

            MusicConfig = ModContent.GetInstance<MusicConfig>();
            DebugConfig = ModContent.GetInstance<DebugConfig>();

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
                    if (data != null) File.WriteAllText(path, data);
                }
            }

            void loadFilter(string name, EffectPriority priority = EffectPriority.VeryHigh)
            {
                Filters.Scene[$"ROI:{name}"] = new Filter(new ScreenShaderData(
                    new Ref<Effect>(GetEffect($"Effects/{name}")), name), priority);
                Filters.Scene[$"ROI:{name}"].Load();
            }
        }

        private void ClientUnload()
        {
            VoidAffinity.Unload();

            MusicConfig = null;
            DebugConfig = null;
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active) return;

            // Make sure your logic here goes from lowest priority to highest so your intended priority is maintained.
            if (Main.LocalPlayer.GetModPlayer<ROIPlayer>().ZoneWasteland &&
                MusicConfig.WastelandMusic != WastelandMusicType.Vanilla)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Wasteland_" + MusicConfig.WastelandMusic);
                priority = MusicPriority.Environment;
            }
        }

        private GameTime _lastGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastGameTime = gameTime;
            if (RadiationMeter.visible)
                radInterface.Update(_lastGameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "ROI: Radiation Meter",
                    delegate
                    {
                        if (RadiationMeter.visible)
                        {
                            radInterface.Draw(Main.spriteBatch, _lastGameTime);
                        }
                        return true;
                    }, InterfaceScaleType.UI));
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "ROI: Void Affinity",
                    delegate
                    {
                        VoidAffinity.Draw(Main.spriteBatch);
                        return true;
                    }, InterfaceScaleType.UI));
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "ROI: Void Buff List",
                    delegate
                    {
                        buffListInterface.Draw(Main.spriteBatch, _lastGameTime);
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }

        public override void PostAddRecipes()
        {
            TreeHooking.Load();
        }

        public override void ModifyLightingBrightness(ref float scale)
        {
            if (Main.LocalPlayer.GetModPlayer<ROIPlayer>().ZoneWasteland) scale = 0.7f;
        }
    }
}