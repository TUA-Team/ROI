using ROI.Core.Networking;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public static ROIMod Instance => ModContent.GetInstance<ROIMod>();

        public override void Load()
        {
            Backporting.Init(this);

            if (!Main.dedServ)
            {
                LoadClient();
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                UnloadClient();
            }

            Backporting.Clear();
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) =>
            PacketManager.Instance.Handle(reader, whoAmI);
    }
}