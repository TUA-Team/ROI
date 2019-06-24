using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;
using ROI.Manager;
using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using ROI.GUI.VoidUI;
using ROI.Manager;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod : Mod
	{
        internal static string SAVE_PATH = "";
		internal static ROIMod instance;
        public static bool dev;
        internal static bool debug;

        #region Load and unload stuff

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
            VoidPillarHealthBar.Load();
            VoidUI.Load();
            VoidHeartHealthBar.Load();
            
            Main.OnTick += DRPManager.Instance.Update;
		}

		private void GeneralLoad()
		{
			instance = this;
            DevManager.Instance.CheckDev();
#if DEBUG
            debug = true;
#else
            debug = false;
#endif
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
            Main.OnTick -= DRPManager.Instance.Update;
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

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            NetworkManager.Instance.ReceivePacket(reader, whoAmI);
        }
    }
}
