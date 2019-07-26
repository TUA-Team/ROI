using ROI.Void;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public void UnlockVoidTier(VoidTiers tier)
        {
            if (VoidTier != tier - 1) // Player cannot skip tiers.
                return;

            VoidTier = tier;
            MaxVoidAffinity = 
        }
        

        private void InitializeVoid()
        {
            VoidAffinity = 0;
            MaxVoidAffinity = 100;

            VoidExposure = 0;

            VoidEffectAttemptCooldown = 60 * Constants.TICKS_PER_SECOND;
            VoidItemCooldown = 300 * Constants.TICKS_PER_SECOND;
        }

        private void ResetEffectsVoid()
        {
            DebuffDurationMultiplier = 1f;

            MaxVoidHearts2 = MaxVoidHearts;
        }

        
        // TODO Go through these and verify is protection level is accurate.
        public float DebuffDurationMultiplier { get; set; }

        public ushort VoidAffinity { get; private set; }
        public ushort MaxVoidAffinity { get; set; }

        public VoidTiers VoidTier { get; internal set; }

        public uint VoidExposure { get; private set; }

        public int VoidEffectAttemptCooldown { get; internal set; }
        public int VoidItemCooldown { get; internal set; }

        public int VoidHeartHP { get; set; }

        /// <summary>Just like player.statLifeMax, you shouldn't change this property with potions and buffs. Use <see cref="MaxVoidHearts2"/>.</summary>
        public int MaxVoidHearts { get; set; }

        /// <summary>Just like player.statLifeMax2. Resets on <see cref="ResetEffects"/>.</summary>
        public int MaxVoidHearts2 { get; set; }
    }
}
