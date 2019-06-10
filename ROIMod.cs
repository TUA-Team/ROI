using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
	internal sealed partial class ROIMod : Mod
	{
		internal static string SAVE_PATH = "";

		internal static ROIMod instance;

		public ROIMod()
		{

		}


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
		}

		private void GeneralLoad()
		{
			instance = this;
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

		}

		#endregion

	    public override void PostDrawInterface(SpriteBatch spriteBatch)
	    {
	        VoidPillarHealthBar.FindPillar();
            VoidPillarHealthBar.Draw(spriteBatch);
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
	}
}
