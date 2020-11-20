using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland
{
    public sealed class WastelandBackground : API.IOnLoad
    {
        private readonly Texture2D[] _texture = new Texture2D[5];


        public void Load(Mod mod)
        {
            if (Main.dedServ)
                return;

            for (int i = 0; i < _texture.Length; i++)
                mod.GetTexture("Assets/Textures/Backgrounds/WastelandBackground" + i);

            On.Terraria.Main.DrawUnderworldBackground += DrawUnderworldBackground;

            Filters.Scene["ROI:UnderworldFilter"] = new Filter(new ScreenShaderData(new Ref<Effect>(mod.GetEffect("Effects/UnderworldFilter")), "UnderworldFilter"), EffectPriority.VeryHigh);
            Filters.Scene["ROI:UnderworldFilter"].Load();
        }


        public void DrawUnderworldBackground(On.Terraria.Main.orig_DrawUnderworldBackground orig, Main instance, bool flat)
        {
            if (Main.screenPosition.Y + Main.screenHeight < (Main.maxTilesY - 220) * 16f)
                return;

            Vector2 value = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            float num = (Main.GameViewMatrix.Zoom.Y - 1f) * 0.5f * 200f;

            for (int i = 4; i >= 0; i--)
            {
                Texture2D texture2D = _texture[i];
                Vector2 vector = new Vector2(texture2D.Width, texture2D.Height) * 0.5f;

                float num2 = flat ? 1f : i * 2 + 3f;
                Vector2 value2 = new Vector2(1f / num2);
                Rectangle value3 = new Rectangle(0, 0, texture2D.Width, texture2D.Height);

                float num3 = 1.3f;
                Vector2 zero = Vector2.Zero;

                switch (i)
                {
                    case 1:
                        {
                            int num4 = (int)(Main.time * 8f) % 4;
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
                    zero.Y += (_texture[0].Height >> 1) * 1.3f - vector.Y;

                zero.Y -= num;
                float num5 = num3 * value3.Width;
                float num6 = value.X * value2.X - vector.X + zero.X - (Main.screenWidth >> 1);
                int num7 = (int)(num6 / num5);

                for (int j = num7 - 2; j < num7 + 4 + (int)(Main.screenWidth / num5); j++)
                {
                    Vector2 value4 = new Vector2(j * num3 * (value3.Width / value2.X), (Main.maxTilesY - 200) * 16f) + vector;
                    Vector2 position = (value4 - value) * value2 + value - Main.screenPosition - vector + zero;
                    Main.spriteBatch.Draw(texture2D, position, new Rectangle?(value3), /*Microsoft.Xna.Framework.Color.White*/Color.Cyan, 0f, Vector2.Zero, num3, SpriteEffects.None, 0f);
                    if (i == 0)
                    {
                        int num8 = (int)(position.Y + value3.Height * num3);
                        Main.spriteBatch.Draw(Main.blackTileTexture,
                            new Rectangle((int)position.X, num8, (int)(value3.Width * num3), Math.Max(0, Main.screenHeight - num8)),
                            new Color(11, 3, 7));
                    }
                }
            }
        }
    }
}
