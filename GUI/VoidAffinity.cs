using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROI.Players;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ROI.GUI
{
    internal sealed class VoidAffinity
    {
        private static Vector2 Offset => Main.playerInventory ? new Vector2(30, 270) : new Vector2(30, 130);

        private static Texture2D voidMeterFilled;
        private static Texture2D voidMeterEmpty;

        public static void Load()
        {
            voidMeterFilled = ModContent.GetTexture("ROI/Textures/UIElements/VoidMeterFull");
            voidMeterEmpty = ModContent.GetTexture("ROI/Textures/UIElements/VoidMeterEmpty");
        }

        public static void Unload()
        {
            voidMeterFilled?.Dispose();
            voidMeterEmpty?.Dispose();
        }

        static float oldPercent;
        static Texture2D tex;
        public static void Draw(SpriteBatch spriteBatch)
        {
            ROIPlayer player = Main.LocalPlayer.GetModPlayer<ROIPlayer>();

            var percent = player.voidAffinity / (float)player.maxVoidAffinity;
            if (oldPercent != percent)
            {
                if (Math.Abs(percent - oldPercent) <= 0.1f) oldPercent = percent;
                else oldPercent += oldPercent < percent ? 0.01f : -0.01f;
                tex = DrawPercent(oldPercent);
            }
            if (player.voidAffinity != 0)
            {
                spriteBatch.Draw(voidMeterEmpty, Offset, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
                spriteBatch.Draw(tex, Offset, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                Rectangle rect = new Rectangle((int)Offset.X, (int)Offset.Y, voidMeterFilled.Width, voidMeterFilled.Height);

                if (rect.Contains(Main.MouseScreen.ToPoint()))
                {
                    var text = $"Affinity: {player.voidAffinity}/{player.maxVoidAffinity}";
                    Main.spriteBatch.DrawString(Main.fontMouseText, text,
                        Main.MouseScreen - new Vector2(0, Main.fontMouseText.MeasureString(text).Y), Color.White);
                }
            }
        }

        public static Texture2D DrawPercent(float percent)
        {
            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, voidMeterFilled.Width, voidMeterFilled.Height);
            Color[] colorData = new Color[voidMeterFilled.Width * voidMeterFilled.Height];
            Color[] refTex = new Color[colorData.Length];
            voidMeterFilled.GetData(refTex);

            var radian = Math.PI / 180;
            var offset = MathHelper.ToRadians(180);
            var max = 6.28318530718 * percent;
            var center = (voidMeterFilled.Size() * .5f).ToPoint16();
            for (double delta = 0; delta < max; delta += radian)
            {
                var unit = new Vector2((float)Math.Sin(delta + offset), (float)Math.Cos(delta + offset));
                for (int k = 0; k < 60; k++)
                {
                    var pos = center + new Point16(
                        Utils.Clamp((int)(unit.X * k), -center.X, center.X - 1),
                        Utils.Clamp((int)(unit.Y * k), -center.Y, center.Y - 1)
                        );
                    var index = pos.Y * voidMeterFilled.Width + pos.X;
                    colorData[index] = refTex[index];
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}