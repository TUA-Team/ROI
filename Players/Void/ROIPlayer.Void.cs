using Microsoft.Xna.Framework;
using System;
using Terraria;
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

<<<<<<< HEAD
        public bool voidHeartBuff = false;

		internal int VoidAffinityAmount
		{
			get => _voidAffinityAmount;
		}
=======
        internal int VoidAffinityAmount
        {
            get => _voidAffinityAmount;
        }
>>>>>>> 93055d08c4298f520ee2b67f37961dd6c4805bd5


        public int AddVoidAffinity(int voidAffinity, bool simulate = false)
        {
            int simulatedAmount = Math.Min(MaxVoidAffinity - VoidAffinityAmount, voidAffinity);
            if (!simulate)
            {
                _voidAffinityAmount += (short)simulatedAmount;
            }
            return simulatedAmount;
        }

        public void DamageVoidHeart(ref int damage)
        {
<<<<<<< HEAD
			if (VoidHeartHP >= 0)
			{
			    CombatText.NewText(
			        new Rectangle((int) player.position.X, (int) player.position.Y, player.width, player.height),
			        Color.Black, damage, true, true);
				VoidHeartHP -= damage;
				damage = 0;
			}

            if (VoidHeartHP <= 0)
            {
                voidHeartBuff = false;
				
            }
		}
	}
=======
            if (VoidHeartHP >= 0)
            {
                CombatText.NewText(
                    new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                    Color.Black, damage, true, true);
                VoidHeartHP -= damage;
                damage = 0;
            }
        }
    }
>>>>>>> 93055d08c4298f520ee2b67f37961dd6c4805bd5
}
