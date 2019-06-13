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

	    internal int VoidAffinityAmount
	    {
            get => _voidAffinityAmount;
	    }


	    public int AddVoidAffinity(int voidAffinity, bool simulate = false)
	    {
	        int simulatedAmount = Math.Min(MaxVoidAffinity - VoidAffinityAmount, voidAffinity);
	        if (!simulate)
	        {
	            _voidAffinityAmount += simulatedAmount;
            }
	        return simulatedAmount;
	    }
    }
}
