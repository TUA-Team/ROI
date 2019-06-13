using System;
using Terraria.ModLoader;

namespace ROI.Players
{
	public sealed partial class ROIPlayer : ModPlayer
	{
        /// <summary>
        /// 1 minute cooldown
        /// </summary>
	    public int voidEffectAttemptCooldown = 60 * 60;

	    public int voidItemCooldown = 60 * 60 * 5;

        internal int VoidAffinityAmount => voidAffinityAmount;

        private void InitVoid()
        {
            MaxVoidAffinity = 100;
            // one minute cool down
            voidEffectAttemptCooldown = 60 * 60;
        }

	    public int AddVoidAffinity(int voidAffinity, bool simulate = false)
	    {
	        short simulatedAmount = 
                (short)Math.Min(MaxVoidAffinity - VoidAffinityAmount, voidAffinity);
	        if (!simulate) voidAffinityAmount += simulatedAmount;
	        return simulatedAmount;
	    }
    }
}
