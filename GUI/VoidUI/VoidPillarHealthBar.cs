using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Enums;
using ROI.NPCs.Void.VoidPillar;
using Terraria;

namespace ROI.GUI.VoidUI
{
    static class VoidPillarHealthBar
    {
        private static VoidPillar pillar;
        
        private static readonly Texture2D BOSSHEALTH_BACKGROUND =
            ROIMod.instance.GetTexture("Textures/UIElements/HealthBarBG");
        private static readonly Texture2D BOSSHEALTH_FOREGROUND =
            ROIMod.instance.GetTexture("Textures/UIElements/HealthBarFG");


        public static void Update()
        {
            if (!Main.npc.Any(i => i.modNPC is VoidPillar))
            {
                pillar = null; //Make sure it's null if there is no void pillar around
                return;
            }

            pillar = Main.npc.First(i => i.modNPC is VoidPillar).modNPC as VoidPillar;
        }

        public static void Draw(SpriteBatch sb)
        {
            if (pillar == null)
            {
                return;
            }

            PillarShieldColor voidPillarShieldColor = pillar.ShieldColor;

            switch (voidPillarShieldColor)
            {
                case PillarShieldColor.Red:
                    DrawHealthBar(sb, Color.Red, "Red");
                    break;
                case PillarShieldColor.Purple:
                    DrawHealthBar(sb, Color.Purple, "Purple");
                    break;
                case PillarShieldColor.Black:
                    DrawHealthBar(sb, Color.Black, "Black");
                    break;
                case PillarShieldColor.Green:
                    DrawHealthBar(sb, Color.Green, "Green");
                    break;
                case PillarShieldColor.Blue:
                    DrawHealthBar(sb, Color.Blue, "Blue");
                    break;
            }
        }

        private static void DrawHealthBar(SpriteBatch sb, Color shieldColor, String colorName)
        {
            Vector2 offset = new Vector2(Main.screenWidth / 2, Main.screenHeight - 100);
            Vector2 textSize = Main.fontDeathText.MeasureString($"Void pillar - {colorName} shield") * 0.5f;
            Vector2 healthTextSize = Main.fontDeathText.MeasureString($"{pillar.ShieldHealth}/20000") * 0.5f;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
           
            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250, (int)offset.Y + 41, 20, 41), new Rectangle(0, 0, 20, 41), shieldColor * 0.5f);
            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250 + 20, (int)offset.Y + 41, 460, 41), new Rectangle(23, 0, 24, 41), shieldColor * 0.5f);
            sb.Draw(BOSSHEALTH_BACKGROUND, new Rectangle((int)offset.X - 250 + 480, (int)offset.Y + 41, 20, 41), new Rectangle(51, 0, 20, 41), shieldColor * 0.5f);

            float barProgress = pillar.ShieldHealth / 20000f;
            int width = (int)(500 * barProgress);
            Rectangle barArea = new Rectangle((int)offset.X - 250 + 2, (int)offset.Y + 41, width, 41);

            sb.Draw(BOSSHEALTH_FOREGROUND, barArea, new Rectangle(23, 0, 24, 41), shieldColor);

            

            Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, $"Void pillar - {colorName} shield", (int)offset.X - textSize.X / 2, offset.Y, Color.Purple, Color.MediumPurple, Vector2.Zero, 0.5f);
            Utils.DrawBorderStringFourWay(sb, Main.fontDeathText, $"{pillar.ShieldHealth}/20000", (int)offset.X - healthTextSize.X / 2, offset.Y + healthTextSize.Y + 10, Color.LightGray, Color.DimGray, Vector2.Zero, 0.5f);
            sb.End();
            sb.Begin();
        }
    }
}
