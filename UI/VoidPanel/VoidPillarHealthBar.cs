using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace ROI.UI.VoidPanel
{
    static class VoidPillarHealthBar
    {
        public static VoidPillar Pillar { get; private set; }

        private static Texture2D BOSSHEALTH_BACKGROUND;
        private static Texture2D BOSSHEALTH_FOREGROUND;

        public static void Load()
        {
            BOSSHEALTH_BACKGROUND =
                ROIMod.instance.GetTexture("Textures/UIElements/HealthBarBG");
            BOSSHEALTH_FOREGROUND =
                ROIMod.instance.GetTexture("Textures/UIElements/HealthBarFG");
        }

        public static void Unload()
        {
            BOSSHEALTH_BACKGROUND?.Dispose();
            BOSSHEALTH_FOREGROUND?.Dispose();
        }

        public static VoidPillar FindPillar()
        {
            for (int i = 0; i < 200; i++)
            {
                var npc = Main.npc[i];
                if (npc.active && npc.modNPC is VoidPillar local
                    && npc.DistanceSQ(Main.LocalPlayer.position) <= 6250000)
                {
                    return Pillar = local;
                }
            }
            return Pillar = null;
        }

        public static void Draw(SpriteBatch sb)
        {
            if (Pillar == null)
            {
                return;
            }

            string name = "Void Pillar - " + Pillar.ShieldColor.ToString() + " Shield";
            string health = Pillar.npc.life + "/" + Pillar.npc.lifeMax;
            var color = Pillar.GetShieldColor();

            Vector2 offset = new Vector2(Main.screenWidth / 2, Main.screenHeight - 100);
            if (Terraria.ModLoader.ModLoader.GetMod("HEROsMod") != null
                || Terraria.ModLoader.ModLoader.GetMod("CheatSheet") != null)
            {
                offset.Y -= 50;
            }
            Vector2 textSize = Main.fontDeathText.MeasureString(name) * 0.5f;
            Vector2 healthTextSize = Main.fontDeathText.MeasureString(health) * 0.5f;

            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250, (int)offset.Y + 41, 20, 41), new Rectangle(0, 0, 20, 41), color * 0.5f);
            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250 + 20, (int)offset.Y + 41, 460, 41), new Rectangle(23, 0, 24, 41), color * 0.5f);
            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250 + 480, (int)offset.Y + 41, 20, 41), new Rectangle(51, 0, 20, 41), color * 0.5f);

            float barProgress = Pillar.ShieldHealth / Pillar.npc.lifeMax;
            int width = (int)(500 * barProgress);
            Rectangle barArea = new Rectangle((int)offset.X - 250 + 2, (int)offset.Y + 41, width, 41);

            sb.Draw(BOSSHEALTH_FOREGROUND, barArea, new Rectangle(23, 0, 24, 41), color);

            Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, name, (int)offset.X - textSize.X / 2, offset.Y, Color.Purple, Color.MediumPurple, Vector2.Zero, 0.5f);
            Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, health, (int)offset.X - healthTextSize.X / 2, offset.Y + healthTextSize.Y + 10, Color.LightGray, Color.DimGray, Vector2.Zero, 0.5f);
        }
    }
}

