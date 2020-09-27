using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public static ROIMod Instance { get; private set; }

        public override void Load()
        {
            Instance = this;

            InitializeLoaders();

            // Utils.GenerateLocalization(this);

            if (!Main.dedServ)
            {
                LoadClient();
            }

            Backporting.Init(this);
        }

        public override void Unload()
        {
            Instance = null;

            UnloadLoaders();

            if (!Main.dedServ)
            {
                UnloadClient();
            }

            Backporting.Clear();
        }
    }
}