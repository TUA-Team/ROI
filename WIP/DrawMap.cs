using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using static Terraria.Main;

namespace ROI.WIP
{
    /// <summary>
    /// Complete refactor/remake of the map drawing code
    /// 
    /// </summary>
    public class DrawMap
    {
        protected void NewDrawMap()
        {
            //string text = ""; Moved after because why would why declare something if it ain't gonna be used after a check
            if (!mapEnabled || !mapReady)
                return;

            CheckIfMapSectionContentIsLost(); //Moved there because it can be done before any variable assignement

            string hoverText = "";

            //float mapX = 0f;
            //float mapY = 0f;
            float mapX = 200f;
            float mapY = 300f;
            float num3 = mapX; //TBD
            float num4 = mapY; //TBD
            float scale = 2f;
            byte b = byte.MaxValue;
            int maxAmountMapSectionX = maxTilesX / textureMaxWidth; // Somehow was assigned as _, decompiler?
            int maxAmountMapSectionY = maxTilesY / textureMaxHeight;
            //float mapStartX = Lighting.offScreenTiles;
            //float mapStartY = Lighting.offScreenTiles;
            //float mapEndX = maxTilesX - Lighting.offScreenTiles - 1;
            //float mapEndX = maxTilesY - Lighting.offScreenTiles - 42;
            float mapStartX = 10f;
            float mapStartY = 10f;
            float mapEndX = maxTilesX - 10;
            float mapEndY = maxTilesY - 10;
            float realMapPositionX = 0f;
            float realMapPositionY = 0f;
            float num13 = 0f;
            float num14 = 0f;
            float mapWidthWithScale = mapEndX - 1f;
            float mapHeightWithScale = mapEndY - 1f;
            scale = (mapFullscreen ? mapFullscreenScale : ((mapStyle != 1) ? mapOverlayScale : mapMinimapScale));
            bool flag = false;
            #region define matrix
            Matrix transformMatrix = UIScaleMatrix;
            if (mapStyle != 1)
                transformMatrix = Matrix.Identity;

            if (mapFullscreen)
                transformMatrix = Matrix.Identity;
            #endregion
            if (!mapFullscreen && scale > 1f)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
                flag = true;
            }

