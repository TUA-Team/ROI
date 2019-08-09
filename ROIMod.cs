using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;
using ROI.Manager;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using ROI.Backgrounds.Underworld;
using ROI.Effects;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI
{
    public sealed partial class ROIMod : Mod
	{
		internal static string SAVE_PATH = "";

		internal static ROIMod instance;
        public static bool dev;
        internal static bool debug;

	    internal static FilterManager roiFilterManager;

        public ROIMod()
		{

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

		private void ClientLoad()
		{
		    roiFilterManager = new FilterManager();

            VoidPillarHealthBar.Load();
            VoidUI.Load();
            VoidHeartHealthBar.Load();
            UnderworldDarkness.Load();            
            Wasteland_Background.Load();
            DRPManager.Instance.Initialize();
            Main.OnTick += DRPManager.Instance.Update;
		}

		private void GeneralLoad()
		{
			instance = this;
            DevManager.Instance.CheckDev();
            ROIModSupport.Load();
#if DEBUG
            debug = true;
#else
            debug = false;
#endif
        }

	    public override void PostAddRecipes()
	    {
	        ROITreeHookLoader.Load();
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
		}

		private void ClientUnload()
		{
            VoidPillarHealthBar.Unload();
            VoidUI.Unload();
            VoidHeartHealthBar.Unload();
            DRPManager.Instance.Unload();
		    roiFilterManager = null;
            Main.OnTick -= DRPManager.Instance.Update;
        }
#endregion

	    public override void UpdateUI(GameTime gameTime)
	    {
	        roiFilterManager.Update(gameTime);
	    }

	    public override void PostDrawInterface(SpriteBatch spriteBatch)
	    {
	        VoidPillarHealthBar.FindPillar();
            VoidPillarHealthBar.Draw(spriteBatch);
	        VoidHeartHealthBar.Draw(spriteBatch);
	        if (!Main.playerInventory)
	        {
                VoidUI.Draw(spriteBatch);
	        }
	    }

	    

	    public override object Call(params object[] args)
	    {
	        string command = (string) args[0];
	        try
	        {
	            switch (command)
	            {
	                case "DarkMind":
	                    int npcID = (int) args[1]; 
	                    int buffType = (int)args[2];
	                    int buffLength = (int) args[3];
	                    bool quiet = (bool) args[4];
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
	        if (Main.ActiveWorldFileData.HasCrimson && (Main.LocalPlayer.position.Y / 16) > Main.maxTilesY - 200)
	        {
	            scale = 0.7f;
	        }
	        base.ModifyLightingBrightness(ref scale);
	    }

        
	}
}
