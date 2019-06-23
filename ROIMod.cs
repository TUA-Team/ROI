using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI
{
    internal sealed partial class ROIMod : Mod
	{
        internal static string SAVE_PATH = "";
		internal static ROIMod instance;
        internal static bool dev;

        private static readonly string[] devIDs = new string[]
        {
            "76561198062217769", // Dradonhunter11
            "76561197970658570", // 2grufs
            "76561193945835208", // DarkPuppey
            "76561193830996047", // Gator
            "76561198098585379", // Chinzilla00
            "76561198265178242", // Demi
            "76561193989806658", // SDF
            "76561198193865502", // Agrair
            "76561198108364775", // HumanGamer
            "76561198046878487", // Webmilio
            "76561198008064465", // Rartrin
            "76561198843721841", // Skeletony
        };

        #region Load and unload stuff

        public override void Load()
		{
			GeneralLoad();
			if (!Main.dedServ)
			{
				NonNetworkLoad();
			}
		}

		private void NonNetworkLoad()
		{
            VoidPillarHealthBar.Load();
            VoidUI.Load();
            VoidHeartHealthBar.Load();
		}

		private void GeneralLoad()
		{
			instance = this;
        }

            string curSteam = (string)(typeof(ModLoader).GetProperty("SteamID64", 
                BindingFlags.Static | BindingFlags.NonPublic).GetAccessors(true)[0]
                .Invoke(null, new object[] { }));
            for (int i = 0; i < 12; i++)
            {
                if (devIDs[i] == curSteam) dev = true;
            }
        }

		public override void Unload()
		{
			GeneralUnload();
			if (!Main.dedServ)
			{
				NonNetworkUnload();
			}
		}

		private void GeneralUnload()
		{
			instance = null;
		}

		private void NonNetworkUnload()
		{
            VoidPillarHealthBar.Unload();
            VoidUI.Unload();
            VoidHeartHealthBar.Unload();
		}

		#endregion

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
                Logger.Error(e);
	        }
	        return null;
	    }
    }
}
