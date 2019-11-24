using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        private int _radiationTimer;
        private int _removeRadiationTimer;

        private void WastelandInit()
        {
            SetRadiationTimer();
            SetRadiationRemovalTimer();
        }

        private void WastelandUpdate()
        {
            if (_radiationTimer <= 0)
            {
                if (ZoneWasteland && radiationLevel != 1)
                {
                    GUI.RadiationMeter.RadiationMeter.visible = true;
                    radiationLevel += 0.01f;
                    SetRadiationTimer();
                }
                else if (!ZoneWasteland && radiationLevel != 0)
                {
                    radiationLevel -= 0.01f;
                    SetRadiationRemovalTimer();
                }
                if ((radiationLevel = Utils.Clamp(radiationLevel, 0, 1)) == 0) GUI.RadiationMeter.RadiationMeter.visible = false;
                if (_radiationTimer < 0) _radiationTimer = 0;
            }

            else if (_radiationTimer >= 1)
            {
                _radiationTimer--;
            }

            if (_removeRadiationTimer >= 1)
            {
                _removeRadiationTimer--;
            }

            ApplyRadiationEffect();
        }

        private void SetRadiationTimer()
        {
            _radiationTimer = 60 * 2;
        }

        private void SetRadiationRemovalTimer()
        {
            _removeRadiationTimer = 150; //50 * 2.5
        }

        private void ApplyRadiationEffect()
        {
            //TODO these effects are annoying as hell
            if (radiationLevel >= 0.2)
            {
                player.statDefense /= 2;
            }

            if (radiationLevel >= 0.4)
            {
                player.allDamage *= 0.75f;
            }

            if (radiationLevel >= 0.7)
            {
                player.statLifeMax2 = (int)(player.statLifeMax2 * 0.9f);
                player.lifeRegen = 0;
                player.allDamage *= 0.6f;
            }

            if (radiationLevel >= 0.85)
            {
                PlayerDeathReason reason = PlayerDeathReason.ByCustomReason($"{player.name} {GetRadiationDeath()}");
                player.Hurt(reason, 1, 0, false, true, false, 20);
            }
        }

        private string GetRadiationDeath()
        {
            string[] deathReason =
            {
                "didn't check their radiation level",
                "tried to become the Hulk but failed miserably",
                "didn't know how to survive to extreme radiation",
                "thought it was a good idea to live in the wasteland",
                "was absorbed by the Heart of the Wasteland",
                "became an irradiated mutant",
                "didn't bother to check their health, so they they died of radiation poisoning"
            };
            return Main.rand.Next(deathReason);
        }
    }
}
