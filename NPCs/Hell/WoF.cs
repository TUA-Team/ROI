using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Hell
{
    abstract class WoF : ModNPC
    {
        protected void DrawWoF()
        {
            if (Main.wof >= 0 && Main.player[Main.myPlayer].gross)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && Main.player[i].tongued && !Main.player[i].dead)
                    {
                        float num = Main.npc[npc.whoAmI].position.X + (float)(Main.npc[npc.whoAmI].width / 2);
                        float num2 = Main.npc[npc.whoAmI].position.Y + (float)(Main.npc[npc.whoAmI].height / 2);
                        Vector2 vector = new Vector2(Main.player[i].position.X + (float)Main.player[i].width * 0.5f, Main.player[i].position.Y + (float)Main.player[i].height * 0.5f);
                        float num3 = num - vector.X;
                        float num4 = num2 - vector.Y;
                        float rotation = (float)Math.Atan2((double)num4, (double)num3) - 1.57f;
                        bool flag = true;
                        while (flag)
                        {
                            float num5 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
                            if (num5 < 40f)
                            {
                                flag = false;
                            }
                            else
                            {
                                num5 = (float)Main.chain12Texture.Height / num5;
                                num3 *= num5;
                                num4 *= num5;
                                vector.X += num3;
                                vector.Y += num4;
                                num3 = num - vector.X;
                                num4 = num2 - vector.Y;
                                Microsoft.Xna.Framework.Color color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f));
                                Main.spriteBatch.Draw(Main.chain12Texture, new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain12Texture.Width, Main.chain12Texture.Height)), color, rotation, new Vector2((float)Main.chain12Texture.Width * 0.5f, (float)Main.chain12Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Main.npc[j].aiStyle == 29)
                    {
                        float num6 = Main.npc[Main.wof].position.X + (float)(Main.npc[Main.wof].width / 2);
                        float num7 = Main.npc[Main.wof].position.Y;
                        float num8 = (float)(Main.wofB - Main.wofT);
                        bool flag2 = false;
                        if (Main.npc[j].frameCounter > 7.0)
                        {
                            flag2 = true;
                        }
                        num7 = (float)Main.wofT + num8 * Main.npc[j].ai[0];
                        Vector2 vector2 = new Vector2(Main.npc[j].position.X + (float)(Main.npc[j].width / 2), Main.npc[j].position.Y + (float)(Main.npc[j].height / 2));
                        float num9 = num6 - vector2.X;
                        float num10 = num7 - vector2.Y;
                        float rotation2 = (float)Math.Atan2((double)num10, (double)num9) - 1.57f;
                        bool flag3 = true;
                        while (flag3)
                        {
                            SpriteEffects effects = SpriteEffects.None;
                            if (flag2)
                            {
                                effects = SpriteEffects.FlipHorizontally;
                                flag2 = false;
                            }
                            else
                            {
                                flag2 = true;
                            }
                            int height = 28;
                            float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                            if (num11 < 40f)
                            {
                                height = (int)num11 - 40 + 28;
                                flag3 = false;
                            }
                            num11 = 28f / num11;
                            num9 *= num11;
                            num10 *= num11;
                            vector2.X += num9;
                            vector2.Y += num10;
                            num9 = num6 - vector2.X;
                            num10 = num7 - vector2.Y;
                            Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)vector2.X / 16, (int)(vector2.Y / 16f));
                            Main.spriteBatch.Draw(Main.chain12Texture, new Vector2(vector2.X - Main.screenPosition.X, vector2.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.chain4Texture.Width, height)), color2, rotation2, new Vector2((float)Main.chain4Texture.Width * 0.5f, (float)Main.chain4Texture.Height * 0.5f), 1f, effects, 0f);
                        }
                    }
                }
                int num12 = 140;
                float num13 = (float)Main.wofT;
                float num14 = (float)Main.wofB;
                num14 = Main.screenPosition.Y + (float)Main.screenHeight;
                float num15 = (float)((int)((num13 - Main.screenPosition.Y) / (float)num12) + 1);
                num15 *= (float)num12;
                if (num15 > 0f)
                {
                    num13 -= num15;
                }
                float num16 = num13;
                float num17 = Main.npc[Main.wof].position.X;
                float num18 = num14 - num13;
                bool flag4 = true;
                SpriteEffects effects2 = SpriteEffects.None;
                if (Main.npc[Main.wof].spriteDirection == 1)
                {
                    effects2 = SpriteEffects.FlipHorizontally;
                }
                if (Main.npc[Main.wof].direction > 0)
                {
                    num17 -= 80f;
                }
                int num19 = 0;
                if (!Main.gamePaused)
                {
                    Main.wofF++;
                }
                if (Main.wofF > 12)
                {
                    num19 = 280;
                    if (Main.wofF > 17)
                    {
                        Main.wofF = 0;
                    }
                }
                else if (Main.wofF > 6)
                {
                    num19 = 140;
                }
                while (flag4)
                {
                    num18 = num14 - num16;
                    if (num18 > (float)num12)
                    {
                        num18 = (float)num12;
                    }
                    bool flag5 = true;
                    int num20 = 0;
                    while (flag5)
                    {
                        int x = (int)(num17 + (float)(Main.wofTexture.Width / 2)) / 16;
                        int y = (int)(num16 + (float)num20) / 16;
                        Main.spriteBatch.Draw(Main.wofTexture, new Vector2(num17 - Main.screenPosition.X, num16 + (float)num20 - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, num19 + num20, Main.wofTexture.Width, 16)), Lighting.GetColor(x, y), 0f, default(Vector2), 1f, effects2, 0f);
                        num20 += 16;
                        if ((float)num20 >= num18)
                        {
                            flag5 = false;
                        }
                    }
                    num16 += (float)num12;
                    if (num16 >= num14)
                    {
                        flag4 = false;
                    }
                }
            }
        }

    }
}
