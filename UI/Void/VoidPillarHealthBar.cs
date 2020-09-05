using Microsoft.Xna.Framework;
using ROI.NPCs.Bosses.VoidPillar;
using ROI.UI.Elements;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.UI.Void
{
    public class VoidPillarHealthBar : UIState
    {
        private Mod _mod;

        public VoidPillarHealthBar(Mod mod) => _mod = mod;

        public override void OnInitialize() {
            var bar = new HealthBar(_mod);
            bar.GetDrawInfo += GetDrawInfo;
        }

        public VoidPillar FindPillar() {
            for (int i = 0; i < 200; i++) {
                var npc = Main.npc[i];
                if (npc.active && npc.modNPC is VoidPillar query
                    && npc.DistanceSQ(Main.LocalPlayer.position) <= 6250000) {
                    return query;
                }
            }

            return null;
        }

        private bool GetDrawInfo(out string name, out string health, out Color color, out float progress) {
            // TODO: (low prio) modtranslations
            name = null;
            health = null;
            color = default;
            progress = 0;

            var pillar = FindPillar();
            if (pillar == null)
                return false;

            name = "Void Pillar - " + pillar.ShieldColor.ToString() + " Shield";
            health = pillar.npc.life + "/" + pillar.npc.lifeMax;
            color = pillar.GetShieldColor();
            progress = pillar.ShieldHealth / pillar.npc.lifeMax;

            return true;
        }
    }
}

