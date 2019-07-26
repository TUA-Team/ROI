using System.IO;
using log4net;
using ROI.Networking;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
	public sealed partial class ROIMod : Mod
	{
		public ROIMod()
        {
            Instance = this;
        }


        public override void Load()
        {
            base.Load();

            if (!Main.dedServ)
            {
                LoadClient();
            }


        }

        public override void Unload()
        {
            base.Unload();

            if (!Main.dedServ)
            {
                UnloadClient();
            }
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) => NetworkPacketManager.Instance.HandlePacket(reader, whoAmI);


        public static ROIMod Instance { get; private set; }

        public static ILog Log => Instance.Logger;

        public static bool Debug { get; }
    }
}