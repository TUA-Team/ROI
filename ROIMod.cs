using System.IO;
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
			GeneralLoad();
			if (!Main.dedServ)
			{
				NonNetworkLoad();
			}
		}

		private void NonNetworkLoad()
		{

		}

		private void GeneralLoad()
		{
			instance = this;
		}

		public override void Unload()
		{
			base.Unload();
		}

		private void GeneralUnload()
		{
			instance = null;
			if (!Main.dedServ)
			{
				NonNetworkUnload();
			}
		}

		private void NonNetworkUnload()
		{

		}
		#endregion

	}
}
