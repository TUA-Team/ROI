using System;
using Terraria.ModLoader;

namespace ROI.Players
{
	public sealed partial class ROIPlayer : ModPlayer
	{
        public int MaxVoidAffinity { get; private set; }

	    public int voidEffectAttemptCooldown;

        internal int VoidAffinityAmount => voidAffinityAmount;

        private void InitVoid()
        {
            MaxVoidAffinity = 100;
            // one minute cool down
            voidEffectAttemptCooldown = 60 * 60;
        }

	    public short AddVoidAffinity(int voidAffinity, bool simulate)
	    {
	        short simulatedAmount = 
                (short)Math.Min(MaxVoidAffinity - VoidAffinityAmount, voidAffinity);
	        if (!simulate) voidAffinityAmount += simulatedAmount;
	        return simulatedAmount;
	    }
    }
}
