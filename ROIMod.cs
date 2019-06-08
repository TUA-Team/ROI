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

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{

		}

		#region Load and unload stuff

		public override void Load()
		{
            #region General load

            instance = this;

            #endregion

            if (!Main.dedServ)
			{
                #region Client load

                VoidPillarHealthBar.Load();

                #endregion
            }
		}

		public override void Unload()
		{
            #region General unload

            instance = null;

            #endregion

            if (!Main.dedServ)
            {
                #region Client unload

                VoidPillarHealthBar.Unload();

                #endregion
            }
        }

		#endregion

	    public override void PostDrawInterface(SpriteBatch spriteBatch)
	    {
	        VoidPillarHealthBar.FindPillar();
            VoidPillarHealthBar.Draw(spriteBatch);
	    }
	}
}
