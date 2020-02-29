using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ROI.Helpers;
using ROI.Players;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.GUI
{
    internal sealed class VoidBuffList : UIState
    {
        private static readonly Vector2 Offset = new Vector2(30, 220);

        private float[] buffAlpha;

        public override void OnInitialize()
        {
            buffAlpha = new float[10];

            for (int i = 0; i < buffAlpha.Length; i++)
            {
                buffAlpha[i] = 0.4f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 10; i++)
            {
                var plr = Main.LocalPlayer.GetModPlayer<ROIPlayer>();
                var time = plr.buffTime[i];
                if (time == 0) return;
                var type = plr.buffType[i];
                var buff = VoidBuffHelper.GetBuff(type);
                var texture = ModContent.GetTexture(buff.Texture);
                var pos = Offset;
                pos.Y += i * 50f;
                Color color = new Color(buffAlpha[i], buffAlpha[i], buffAlpha[i], buffAlpha[i]);
                spriteBatch.Draw(texture, pos, color);
                string text = Lang.LocalizedDuration(new TimeSpan(0, 0, plr.buffTime[i] / 60), true, false);
                spriteBatch.DrawString(Main.fontItemStack, text, pos + new Vector2(0, 32), color);
                if (new Rectangle((int)pos.X, (int)pos.Y, 32, 32).Contains(new Point(Main.mouseX, Main.mouseY)))
                {
                    if (buffAlpha[i] < 1) buffAlpha[i] += 0.01f;
                    bool flag = Main.mouseRight && Main.mouseRightRelease;
                    if (PlayerInput.UsingGamepad)
                    {
                        flag = Main.mouseLeft && Main.mouseLeftRelease && Main.playerInventory;
                        if (Main.playerInventory)
                        {
                            Main.player[Main.myPlayer].mouseInterface = true;
                        }
                    }
                    else
                    {
                        Main.player[Main.myPlayer].mouseInterface = true;
                    }
                    if (flag)
                    {
                        plr.buffTime[i] = 0;
                        if (i != 9)
                        {
                            Array.Copy(plr.buffType, i + 1, plr.buffType, i, 9 - i);
                        }
                    }
                    else
                    {
                        //TODO: make VoidBuff localizable with proper ModTranslation stuff
                        text = $"{buff.DisplayName.GetDefault()}\n{buff.Description.GetDefault()}";
                        spriteBatch.DrawString(Main.fontItemStack, text, Main.MouseScreen, Color.White);
                    }
                }
                else
                {
                    if (buffAlpha[i] > 0.4) buffAlpha[i] -= 0.05f;
                }
            }
        }
    }
}