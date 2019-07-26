using Terraria;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public static ROIPlayer Get(Player player) => player.GetModPlayer<ROIPlayer>();


        public override void Initialize()
        {
            base.Initialize();

            InitializeVoid();
        }
    }
}