using ROI.API;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public static ROIMod Instance => ModContent.GetInstance<ROIMod>();

        public const bool DEBUG = true;

        public override void Load()
        {
            InitializeLoaders();

            // Utils.GenerateLocalization(this);

            if (!Main.dedServ)
            {
                LoadClient();
            }

            SimpleLoadables.Load(this);

            Backporting.Init(this);
        }

        public override void Unload()
        {
            UnloadLoaders();

            if (!Main.dedServ)
            {
                UnloadClient();
            }

            Backporting.Clear();
        }
    }
}