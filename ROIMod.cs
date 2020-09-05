using log4net;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public ROIMod() {
            Instance = this;
        }


        public override void Load() {
            base.Load();

            InitializeHelpers();

            if (!Main.dedServ) {
                LoadClient();
            }
        }

        public override void Unload() {
            base.Unload();

            UnloadHelpers();

            if (!Main.dedServ) {
                UnloadClient();
            }
        }


        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            networkLoader.HandlePacket(reader);


        public static ROIMod Instance { get; private set; }

        public static ILog Log => Instance.Logger;

        public static bool Debug { get; }
    }
}