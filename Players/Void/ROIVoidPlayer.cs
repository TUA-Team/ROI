using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        //By default is 100, which correspond to Alpha Tier capacity
        private int _maxVoidAffinity = 100;

        public int MaxVoidAffinity => _maxVoidAffinity;

        internal int VoidAffinityAmount => _voidAffinityAmount;


    }
}
