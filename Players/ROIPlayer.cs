using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public static ROIPlayer Get(Player player) => player.GetModPlayer<ROIPlayer>();


        private void UpdatePreviousBuffs()
        {
            PreviousBuffs.Clear();

            for (int i = 0; i < player.CountBuffs(); i++)
                PreviousBuffs.Add(player.buffType[i]);
        }

        private void OnNewBuffDetected(int previousBuffType)
        {
            for (int i = 0; i < player.buffType.Length; i++)
                if (player.buffType[i] == previousBuffType && Main.debuff[player.buffType[i]]) // TODO Check if you can just do player.buffType[previousBuffType] instead of iterating through this.
                    player.buffTime[i] = (int)(player.buffTime[previousBuffType] * DebuffDurationMultiplier);
        }


        public override void Initialize()
        {
            base.Initialize();

            InitializeVoid();
        }

        public override void OnEnterWorld(Player player)
        {
            List<int> previousBuffs = new List<int>(PreviousBuffs.ToArray());
            UpdatePreviousBuffs();

            for (int i = 0; i < PreviousBuffs.Count; i++)
                if (!previousBuffs.Contains(PreviousBuffs[i]))
                    OnNewBuffDetected(PreviousBuffs[i]);
        }

        public override void PreUpdateBuffs()
        {
            base.PreUpdateBuffs();
        }

        public override void ResetEffects()
        {
            base.ResetEffects();

            ResetEffectsVoid();
        }


        private List<int> PreviousBuffs { get; set; } = new List<int>();
    }
}