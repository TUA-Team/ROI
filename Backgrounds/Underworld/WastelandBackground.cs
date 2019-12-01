using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Backgrounds.Underworld
{
    internal class WastelandBackground
    {
        private readonly Texture2D[] texture = new Texture2D[5];

        public WastelandBackground()
        {
            for (int i = 0; i < texture.Length; i++)
                texture[i] = ModContent.GetTexture("ROI/Backgrounds/Underworld/Wasteland_" + i);
            On.Terraria.Main.DrawUnderworldBackground += DrawUnderworldBackground;
        }

        private void DrawUnderworldBackground(On.Terraria.Main.orig_DrawUnderworldBackground orig, Main instance, bool flat)
        {
            if (Main.ActiveWorldFileData.HasCorruption && !Main.ActiveWorldFileData.HasCrimson
                && !ModContent.GetInstance<Configs.DebugConfig>().GenWasteland)
            {
                orig(instance, flat);
                return;
            }

            if (Main.screenPosition.Y + Main.screenHeight < (Main.maxTilesY - 220) * 16f)
                return;

            Vector2 value = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            float num = (Main.GameViewMatrix.Zoom.Y - 1f) * 0.5f * 200f;

            for (int i = 4; i >= 0; i--)
            {
                var texture2D = texture[i];
                Vector2 vector = new Vector2(texture2D.Width, texture2D.Height) * 0.5f;

                float num2 = flat ? 1f : (i * 2 + 3f);
                Vector2 value2 = new Vector2(1f / num2);
                Rectangle value3 = new Rectangle(0, 0, texture2D.Width, texture2D.Height);

                float num3 = 1.3f;
                Vector2 zero = Vector2.Zero;

                switch (i)
                {
                    case 1:
                        {
                            int num4 = (int)(Main.GlobalTime * 8f) % 4;
                            value3 = new Rectangle((num4 >> 1) * (texture2D.Width >> 1), num4 % 2 * (texture2D.Height >> 1), texture2D.Width >> 1, texture2D.Height >> 1);
                            vector *= 0.5f;
                            zero.Y += 75f;
                            break;
                        }
                    case 2:
                        zero.Y += 75f;
                        break;
                    case 3:
                        zero.Y += 75f;
                        break;
                    case 4:
                        num3 = 0.5f;
                        zero.Y -= 25f;
                        break;
                }
                if (flat) num3 *= 1.5f;
                vector *= num3;

                if (flat)
                    zero.Y += (texture[0].Height >> 1) * 1.3f - vector.Y;

                zero.Y -= num;
                float num5 = num3 * value3.Width;
                float num6 = (value.X * value2.X) - vector.X + zero.X - (Main.screenWidth >> 1);
                int num7 = (int)(num6 / num5);

                for (int j = num7 - 2; j < num7 + 4 + (int)(Main.screenWidth / num5); j++)
                {
                    Main.spriteBatch.Draw(texture2D,
                        (new Vector2(j * num3 * (value3.Width / value2.X), (Main.maxTilesY - 200) * 16f) + vector - value) * value2 + value - Main.screenPosition - vector + zero,
                        new Rectangle?(value3), Color.Cyan, 0f, Vector2.Zero, num3, SpriteEffects.None, 0f);
                    if (i == 0)
                    {
                        int num8 = (int)(((new Vector2(j
                            * num3
                            * (value3.Width / value2.X), (Main.maxTilesY - 200) * 16f) + vector - value) * value2 + value - Main.screenPosition - vector + zero).Y + value3.Height * num3);
                        Main.spriteBatch.Draw(Main.blackTileTexture,
                            new Rectangle((int)((new Vector2(j * num3 * (value3.Width / value2.X), (Main.maxTilesY - 200) * 16f) + vector - value) * value2 + value - Main.screenPosition - vector + zero).X,
                            num8, (int)(value3.Width * num3), Math.Max(0, Main.screenHeight - num8)), new Color(11, 3, 7));
                    }
                }
            }
        }
    }
}
