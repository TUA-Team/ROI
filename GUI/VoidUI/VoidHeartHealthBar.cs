using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROI.Players;
using Terraria;

namespace ROI.GUI.VoidUI
{
    /// <summary>
    /// I'm lazy, not doing my own UI state because I hate that system
    /// </summary>
    class VoidHeartHealthBar
    {
        private static Texture2D heartTexture;

        public static void Load()
        {
            heartTexture = ROIMod.instance.GetTexture("Textures/UIElements/VoidHeart");
        }

        public static void Unload()
        {
            heartTexture = null;
        }

        public static void Draw(SpriteBatch sb)
        {
            ROIPlayer player = Main.LocalPlayer.GetModPlayer<ROIPlayer>();

            int main_UIScreenAnchorX = (int)typeof(Main)
                .GetField("UI_ScreenAnchorX", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            player.VoidHeartHP = 100;
            player.MaxVoidHeartStats = 100;
            player.MaxVoidHeartStatsExtra = 100;

            int LifePerHeart = 10;
            int numberOfVoidHeart = (int)(player.MaxVoidHeartStats * .1);

            Vector2 drawingOffset = new Vector2(500, 6f);

            int additionalHealth = player.MaxVoidHeartStatsExtra - player.VoidHeartHP;

            LifePerHeart += additionalHealth / numberOfVoidHeart;

            // int heartsToDraw = ROIUtils.LowClamp(player.MaxVoidHeartStatsExtra / LifePerHeart, 10);

            string text = ROIUtils.GetLangValue("VoidHeartHealthBar", player.VoidHeartHP, player.MaxVoidHeartStatsExtra);
            Vector2 textSize = Main.fontMouseText.MeasureString(text);

            sb.DrawString(Main.fontMouseText,
                text, 
                new Vector2(drawingOffset.X + textSize.X, drawingOffset.Y), 
                new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 
                0f, 
                new Vector2(Main.fontMouseText.MeasureString(Main.player[Main.myPlayer].statLife + "/" + Main.player[Main.myPlayer].statLifeMax2).X, 0f),
                1f,  
                SpriteEffects.None, 
                0f);

            for (int i = 1; i < player.MaxVoidHeartStatsExtra / LifePerHeart + 1; i++)
            {
                float heartScale = 1f;
                float heartAlpha;
                bool pulsatingEffect = false;

                if (player.VoidHeartHP >= i * LifePerHeart)
                {
                    heartAlpha = 255;
                    pulsatingEffect = player.VoidHeartHP == i * LifePerHeart;
                }
                else
                {
                    float individualHeartValue = (Main.player[Main.myPlayer].statLife - (float)(i - 1) * LifePerHeart) / LifePerHeart;
                    heartAlpha = ROIUtils.LowClamp((int)(30 + 225 * individualHeartValue), 30);
                    heartScale = ROIUtils.LowClamp(individualHeartValue * .25f + 0.75f, .75f);
                    pulsatingEffect = individualHeartValue > 0f;
                }
                if (pulsatingEffect)
                {
                    heartScale += Main.cursorScale - 1f;
                }
                int alpha = (int)(heartAlpha * 0.9);
                Main.spriteBatch.Draw(heartTexture, 
                    new Vector2(500 + 26 * (i - 1) + main_UIScreenAnchorX + heartTexture.Width * .5f, 
                        // 32f + (heartTexture.Height - heartTexture.Height * heartScale) / 2f + heartTexture.Height / 2
                        32 + heartTexture.Height * .5f), 
                    new Rectangle(0, 0, heartTexture.Width, heartTexture.Height), 
                    new Color(heartAlpha, heartAlpha, heartAlpha, alpha),
                    0f,
                    new Vector2(Main.heartTexture.Width * .5f, Main.heartTexture.Height * .5f), 
                    heartScale, 
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
