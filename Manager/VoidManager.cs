using ROI.Players;
using Terraria;

namespace ROI.Manager
{
	internal sealed class VoidManager : BaseInstanceManager<VoidManager>
	{

		public override void Initialize()
		{

		}

		public float Percent(ROIPlayer player)
		{
			return player.VoidAffinityAmount * 100f / player.MaxVoidAffinity;
		}

		/// <summary>
		/// Unlock the player tier locally, then will send to the server, it's a global unlock anyway for everyone in the server
		/// </summary>
		public void UnlockTier(ROIPlayer player, int tier)
		{
			if (player.VoidTier == tier - 1)
				player.VoidTier = tier;
		}

		#region Void Effect
		public void Effect(ROIPlayer target)
		{
		    if (target.VoidTier >= 6) //normally impossible but hey, still prevempting potential error
		    {
		        target.VoidTier = 6;
		    }

		    if(Main.rand.Next(100) == 0 || VoidPillarRequirement(target))
		    {
		        if (VoidPillarRequirement(target))
		        {
		            target.AddVoidAffinity(target.MaxVoidAffinity, false);
		        }
		    }
		}

	    private static bool VoidPillarRequirement(ROIPlayer target)
	    {
	        return target.VoidAffinityAmount == target.MaxVoidAffinity && target.VoidTier == 6;
	    }

	    private bool Tier1Effect(ROIPlayer target)
		{
			return false;
		}

		private bool Tier2Effect(ROIPlayer target)
		{
			return false;
		}

		private bool Tier3Effect(ROIPlayer target)
		{
			return false;
		}

		private bool Tier4Effect(ROIPlayer target)
		{
			return false;
		}

		/// <summary>
		/// Possible effect can range 
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private bool Tier5Effect(ROIPlayer target)
		{
			return false;
		}

		/// <summary>
		/// Void pillar boss fight, will drain all your void affinity if it spawn
		/// </summary>
		/// <param name="target"></param>
		private void Tier6Effect(ROIPlayer target)
		{

		}
		#endregion

	}
}