            if (mapFullscreen)
            {
                scale = DrawMapFullScreen(mapStartX, mapEndX, mapStartY, mapEndY, out mapX, out mapY, out flag);
            }
            else if (mapStyle == 1)
            {
                miniMapWidth = 240;
                miniMapHeight = 240;
                miniMapX = screenWidth - miniMapWidth - 52;
                miniMapY = 90;
                _ = (float)miniMapHeight / (float)maxTilesY;
                if ((double)mapMinimapScale < 0.2)
                    mapMinimapScale = 0.2f;

                if (mapMinimapScale > 3f)
                    mapMinimapScale = 3f;

                if ((double)mapMinimapAlpha < 0.01)
                    mapMinimapAlpha = 0.01f;

                if (mapMinimapAlpha > 1f)
                    mapMinimapAlpha = 1f;

                scale = mapMinimapScale;
                b = (byte)(255f * mapMinimapAlpha);
                mapX = miniMapX;
                mapY = miniMapY;
                num3 = mapX;
                num4 = mapY;
                float realScreenPositionX = (screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f;
                float realScreenPositionY = (screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f;
                realMapPositionX = (0f - (realScreenPositionX - (float)(int)((screenPosition.X + (float)(PlayerInput.RealScreenWidth / 2)) / 16f))) * scale;
                realMapPositionY = (0f - (realScreenPositionY - (float)(int)((screenPosition.Y + (float)(PlayerInput.RealScreenHeight / 2)) / 16f))) * scale;
                mapWidthWithScale = (float)miniMapWidth / scale;
                mapHeightWithScale = (float)miniMapHeight / scale;
                num13 = (float)(int)realScreenPositionX - mapWidthWithScale / 2f;
                num14 = (float)(int)realScreenPositionY - mapHeightWithScale / 2f;
                _ = (float)maxTilesY + num14;
                float x = num3 - 6f;
                float y = num4 - 6f;
                spriteBatch.Draw(miniMapFrame2Texture, new Vector2(x, y), new Microsoft.Xna.Framework.Rectangle(0, 0, miniMapFrame2Texture.Width, miniMapFrame2Texture.Height), new Microsoft.Xna.Framework.Color(b, b, b, b), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
            else if (mapStyle == 2)
            {
                float num30 = (float)screenWidth / (float)maxTilesX;
                if (mapOverlayScale < num30)
                    mapOverlayScale = num30;

                if (mapOverlayScale > 16f)
                    mapOverlayScale = 16f;

                if ((double)mapOverlayAlpha < 0.01)
                    mapOverlayAlpha = 0.01f;

                if (mapOverlayAlpha > 1f)
                    mapOverlayAlpha = 1f;

                scale = mapOverlayScale;
                b = (byte)(255f * mapOverlayAlpha);
                _ = maxTilesX;
                _ = maxTilesY;
                float num31 = (screenPosition.X + (float)(screenWidth / 2)) / 16f;
                float num32 = (screenPosition.Y + (float)(screenHeight / 2)) / 16f;
                num31 *= scale;
                float num33 = num32 * scale;
                mapX = 0f - num31 + (float)(screenWidth / 2);
                mapY = 0f - num33 + (float)(screenHeight / 2);
                mapX += mapStartX * scale;
                mapY += mapStartY * scale;
            }

            if (mapStyle == 1 && !mapFullscreen)
            {
                if (num13 < mapStartX)
                    mapX -= (num13 - mapStartX) * scale;

                if (num14 < mapStartY)
                    mapY -= (num14 - mapStartY) * scale;
            }

            mapWidthWithScale = num13 + mapWidthWithScale;
            mapHeightWithScale = num14 + mapHeightWithScale;
            if (num13 > mapStartX)
                mapStartX = num13;

            if (num14 > mapStartY)
                mapStartY = num14;

            if (mapWidthWithScale < mapEndX)
                mapEndX = mapWidthWithScale;

            if (mapHeightWithScale < mapEndY)
                mapEndY = mapHeightWithScale;

            float num34 = (float)textureMaxWidth * scale;
            float num35 = (float)textureMaxHeight * scale;
            float num36 = mapX;
            float num37 = 0f;
            for (int k = 0; k <= mapTargetX - 1; k++)
            {
                if (!((float)((k + 1) * textureMaxWidth) > mapStartX) || !((float)(k * textureMaxWidth) < mapStartX + mapEndX))
                    continue;

                for (int l = 0; l <= maxAmountMapSectionY; l++)
                {
                    if ((float)((l + 1) * textureMaxHeight) > mapStartY && (float)(l * textureMaxHeight) < mapStartY + mapEndY)
                    {
                        float num38 = mapX + (float)(int)((float)k * num34);
                        float num39 = mapY + (float)(int)((float)l * num35);
                        float num40 = k * textureMaxWidth;
                        float num41 = l * textureMaxHeight;
                        float num42 = 0f;
                        float num43 = 0f;
                        if (num40 < mapStartX)
                        {
                            num42 = mapStartX - num40;
                            num38 = mapX;
                        }
                        else
                        {
                            num38 -= mapStartX * scale;
                        }

                        if (num41 < mapStartY)
                        {
                            num43 = mapStartY - num41;
                            num39 = mapY;
                        }
                        else
                        {
                            num39 -= mapStartY * scale;
                        }

                        num38 = num36;
                        float num44 = textureMaxWidth;
                        float num45 = textureMaxHeight;
                        float num46 = (k + 1) * textureMaxWidth;
                        float num47 = (l + 1) * textureMaxHeight;
                        if (num46 >= mapEndX)
                            num44 -= num46 - mapEndX;

                        if (num47 >= mapEndY)
                            num45 -= num47 - mapEndY;

                        num38 += realMapPositionX;
                        num39 += realMapPositionY;
                        if (num44 > num42)
                            spriteBatch.Draw(Main.instance.mapTarget[k, l], new Vector2(num38, num39), new Microsoft.Xna.Framework.Rectangle((int)num42, (int)num43, (int)num44 - (int)num42, (int)num45 - (int)num43), new Microsoft.Xna.Framework.Color(b, b, b, b), 0f, default(Vector2), scale, SpriteEffects.None, 0f);

                        num37 = (float)((int)num44 - (int)num42) * scale;
                    }

                    if (l == maxAmountMapSectionY)
                        num36 += num37;
                }
            }

            if (flag)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
            }

            if (!mapFullscreen)
            {
                if (mapStyle == 2)
                {
                    float num48 = (scale * 0.2f * 2f + 1f) / 3f;
                    if (num48 > 1f)
                        num48 = 1f;

                    num48 *= UIScale;
                    for (int m = 0; m < 200; m++)
                    {
                        if (npc[m].active && npc[m].townNPC)
                        {
                            int num49 = NPC.TypeToHeadIndex(npc[m].type);
                            if (num49 > 0)
                            {
                                SpriteEffects effects = SpriteEffects.None;
                                if (npc[m].direction > 0)
                                    effects = SpriteEffects.FlipHorizontally;

                                float num50 = (npc[m].position.X + (float)(npc[m].width / 2)) / 16f * scale;
                                float num51 = (npc[m].position.Y + (float)(npc[m].height / 2)) / 16f * scale;
                                num50 += mapX;
                                num51 += mapY;
                                num50 -= 10f * scale;
                                num51 -= 10f * scale;
                                spriteBatch.Draw(npcHeadTexture[num49], new Vector2(num50, num51), new Microsoft.Xna.Framework.Rectangle(0, 0, npcHeadTexture[num49].Width, npcHeadTexture[num49].Height), new Microsoft.Xna.Framework.Color(b, b, b, b), 0f, new Vector2(npcHeadTexture[num49].Width / 2, npcHeadTexture[num49].Height / 2), num48, effects, 0f);
                            }
                        }

                        if (!npc[m].active || npc[m].GetBossHeadTextureIndex() == -1)
                            continue;

                        float bossHeadRotation = npc[m].GetBossHeadRotation();
                        SpriteEffects bossHeadSpriteEffects = npc[m].GetBossHeadSpriteEffects();
                        Vector2 vector = npc[m].Center + new Vector2(0f, npc[m].gfxOffY);
                        if (npc[m].type == 134)
                        {
                            Vector2 center = npc[m].Center;
                            int num52 = 1;
                            int num53 = (int)npc[m].ai[0];
                            while (num52 < 15 && npc[num53].active && npc[num53].type >= 134 && npc[num53].type <= 136)
                            {
                                num52++;
                                center += npc[num53].Center;
                                num53 = (int)npc[num53].ai[0];
                            }

                            center /= (float)num52;
                            vector = center;
                        }

                        int bossHeadTextureIndex = npc[m].GetBossHeadTextureIndex();
                        float num54 = vector.X / 16f * scale;
                        float num55 = vector.Y / 16f * scale;
                        num54 += mapX;
                        num55 += mapY;
                        num54 -= 10f * scale;
                        num55 -= 10f * scale;
                        spriteBatch.Draw(npcHeadBossTexture[bossHeadTextureIndex], new Vector2(num54, num55), null, new Microsoft.Xna.Framework.Color(b, b, b, b), bossHeadRotation, npcHeadBossTexture[bossHeadTextureIndex].Size() / 2f, num48, bossHeadSpriteEffects, 0f);
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
                    for (int n = 0; n < 255; n++)
                    {
                        if (player[n].active && !player[n].dead && n != myPlayer && ((!player[myPlayer].hostile && !player[n].hostile) || (player[myPlayer].team == player[n].team && player[n].team != 0) || n == myPlayer))
                        {
                            float num56 = (player[n].position.X + (float)(player[n].width / 2)) / 16f * scale;
                            float num57 = player[n].position.Y / 16f * scale;
                            num56 += mapX;
                            num57 += mapY;
                            num56 -= 6f;
                            num57 -= 2f;
                            num57 -= 2f - scale / 5f * 2f;
                            num56 -= 10f * scale;
                            num57 -= 10f * scale;
                            Main.instance.DrawPlayerHead(player[n], num56, num57, (float)(int)b / 255f, num48);
                        }
                    }

                    spriteBatch.End();
                    spriteBatch.Begin();
                }

                if (mapStyle == 1)
                {
                    float num58 = num3 - 6f;
                    float num59 = num4 - 6f;
                    float num60 = (scale * 0.25f * 2f + 1f) / 3f;
                    if (num60 > 1f)
                        num60 = 1f;

                    for (int num61 = 0; num61 < 200; num61++)
                    {
                        if (npc[num61].active && npc[num61].townNPC)
                        {
                            int num62 = NPC.TypeToHeadIndex(npc[num61].type);
                            if (num62 > 0)
                            {
                                SpriteEffects effects2 = SpriteEffects.None;
                                if (npc[num61].direction > 0)
                                    effects2 = SpriteEffects.FlipHorizontally;

                                float num63 = ((npc[num61].position.X + (float)(npc[num61].width / 2)) / 16f - num13) * scale;
                                float num64 = ((npc[num61].position.Y + npc[num61].gfxOffY + (float)(npc[num61].height / 2)) / 16f - num14) * scale;
                                num63 += num3;
                                num64 += num4;
                                num64 -= 2f * scale / 5f;
                                if (num63 > (float)(miniMapX + 12) && num63 < (float)(miniMapX + miniMapWidth - 16) && num64 > (float)(miniMapY + 10) && num64 < (float)(miniMapY + miniMapHeight - 14))
                                {
                                    spriteBatch.Draw(npcHeadTexture[num62], new Vector2(num63 + realMapPositionX, num64 + realMapPositionY), new Microsoft.Xna.Framework.Rectangle(0, 0, npcHeadTexture[num62].Width, npcHeadTexture[num62].Height), new Microsoft.Xna.Framework.Color(b, b, b, b), 0f, new Vector2(npcHeadTexture[num62].Width / 2, npcHeadTexture[num62].Height / 2), num60, effects2, 0f);
                                    float num65 = num63 - (float)(npcHeadTexture[num62].Width / 2) * num60;
                                    float num66 = num64 - (float)(npcHeadTexture[num62].Height / 2) * num60;
                                    float num67 = num65 + (float)npcHeadTexture[num62].Width * num60;
                                    float num68 = num66 + (float)npcHeadTexture[num62].Height * num60;
                                    if ((float)mouseX >= num65 && (float)mouseX <= num67 && (float)mouseY >= num66 && (float)mouseY <= num68)
                                        hoverText = npc[num61].FullName;
                                }
                            }
                        }

                        if (!npc[num61].active || npc[num61].GetBossHeadTextureIndex() == -1)
                            continue;

                        float bossHeadRotation2 = npc[num61].GetBossHeadRotation();
                        SpriteEffects bossHeadSpriteEffects2 = npc[num61].GetBossHeadSpriteEffects();
                        Vector2 vector2 = npc[num61].Center + new Vector2(0f, npc[num61].gfxOffY);
                        if (npc[num61].type == 134)
                        {
                            Vector2 center2 = npc[num61].Center;
                            int num69 = 1;
                            int num70 = (int)npc[num61].ai[0];
                            while (num69 < 15 && npc[num70].active && npc[num70].type >= 134 && npc[num70].type <= 136)
                            {
                                num69++;
                                center2 += npc[num70].Center;
                                num70 = (int)npc[num70].ai[0];
                            }

                            center2 /= (float)num69;
                            vector2 = center2;
                        }

                        int bossHeadTextureIndex2 = npc[num61].GetBossHeadTextureIndex();
                        float num71 = (vector2.X / 16f - num13) * scale;
                        float num72 = (vector2.Y / 16f - num14) * scale;
                        num71 += num3;
                        num72 += num4;
                        num72 -= 2f * scale / 5f;
                        if (num71 > (float)(miniMapX + 12) && num71 < (float)(miniMapX + miniMapWidth - 16) && num72 > (float)(miniMapY + 10) && num72 < (float)(miniMapY + miniMapHeight - 14))
                        {
                            spriteBatch.Draw(npcHeadBossTexture[bossHeadTextureIndex2], new Vector2(num71 + realMapPositionX, num72 + realMapPositionY), null, new Microsoft.Xna.Framework.Color(b, b, b, b), bossHeadRotation2, npcHeadBossTexture[bossHeadTextureIndex2].Size() / 2f, num60, bossHeadSpriteEffects2, 0f);
                            float num73 = num71 - (float)(npcHeadBossTexture[bossHeadTextureIndex2].Width / 2) * num60;
                            float num74 = num72 - (float)(npcHeadBossTexture[bossHeadTextureIndex2].Height / 2) * num60;
                            float num75 = num73 + (float)npcHeadBossTexture[bossHeadTextureIndex2].Width * num60;
                            float num76 = num74 + (float)npcHeadBossTexture[bossHeadTextureIndex2].Height * num60;
                            if ((float)mouseX >= num73 && (float)mouseX <= num75 && (float)mouseY >= num74 && (float)mouseY <= num76)
                                hoverText = npc[num61].GivenOrTypeName;
                        }
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, UIScaleMatrix);
                    for (int num77 = 0; num77 < 255; num77++)
                    {
                        if (!player[num77].active || ((player[myPlayer].hostile || player[num77].hostile) && (player[myPlayer].team != player[num77].team || player[num77].team == 0) && num77 != myPlayer))
                            continue;

                        float num78 = ((player[num77].position.X + (float)(player[num77].width / 2)) / 16f - num13) * scale;
                        float num79 = ((player[num77].position.Y + player[num77].gfxOffY + (float)(player[num77].height / 2)) / 16f - num14) * scale;
                        num78 += num3;
                        num79 += num4;
                        num78 -= 6f;
                        num79 -= 6f;
                        num79 -= 2f - scale / 5f * 2f;
                        num78 += realMapPositionX;
                        num79 += realMapPositionY;
                        if (screenPosition.X != leftWorld + 640f + 16f && screenPosition.X + (float)screenWidth != rightWorld - 640f - 32f && screenPosition.Y != topWorld + 640f + 16f && !(screenPosition.Y + (float)screenHeight > bottomWorld - 640f - 32f) && num77 == myPlayer && zoomX == 0f && zoomY == 0f)
                        {
                            num78 = num3 + (float)(miniMapWidth / 2);
                            num79 = num4 + (float)(miniMapHeight / 2);
                            num79 -= 3f;
                            num78 -= 4f;
                        }

                        if (!player[num77].dead && num78 > (float)(miniMapX + 6) && num78 < (float)(miniMapX + miniMapWidth - 16) && num79 > (float)(miniMapY + 6) && num79 < (float)(miniMapY + miniMapHeight - 14))
                        {
                            Main.instance.DrawPlayerHead(player[num77], num78, num79, (float)(int)b / 255f, num60);
                            if (num77 != myPlayer)
                            {
                                float num80 = num78 + 4f - 14f * num60;
                                float num81 = num79 + 2f - 14f * num60;
                                float num82 = num80 + 28f * num60;
                                float num83 = num81 + 28f * num60;
                                if ((float)mouseX >= num80 && (float)mouseX <= num82 && (float)mouseY >= num81 && (float)mouseY <= num83)
                                    hoverText = player[num77].name;
                            }
                        }

                        if (!player[num77].showLastDeath)
                            continue;

                        num78 = (player[num77].lastDeathPostion.X / 16f - num13) * scale;
                        num79 = (player[num77].lastDeathPostion.Y / 16f - num14) * scale;
                        num78 += num3;
                        num79 += num4;
                        num79 -= 2f - scale / 5f * 2f;
                        num78 += realMapPositionX;
                        num79 += realMapPositionY;
                        if (num78 > (float)(miniMapX + 8) && num78 < (float)(miniMapX + miniMapWidth - 18) && num79 > (float)(miniMapY + 8) && num79 < (float)(miniMapY + miniMapHeight - 16))
                        {
                            spriteBatch.Draw(Main.instance.mapDeathTexture, new Vector2(num78, num79), new Microsoft.Xna.Framework.Rectangle(0, 0, mapDeathTexture.Width, mapDeathTexture.Height), Microsoft.Xna.Framework.Color.White, 0f, new Vector2((float)mapDeathTexture.Width * 0.5f, (float)mapDeathTexture.Height * 0.5f), num60, SpriteEffects.None, 0f);
                            float num84 = num78 + 4f - 14f * num60;
                            float num85 = num79 + 2f - 14f * num60;
                            num84 -= 4f;
                            num85 -= 4f;
                            float num86 = num84 + 28f * num60;
                            float num87 = num85 + 28f * num60;
                            if ((float)mouseX >= num84 && (float)mouseX <= num86 && (float)mouseY >= num85 && (float)mouseY <= num87)
                            {
                                TimeSpan timeSpan = DateTime.Now - player[num77].lastDeathTime;
                                hoverText = Language.GetTextValue("Game.PlayerDeathTime", player[num77].name, Lang.LocalizedDuration(timeSpan, abbreviated: false, showAllAvailableUnits: false));
                            }
                        }
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, UIScaleMatrix);
                    spriteBatch.Draw(miniMapFrameTexture, new Vector2(num58, num59), new Microsoft.Xna.Framework.Rectangle(0, 0, miniMapFrameTexture.Width, miniMapFrameTexture.Height), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                    for (int num88 = 0; num88 < 3; num88++)
                    {
                        float num89 = num58 + 148f + (float)(num88 * 26);
                        float num90 = num59 + 234f;
                        if (!((float)mouseX > num89) || !((float)mouseX < num89 + 22f) || !((float)mouseY > num90) || !((float)mouseY < num90 + 22f))
                            continue;

                        spriteBatch.Draw(miniMapButtonTexture[num88], new Vector2(num89, num90), new Microsoft.Xna.Framework.Rectangle(0, 0, miniMapButtonTexture[num88].Width, miniMapButtonTexture[num88].Height), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                        if (PlayerInput.IgnoreMouseInterface)
                            continue;

                        player[myPlayer].mouseInterface = true;
                        if (mouseLeft)
                        {
                            if (mouseLeftRelease)
                                PlaySound(12);

                            switch (num88)
                            {
                                case 0:
                                    mapMinimapScale = 1.25f;
                                    break;
                                case 1:
                                    mapMinimapScale *= 0.975f;
                                    break;
                                case 2:
                                    mapMinimapScale *= 1.025f;
                                    break;
                            }
                        }
                    }
                }
            }

            if (mapFullscreen)
            {
                int num91 = (int)((0f - mapX + (float)mouseX) / scale + mapStartX);
                int num92 = (int)((0f - mapY + (float)mouseY) / scale + mapStartY);
                bool flag2 = false;
                if ((float)num91 < mapStartX)
                    flag2 = true;

                if ((float)num91 >= mapEndX)
                    flag2 = true;

                if ((float)num92 < mapStartY)
                    flag2 = true;

                if ((float)num92 >= mapEndY)
                    flag2 = true;

                if (!flag2 && Map[num91, num92].Light > 40)
                {
                    int type = Map[num91, num92].Type;
                    int num93 = MapHelper.tileLookup[21];
                    int num94 = MapHelper.tileLookup[441];
                    int num95 = MapHelper.tileOptionCounts[21];
                    int num96 = MapHelper.tileLookup[467];
                    int num97 = MapHelper.tileLookup[468];
                    int num98 = MapHelper.tileOptionCounts[467];
                    int num99 = MapHelper.tileLookup[88];
                    int num100 = MapHelper.tileOptionCounts[88];
                    LocalizedText[] chestType = Lang.chestType;
                    LocalizedText[] chestType2 = Lang.chestType2;
                    if (type >= num93 && type < num93 + num95)
                    {
                        Tile tile = Main.tile[num91, num92];
                        if (tile != null)
                        {
                            int num101 = num91;
                            int num102 = num92;
                            if (tile.frameX % 36 != 0)
                                num101--;

                            if (tile.frameY % 36 != 0)
                                num102--;

                            hoverText = DrawMap_FindChestName(chestType, tile, num101, num102);
                        }
                    }
                    else if (type >= num96 && type < num96 + num98)
                    {
                        Tile tile2 = Main.tile[num91, num92];
                        if (tile2 != null)
                        {
                            int num103 = num91;
                            int num104 = num92;
                            if (tile2.frameX % 36 != 0)
                                num103--;

                            if (tile2.frameY % 36 != 0)
                                num104--;

                            hoverText = DrawMap_FindChestName(chestType2, tile2, num103, num104);
                        }
                    }
                    else if (type >= num94 && type < num94 + num95)
                    {
                        Tile tile3 = Main.tile[num91, num92];
                        if (tile3 != null)
                        {
                            int num105 = num91;
                            int num106 = num92;
                            if (tile3.frameX % 36 != 0)
                                num105--;

                            if (tile3.frameY % 36 != 0)
                                num106--;

                            hoverText = chestType[tile3.frameX / 36].Value;
                        }
                    }
                    else if (type >= num97 && type < num97 + num98)
                    {
                        Tile tile4 = Main.tile[num91, num92];
                        if (tile4 != null)
                        {
                            int num107 = num91;
                            int num108 = num92;
                            if (tile4.frameX % 36 != 0)
                                num107--;

                            if (tile4.frameY % 36 != 0)
                                num108--;

                            hoverText = chestType2[tile4.frameX / 36].Value;
                        }
                    }
                    else if (type >= num99 && type < num99 + num100)
                    {
                        //patch file: num91, num92
                        Tile tile5 = Main.tile[num91, num92];
                        if (tile5 != null)
                        {
                            int num109 = num92;
                            int x2 = num91 - tile5.frameX % 54 / 18;
                            if (tile5.frameY % 36 != 0)
                                num109--;

                            int num110 = Chest.FindChest(x2, num109);
                            hoverText = ((num110 < 0) ? Lang.dresserType[0].Value : ((!(chest[num110].name != "")) ? Lang.dresserType[tile5.frameX / 54].Value : (Lang.dresserType[tile5.frameX / 54].Value + ": " + chest[num110].name)));
                        }
                    }
                    else
                    {
                        hoverText = Lang.GetMapObjectName(type);
                        hoverText = Lang._mapLegendCache.FromTile(Map[num91, num92], num91, num92);
                    }
                }

                float num111 = (scale * 0.25f * 2f + 1f) / 3f;
                if (num111 > 1f)
                    num111 = 1f;

                num111 = 1f;
                num111 = UIScale;
                for (int num112 = 0; num112 < 200; num112++)
                {
                    if (npc[num112].active && npc[num112].townNPC)
                    {
                        int num113 = NPC.TypeToHeadIndex(npc[num112].type);
                        if (num113 > 0)
                        {
                            SpriteEffects effects3 = SpriteEffects.None;
                            if (npc[num112].direction > 0)
                                effects3 = SpriteEffects.FlipHorizontally;

                            float num114 = (npc[num112].position.X + (float)(npc[num112].width / 2)) / 16f * scale;
                            float num115 = (npc[num112].position.Y + npc[num112].gfxOffY + (float)(npc[num112].height / 2)) / 16f * scale;
                            num114 += mapX;
                            num115 += mapY;
                            num114 -= 10f * scale;
                            num115 -= 10f * scale;
                            spriteBatch.Draw(npcHeadTexture[num113], new Vector2(num114, num115), new Microsoft.Xna.Framework.Rectangle(0, 0, npcHeadTexture[num113].Width, npcHeadTexture[num113].Height), new Microsoft.Xna.Framework.Color(b, b, b, b), 0f, new Vector2(npcHeadTexture[num113].Width / 2, npcHeadTexture[num113].Height / 2), num111, effects3, 0f);
                            float num116 = num114 - (float)(npcHeadTexture[num113].Width / 2) * num111;
                            float num117 = num115 - (float)(npcHeadTexture[num113].Height / 2) * num111;
                            float num118 = num116 + (float)npcHeadTexture[num113].Width * num111;
                            float num119 = num117 + (float)npcHeadTexture[num113].Height * num111;
                            if ((float)mouseX >= num116 && (float)mouseX <= num118 && (float)mouseY >= num117 && (float)mouseY <= num119)
                                hoverText = npc[num112].FullName;
                        }
                    }

                    if (!npc[num112].active || npc[num112].GetBossHeadTextureIndex() == -1)
                        continue;

                    float bossHeadRotation3 = npc[num112].GetBossHeadRotation();
                    SpriteEffects bossHeadSpriteEffects3 = npc[num112].GetBossHeadSpriteEffects();
                    Vector2 vector3 = npc[num112].Center + new Vector2(0f, npc[num112].gfxOffY);
                    if (npc[num112].type == 134)
                    {
                        Vector2 center3 = npc[num112].Center;
                        int num120 = 1;
                        int num121 = (int)npc[num112].ai[0];
                        while (num120 < 15 && npc[num121].active && npc[num121].type >= 134 && npc[num121].type <= 136)
                        {
                            num120++;
                            center3 += npc[num121].Center;
                            num121 = (int)npc[num121].ai[0];
                        }

                        center3 /= (float)num120;
                        vector3 = center3;
                    }

                    int bossHeadTextureIndex3 = npc[num112].GetBossHeadTextureIndex();
                    float num122 = vector3.X / 16f * scale;
                    float num123 = vector3.Y / 16f * scale;
                    num122 += mapX;
                    num123 += mapY;
                    num122 -= 10f * scale;
                    num123 -= 10f * scale;
                    spriteBatch.Draw(npcHeadBossTexture[bossHeadTextureIndex3], new Vector2(num122, num123), null, new Microsoft.Xna.Framework.Color(b, b, b, b), bossHeadRotation3, npcHeadBossTexture[bossHeadTextureIndex3].Size() / 2f, num111, bossHeadSpriteEffects3, 0f);
                    float num124 = num122 - (float)(npcHeadBossTexture[bossHeadTextureIndex3].Width / 2) * num111;
                    float num125 = num123 - (float)(npcHeadBossTexture[bossHeadTextureIndex3].Height / 2) * num111;
                    float num126 = num124 + (float)npcHeadBossTexture[bossHeadTextureIndex3].Width * num111;
                    float num127 = num125 + (float)npcHeadBossTexture[bossHeadTextureIndex3].Height * num111;
                    if ((float)mouseX >= num124 && (float)mouseX <= num126 && (float)mouseY >= num125 && (float)mouseY <= num127)
                        hoverText = npc[num112].GivenOrTypeName;
                }

                bool flag3 = false;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                for (int num128 = 0; num128 < 255; num128++)
                {
                    if (player[num128].active && ((!player[myPlayer].hostile && !player[num128].hostile) || (player[myPlayer].team == player[num128].team && player[num128].team != 0) || num128 == myPlayer) && player[num128].showLastDeath)
                    {
                        float num129 = (player[num128].lastDeathPostion.X / 16f - num13) * scale;
                        float num130 = (player[num128].lastDeathPostion.Y / 16f - num14) * scale;
                        num129 += mapX;
                        num130 += mapY;
                        num130 -= 2f - scale / 5f * 2f;
                        num129 -= 10f * scale;
                        num130 -= 10f * scale;
                        spriteBatch.Draw(Main.instance.mapDeathTexture, new Vector2(num129, num130), new Microsoft.Xna.Framework.Rectangle(0, 0, mapDeathTexture.Width, mapDeathTexture.Height), Microsoft.Xna.Framework.Color.White, 0f, new Vector2((float)mapDeathTexture.Width * 0.5f, (float)mapDeathTexture.Height * 0.5f), num111, SpriteEffects.None, 0f);
                        float num131 = num129 + 4f - 14f * num111;
                        float num132 = num130 + 2f - 14f * num111;
                        float num133 = num131 + 28f * num111;
                        float num134 = num132 + 28f * num111;
                        if ((float)mouseX >= num131 && (float)mouseX <= num133 && (float)mouseY >= num132 && (float)mouseY <= num134)
                        {
                            TimeSpan timeSpan2 = DateTime.Now - player[num128].lastDeathTime;
                            hoverText = Language.GetTextValue("Game.PlayerDeathTime", player[num128].name, Lang.LocalizedDuration(timeSpan2, abbreviated: false, showAllAvailableUnits: false));
                        }
                    }
                }

                for (int num135 = 0; num135 < 255; num135++)
                {
                    if (!player[num135].active || ((player[myPlayer].hostile || player[num135].hostile) && (player[myPlayer].team != player[num135].team || player[num135].team == 0) && num135 != myPlayer))
                        continue;

                    float num136 = ((player[num135].position.X + (float)(player[num135].width / 2)) / 16f - num13) * scale;
                    float num137 = ((player[num135].position.Y + player[num135].gfxOffY + (float)(player[num135].height / 2)) / 16f - num14) * scale;
                    num136 += mapX;
                    num137 += mapY;
                    num136 -= 6f;
                    num137 -= 2f;
                    num137 -= 2f - scale / 5f * 2f;
                    num136 -= 10f * scale;
                    num137 -= 10f * scale;
                    float num138 = num136 + 4f - 14f * num111;
                    float num139 = num137 + 2f - 14f * num111;
                    float num140 = num138 + 28f * num111;
                    float num141 = num139 + 28f * num111;
                    if (player[num135].dead)
                        continue;

                    Main.instance.DrawPlayerHead(player[num135], num136, num137, (float)(int)b / 255f, num111);
                    if (!((float)mouseX >= num138) || !((float)mouseX <= num140) || !((float)mouseY >= num139) || !((float)mouseY <= num141))
                        continue;

                    hoverText = player[num135].name;
                    if (num135 != myPlayer && player[myPlayer].team > 0 && player[myPlayer].team == player[num135].team && netMode == 1 && player[myPlayer].HasUnityPotion())
                    {
                        flag3 = true;
                        if (!Main.instance.unityMouseOver)
                            PlaySound(12);

                        Main.instance.unityMouseOver = true;
                        Main.instance.DrawPlayerHead(player[num135], num136, num137, 2f, num111 + 0.5f);
                        hoverText = Language.GetTextValue("Game.TeleportTo", player[num135].name);
                        if (mouseLeft && mouseLeftRelease)
                        {
                            mouseLeftRelease = false;
                            mapFullscreen = false;
                            player[myPlayer].UnityTeleport(player[num135].position);
                            player[myPlayer].TakeUnityPotion();
                        }
                    }
                }

                if (!flag3 && Main.instance.unityMouseOver)
                    Main.instance.unityMouseOver = false;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, UIScaleMatrix);
                PlayerInput.SetZoom_UI();
                int num142 = 10;
                int num143 = screenHeight - 40;
                if (showFrameRate)
                    num143 -= 15;

                int num144 = 0;
                int num145 = 130;
                if (mouseX >= num142 && mouseX <= num142 + 32 && mouseY >= num143 && mouseY <= num143 + 30)
                {
                    num145 = 255;
                    num144 += 4;
                    player[myPlayer].mouseInterface = true;
                    if (mouseLeft && mouseLeftRelease)
                    {
                        PlaySound(10);
                        mapFullscreen = false;
                    }
                }

                spriteBatch.Draw(mapIconTexture[num144], new Vector2(num142, num143), new Microsoft.Xna.Framework.Rectangle(0, 0, mapIconTexture[num144].Width, mapIconTexture[num144].Height), new Microsoft.Xna.Framework.Color(num145, num145, num145, num145), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                ModHooks.PostDrawFullscreenMap(ref hoverText);
                DrawCursor(DrawThickCursor());
            }

            if (hoverText != "")
                Main.instance.MouseText(hoverText, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin();
            PlayerInput.SetZoom_Unscaled();
            TimeLogger.DetailedDrawTime(9);
        }

        private static float DrawMapFullScreen(float mapStartX, float mapEndX, float mapStartY, float mapEndY, out float mapX, out float mapY, out bool flag)
        {
            float scale;
            if (mouseLeft && Main.instance.IsActive && !CaptureManager.Instance.UsingMap)
            {
                if (mouseLeftRelease)
                {
                    grabMapX = mouseX;
                    grabMapY = mouseY;
                }
                else
                {
                    float num17 = (float) mouseX - grabMapX;
                    float num18 = (float) mouseY - grabMapY;
                    grabMapX = mouseX;
                    grabMapY = mouseY;
                    num17 *= 0.06255f;
                    num18 *= 0.06255f;
                    mapFullscreenPos.X -= num17 * (16f / mapFullscreenScale);
                    mapFullscreenPos.Y -= num18 * (16f / mapFullscreenScale);
                }
            }

            player[myPlayer].mouseInterface = true;
            float num19 = (float) screenWidth / (float) maxTilesX * 0.8f;
            if (mapFullscreenScale < num19)
                mapFullscreenScale = num19;

            if (mapFullscreenScale > 16f)
                mapFullscreenScale = 16f;

            scale = mapFullscreenScale;
            if (mapFullscreenPos.X < mapStartX)
                mapFullscreenPos.X = mapStartX;

            if (mapFullscreenPos.X > mapEndX)
                mapFullscreenPos.X = mapEndX;

            if (mapFullscreenPos.Y < mapStartY)
                mapFullscreenPos.Y = mapStartY;

            if (mapFullscreenPos.Y > mapEndY)
                mapFullscreenPos.Y = mapEndY;

            float num20 = mapFullscreenPos.X;
            float num21 = mapFullscreenPos.Y;
            if (resetMapFull)
            {
                resetMapFull = false;
                num20 = (screenPosition.X + (float) (screenWidth / 2)) / 16f;
                num21 = (screenPosition.Y + (float) (screenHeight / 2)) / 16f;
                mapFullscreenPos.X = num20;
                mapFullscreenPos.Y = num21;
            }

            num20 *= scale;
            num21 *= scale;
            mapX = 0f - num20 + (float) (screenWidth / 2);
            mapY = 0f - num21 + (float) (screenHeight / 2);
            mapX += mapStartX * scale;
            mapY += mapStartY * scale;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            flag = true;
            Texture2D modTexture = PlayerHooks.GetMapBackgroundImage(player[myPlayer]);
            if (modTexture != null)
            {
                spriteBatch.Draw(modTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
            }
            else if (screenPosition.Y > (maxTilesY - 232) * 16)
            {
                spriteBatch.Draw(Main.instance.mapBG3Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
            }
            else if (player[myPlayer].ZoneDungeon)
            {
                spriteBatch.Draw(Main.instance.mapBG5Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
            }
            else if (Main.tile[(int) (player[myPlayer].Center.X / 16f), (int) (player[myPlayer].Center.Y / 16f)].wall == 87)
            {
                spriteBatch.Draw(Main.instance.mapBG14Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
            }
            else if ((double) screenPosition.Y > worldSurface * 16.0)
            {
                if (player[myPlayer].ZoneSnow)
                    spriteBatch.Draw(Main.instance.mapBG4Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
                else if (player[myPlayer].ZoneJungle)
                    spriteBatch.Draw(Main.instance.mapBG13Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (sandTiles > 1000)
                    spriteBatch.Draw(Main.instance.mapBG15Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else
                    spriteBatch.Draw(Main.instance.mapBG2Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), Microsoft.Xna.Framework.Color.White);
            }
            else
            {
                int num27 = (int) ((screenPosition.X + (float) (screenWidth / 2)) / 16f);
                if (player[myPlayer].ZoneCorrupt)
                    spriteBatch.Draw(Main.instance.mapBG6Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (player[myPlayer].ZoneCrimson)
                    spriteBatch.Draw(Main.instance.mapBG7Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (player[myPlayer].ZoneHoly)
                    spriteBatch.Draw(Main.instance.mapBG8Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if ((double) (screenPosition.Y / 16f) < worldSurface + 10.0 && (num27 < 380 || num27 > maxTilesX - 380))
                    spriteBatch.Draw(Main.instance.mapBG11Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (player[myPlayer].ZoneSnow)
                    spriteBatch.Draw(Main.instance.mapBG12Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (player[myPlayer].ZoneJungle)
                    spriteBatch.Draw(Main.instance.mapBG9Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else if (sandTiles > 1000)
                    spriteBatch.Draw(Main.instance.mapBG10Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
                else
                    spriteBatch.Draw(Main.instance.mapBG1Texture, new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, screenHeight), bgColor);
            }

            /* Map texture drawing replaced by an adaptive drawing below, as mod worlds sometimes aren't regular sizes.
				Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle((int)num23, (int)num24, (int)num25, (int)num26);
				spriteBatch.Draw(mapTexture, destinationRectangle, Microsoft.Xna.Framework.Color.White);
				*/
            int x = (int) (mapX + mapFullscreenScale * 10);
            int y = (int) (mapY + mapFullscreenScale * 10);
            int width = (int) ((maxTilesX - 40) * mapFullscreenScale);
            int height = (int) ((maxTilesY - 40) * mapFullscreenScale);
            var destinationRectangle = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
            spriteBatch.Draw(mapTexture, destinationRectangle, new Microsoft.Xna.Framework.Rectangle(40, 4, 848, 240), Microsoft.Xna.Framework.Color.White);
            int edgeWidth = (int) (40 * mapFullscreenScale * 5);
            int edgeHeight = (int) (4 * mapFullscreenScale * 5);
            destinationRectangle = new Microsoft.Xna.Framework.Rectangle(x - edgeWidth, y - edgeHeight, edgeWidth, height + 2 * edgeHeight);
            spriteBatch.Draw(mapTexture, destinationRectangle, new Microsoft.Xna.Framework.Rectangle(0, 0, 40, 248), Microsoft.Xna.Framework.Color.White);
            destinationRectangle = new Microsoft.Xna.Framework.Rectangle(x + width, y - edgeHeight, edgeWidth, height + 2 * edgeHeight);
            spriteBatch.Draw(mapTexture, destinationRectangle, new Microsoft.Xna.Framework.Rectangle(888, 0, 40, 248), Microsoft.Xna.Framework.Color.White);
            if (scale < 1f)
            {
                spriteBatch.End();
                spriteBatch.Begin();
                flag = false;
            }

            return scale;
        }

        private static void CheckIfMapSectionContentIsLost()
        {
            for (int x = 0; x < Main.instance.mapTarget.GetLength(0); x++)
            {
                for (int y = 0; y < Main.instance.mapTarget.GetLength(1); y++)
                {
                    if (Main.instance.mapTarget[x, y] != null)
                    {
                        if (Main.instance.mapTarget[x, y].IsContentLost && !mapWasContentLost[x, y])
                        {
                            mapWasContentLost[x, y] = true;
                            refreshMap = true;
                            clearMap = true;
                        }
                        else if (!Main.instance.mapTarget[x, y].IsContentLost && mapWasContentLost[x, y])
                        {
                            mapWasContentLost[x, y] = false;
                        }
                    }
                }
            }
        }
    }
}
