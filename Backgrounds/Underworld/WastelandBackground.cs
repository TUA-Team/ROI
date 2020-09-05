using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace ROI.Backgrounds.Underworld
{
    public class WastelandBackground : Background
    {
        private readonly Texture2D[] _texture = new Texture2D[5];


        public WastelandBackground()
        {
            for (int i = 0; i < _texture.Length; i++)
                ROIMod.Instance.GetTexture("Backgrounds/Underworld/WastelandBackground" + i);

            On.Terraria.Main.DrawUnderworldBackground += DrawUnderworldBackground;
        }


        public void DrawUnderworldBackground(On.Terraria.Main.orig_DrawUnderworldBackground orig, Main instance, bool flat)
        {
            if (Main.ActiveWorldFileData.HasCorruption)
            {
                orig(instance, flat);
                return;
            }

            for (int i = 0; i < _texture.Length; i++)
                _texture[i] = Terraria.ModLoader.ModContent.GetTexture("ROI/Backgrounds/Underworld/Wasteland_" + i);

            if (!Main.ActiveWorldFileData.HasCrimson)
            {
                orig(instance, flat);
                return;
            }

            if (Main.screenPosition.Y + (float)Main.screenHeight < (float)(Main.maxTilesY - 220) * 16f)
                return;

            Vector2 value = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
            float num = (Main.GameViewMatrix.Zoom.Y - 1f) * 0.5f * 200f;

            for (int i = 4; i >= 0; i--)
            {
                Texture2D texture2D = _texture[i];
                Vector2 vector = new Vector2((float)texture2D.Width, (float)texture2D.Height) * 0.5f;

                float num2 = flat ? 1f : ((float)(i * 2) + 3f);
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
                if (flat)
                {
                    num3 *= 1.5f;
                }
                vector *= num3;

                if (flat)
                    zero.Y += (float)(_texture[0].Height >> 1) * 1.3f - vector.Y;

                zero.Y -= num;
                float num5 = num3 * (float)value3.Width;
                float num6 = value.X * value2.X - vector.X + zero.X - (float)(Main.screenWidth >> 1);
                int num7 = (int)(num6 / num5);

                for (int j = num7 - 2; j < num7 + 4 + (int)((float)Main.screenWidth / num5); j++)
                {
                    Vector2 value4 = new Vector2((float)j * num3 * ((float)value3.Width / value2.X), (float)(Main.maxTilesY - 200) * 16f) + vector;
                    Vector2 position = (value4 - value) * value2 + value - Main.screenPosition - vector + zero;
                    Main.spriteBatch.Draw(texture2D, position, new Rectangle?(value3), /*Microsoft.Xna.Framework.Color.White*/Color.Cyan, 0f, Vector2.Zero, num3, SpriteEffects.None, 0f);
                    if (i == 0)
                    {
                        int num8 = (int)(position.Y + (float)value3.Height * num3);
                        Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)position.X, num8, (int)((float)value3.Width * num3), Math.Max(0, Main.screenHeight - num8)), new Color(11, 3, 7));
                    }
                }
            }
        }
    }
}
