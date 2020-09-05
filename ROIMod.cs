using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Backgrounds.Underworld;
using ROI.Effects;
using ROI.Effects.CustomSky;
using ROI.GUI;
using ROI.GUI.VoidUI;
using ROI.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfinityCore.API.Interface;
using InfinityCore.API.Pots;
using InfinityCore.API.Pots.DropTable;
using InfinityCore.Enums;
using Microsoft.Xna.Framework.Media;
using ROI.Configs;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;
using ReLogic.Graphics;
using ROI.NPCs.Void;
using Terraria.ID;
using Terraria.Localization;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace ROI
{
    public sealed partial class ROIMod : Mod, IModExtension
    {
        internal static string SAVE_PATH = "";

        internal static ROIMod instance;
#if DEBUG
        public const bool dev = true;
#else
        public const bool dev = false;
#endif
        internal static bool debug = dev;

        internal static FilterManager roiFilterManager;

        internal UnifiedRandom rng;

        internal UserInterface radiationInterface;
        internal RadiationMeter radiationMeter;

        //Config
        internal static MainMenuConfig menu;

        //Infinity core static loader
        public static bool EnableInfinityCoreStaticLoader = true;

        public static GameTime gameTime;
        private static VideoPlayer player;
        private static Video video;
        private static Texture2D videoTexture;

        public override uint ExtraPlayerBuffSlots => 255 - 22;

        public ROIMod()
        {
            rng = new UnifiedRandom();
        }

        #region Load and unload stuff

        public override void Load()
        {
            GeneralLoad();
            if (!Main.dedServ)
            {
                ClientLoad();
            }
            
        }

        /*
        public override bool LoadResource(string path, int length, Func<Stream> getStream)
        {
            string extension = Path.GetExtension(path).ToLower();
            path = Path.ChangeExtension(path, null);
            switch (extension) {
                case ".mp4" :
                    if (!Main.dedServ)
                    {
                        return base.LoadResource(path, length, getStream);
                    }
                    video = Main.instance.OurLoad<Video>("tmod:" + Name + "/" + path);
                    return true;
            }

            return base.LoadResource(path, length, getStream);
        }*/


        private void ClientLoad()
        {
            roiFilterManager = new FilterManager();

            VoidPillarHealthBar.Load();
            VoidAffinity.Load();
            VoidHeartHealthBar.Load();
            UnderworldDarkness.Load();
            Wasteland_Background.Load();
            DRPManager.Instance.Initialize();
            radiationInterface = new UserInterface();
            radiationMeter = new RadiationMeter();
            radiationInterface.SetState(radiationMeter);

            Filters.Scene["ROI:UnderworldFilter"] = new Filter(new ScreenShaderData(new Ref<Effect>(ROIMod.instance.GetEffect("Effects/UnderworldFilter")), "UnderworldFilter"), EffectPriority.VeryHigh);
            Filters.Scene["ROI:UnderworldFilter"].Load();

            Filters.Scene["ROI:ShockwaveColor"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/ShockwaveColorEffect")), "ShockwaveColoured"), EffectPriority.VeryHigh);
            Filters.Scene["ROI:ShockwaveColor"].Load();

            for (int i = 0; i < 10; i++)
            {
                Filters.Scene[$"ROI:Shockwave{i}"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/Shockwave")), "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene[$"ROI:Shockwave{i}"].Load();
            }

            SkyManager.Instance["ROI:VoidSky"] = new VoidSky();

            Main.OnTick += DRPManager.Instance.Update;

            TextureCache.Initialize();

            InfinityCore.InfinityCore.screenOverlays.Add(new DungeonOverlay());

            Main.OnTick += delegate
            {
                if (!Main.gameMenu)
                {
                    return;
                }

                //Main.worldSurface = 565;
                string currentBackgroundSetting = menu.NewMainMenuTheme;
                switch (currentBackgroundSetting)
                {
                    case "Vanilla" :
                        return;
                    default:
                        if (Filters.Scene[menu.NewMainMenuTheme] != null)
                        {
                            Filters.Scene.Activate(menu.NewMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        }

                        if (SkyManager.Instance[menu.NewMainMenuTheme] != null)
                        {
                            SkyManager.Instance.Activate(menu.NewMainMenuTheme, new Vector2(2556.793f, 4500f), new object[0]);
                        }

                        if (Overlays.Scene[menu.NewMainMenuTheme] != null)
                        {
                            Overlays.Scene.Activate(menu.NewMainMenuTheme,
                                Vector2.Zero - new Vector2(0f, 10f), new object[0]);
                        }
                        break;
                }
            };
            if (InfinityCore.InfinityCore.IsTMLFNA64())
            {
                player = new VideoPlayer();
                video = InfinityCore.InfinityCore.LoadVideo(this, "ROI/Backgrounds/MainMenu/Menu_BG");

                On.Terraria.Main.DrawMenu += (orig, self, time) =>
                {
                   
                    if (player.State == MediaState.Stopped)
                    {
                        player.Play(video);
                    }
                    videoTexture = player.GetTexture();
                    Vector2 scalingTexture = new Vector2(Main.graphics.GraphicsDevice.Viewport.Width / (float)video.Width, Main.graphics.GraphicsDevice.Viewport.Height / (float)video.Height);
                    Main.spriteBatch.Draw(videoTexture, Vector2.Zero, new Rectangle(0, 0, videoTexture.Width, videoTexture.Height), Color.White, 0f, Vector2.Zero, scalingTexture, SpriteEffects.None, 1f);
                    orig(self, time);
                };
            }
            
        }

        private void GeneralLoad()
        {

            instance = this;
            DevManager.Instance.CheckDev();
            ROIModSupport.Load();
            //Patch.Load();
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new ROISerializer.VersionSerializer());
#if DEBUG
            debug = true;
#else
            debug = false;
#endif
        }

        public override void PostAddRecipes()
        {
            List<PotsDrop> drop = PotsRegistry.ModifyPotsDrop((int) PotsTypeID.Lihzahrd);
            //drop.Insert(0, new ItemPotsDrop("brick", ItemID.LihzahrdBrick, 10, (i, i1) => Main.rand.NextBool()));
            drop = PotsRegistry.ModifyPotsDrop((int) PotsTypeID.Hell);
            //drop.Insert(0, new ItemPotsDrop("Obsidian", ItemID.Obsidian, 7, (i, i1) => Main.rand.Next(25) == 0));
            //drop.Insert(1, new ItemPotsDrop("Hellstone", ItemID.Hellstone, 3, (i, i1) => Main.rand.Next(35) == 0));

            PotsRegistry.RegisterModPotsStyle(TileType("Wasteland_Rock"), new ModPots[] {(ModPots)GetTile("Wasteland_Pots_Big"), (ModPots)GetTile("Wasteland_Pots_Small")});
            PotsRegistry.RegisterModPotsStyle(TileType("Wasteland_Dirt"), new ModPots[] {(ModPots)GetTile("Wasteland_Pots_Big"), (ModPots)GetTile("Wasteland_Pots_Small")});
            PotsRegistry.RegisterModPotsStyle(TileType("Wasteland_Grass"), new ModPots[] {(ModPots)GetTile("Wasteland_Pots_Big"), (ModPots)GetTile("Wasteland_Pots_Small")});
        }

        public override void Unload()
        {
            GeneralUnload();
            if (!Main.dedServ)
            {
                ClientUnload();
            }
        }

        private void GeneralUnload()
        {
            instance = null;
            rng = null;
            //Patch.Unload();
        }

        private void ClientUnload()
        {
            VoidPillarHealthBar.Unload();
            VoidAffinity.Unload();
            VoidHeartHealthBar.Unload();
            DRPManager.Instance.Unload();
            roiFilterManager = null;
            Main.OnTick -= DRPManager.Instance.Update;
            TextureCache.Unload();
        }
        #endregion

        public override void UpdateUI(GameTime gameTime)
        {
            ROIMod.gameTime = gameTime;
            if (radiationInterface.IsVisible)
            {
                radiationInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != 1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ROI : Radiation Meter",
                    delegate
                    {
                        if (radiationInterface.IsVisible)
                        {
                            radiationInterface.CurrentState.Draw(Main.spriteBatch);
                        }
                        return true;
                    }));
            }
        }

		public override void PostDrawInterface(SpriteBatch spriteBatch)
	    {
	        //VoidPillarHealthBar.FindPillar();
            //VoidPillarHealthBar.Draw(spriteBatch);
	        //VoidHeartHealthBar.Draw(spriteBatch);
	        if (!Main.playerInventory)
	        {
                //VoidUI.Draw(spriteBatch);
	        }
        }

        

        


        public override object Call(params object[] args)
        {
            string command = (string)args[0];
            try
            {
                switch (command)
                {
                    case "DarkMind":
                        int npcID = (int)args[1];
                        int buffType = (int)args[2];
                        int buffLength = (int)args[3];
                        bool quiet = (bool)args[4];
                        Main.npc[npcID].AddBuff(buffType, buffLength, true, quiet);
                        break;
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkManager.Instance.ReceivePacket(reader, whoAmI);
        }

        public override void ModifyLightingBrightness(ref float scale)
        {
            scale = 1.09f;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            backgroundColor = new Color(75,0,130);
            tileColor = new Color(139,0,139);
            base.ModifySunLightColor(ref tileColor, ref backgroundColor);
        }

        public bool ModifyCameraBound()
        {
            return false;
        }
    }
}
