using InfinityCore.API.Interface;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer, IPlayerExtension
    {
        internal bool irrawoodSet = false;
        internal bool irradiatedSet = false;

        #region Irradiated Specific bonus

        internal bool irradiatedHood = false;
        internal bool irradiatedMask = false;
        internal bool irradiatedHat = false;
        internal bool irradiatedHornedHelmet = false;
        internal bool irradiatedHelmet;

        #endregion

        private void ResetArmorEffect()
        {
            irrawoodSet = false;
            irradiatedSet = false;

            irradiatedHat = false;
            irradiatedHelmet = false;
            irradiatedHood = false;
            irradiatedHornedHelmet = false;
            irradiatedMask = false;

        }
    }
}
