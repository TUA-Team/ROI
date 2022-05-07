using ROI.Content.Items;
using ROI.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    public sealed partial class ROIMod
    {
        //public MusicConfig MusicConfig;

        private void LoadClient()
        {
            //MusicConfig = ModContent.GetInstance<MusicConfig>();

            LoadDebug();
        }

        private void UnloadClient()
        {
        }
    }
}
