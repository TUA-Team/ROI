using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public override void Load() {
            base.Load();

            InitializeLoaders();

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
    }
}