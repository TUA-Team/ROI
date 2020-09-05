using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Reflection;
using Terraria;
using ROIPlayer = ROI.Players.ROIPlayer;

namespace ROI.UI.Void
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
            int numberOfVoidHeart = player.MaxVoidHeartStats / 10;

            Vector2 drawingOffset = new Vector2(500, 6f);

            int additionalHealth = player.MaxVoidHeartStatsExtra - player.VoidHeartHP;

            LifePerHeart += additionalHealth / numberOfVoidHeart;

            int numberOfHeartToDraw = player.MaxVoidHeartStatsExtra / LifePerHeart;

            if (numberOfHeartToDraw >= 10)
            {
                numberOfHeartToDraw = 10;
            }

            string text = $"Void HP {player.VoidHeartHP}/{player.MaxVoidHeartStatsExtra}";
            Vector2 textSize = Main.fontMouseText.MeasureString(text);

            sb.DrawString(Main.fontMouseText, text, new Vector2(drawingOffset.X + textSize.X, drawingOffset.Y), new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), 0f, new Vector2(Main.fontMouseText.MeasureString(Main.player[Main.myPlayer].statLife + "/" + Main.player[Main.myPlayer].statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);


            for (int i = 1; i < player.MaxVoidHeartStatsExtra / LifePerHeart + 1; i++)
            {
                float heartScale = 1f;
                float heartAlpha;
                bool pulsatingEffect = false;

                if (player.VoidHeartHP >= i * LifePerHeart)
                {
                    heartAlpha = 255;
                    if (player.VoidHeartHP == i * LifePerHeart)
                    {
                        pulsatingEffect = true;
                    }
                }
                else
                {
                    float individualHeartValue = ((float)Main.player[Main.myPlayer].statLife - (float)(i - 1) * LifePerHeart) / LifePerHeart;
                    heartAlpha = (int)(30f + 225f * individualHeartValue);
                    if (heartAlpha < 30)
                    {
                        heartAlpha = 30;
                    }
                    heartScale = individualHeartValue / 4f + 0.75f;
                    if ((double)heartScale < 0.75)
                    {
                        heartScale = 0.75f;
                    }
                    if (individualHeartValue > 0f)
                    {
                        pulsatingEffect = true;
                    }
                }

                if (pulsatingEffect)
                {
                    heartScale += Main.cursorScale - 1f;
                }
                int alpha = (int)((double)((float)heartAlpha) * 0.9);
                Main.spriteBatch.Draw(heartTexture, new Vector2((float)(500 + 26 * (i - 1) + main_UIScreenAnchorX + heartTexture.Width / 2), 32f + ((float)heartTexture.Height - (float)heartTexture.Height * heartScale) / 2f + (float)(heartTexture.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, heartTexture.Width, heartTexture.Height)), new Microsoft.Xna.Framework.Color(heartAlpha, heartAlpha, heartAlpha, alpha), 0f, new Vector2((float)(Main.heartTexture.Width / 2), (float)(Main.heartTexture.Height / 2)), heartScale, SpriteEffects.None, 0f);
            }
        }
    }
}
