using System;
using Terraria.ModLoader;

namespace ROI.Players
{
	public sealed partial class ROIPlayer : ModPlayer
	{
		//By default is 100, which correspond to Alpha Tier capacity
		private int _maxVoidAffinity = 100;

		public int MaxVoidAffinity => _maxVoidAffinity;

        /// <summary>
        /// 1 minute cooldown
        /// </summary>
	    public int voidEffectAttemptCooldown = 60 * 60; 

	    internal int VoidAffinityAmount
	    {
            get => _voidAffinityAmount;
	    }


	    public int AddVoidAffinity(int voidAffinity, bool simulate)
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
