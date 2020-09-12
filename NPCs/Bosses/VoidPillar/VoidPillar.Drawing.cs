using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ROI.NPCs.Bosses.VoidPillar
{
    public sealed partial class VoidPillar
    {
        private float _teleportationRingScale = 0f;
        private float _teleportationRingOpacity = 0f;
        private float _teleportationRingRotation = 0f;
        private static readonly Texture2D RING_TEXTURE = ModContent.GetTexture("ROI/Textures/NPCs/VoidRing");

        /// <summary>
        /// Thanks direwolf420 for the help here :D
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="drawColor"></param>
        /// <returns></returns>
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (MovementAIPhase == 2f)
            {
                Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
                for (int k = (npc.oldPos.Length / 3); k < npc.oldPos.Length; k++)
                {
                    Vector2 drawPosition = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0, npc.gfxOffY);
                    Color color = npc.GetAlpha(drawColor) * ((float)(npc.oldPos.Length - k) / (2f * npc.oldPos.Length));
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPosition, npc.frame, color, npc.oldRot[k], drawOrigin, npc.scale, SpriteEffects.None, 0f);
                }
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            Vector2 center = npc.Center - Main.screenPosition;
            DrawData drawingData = new DrawData(TextureManager.Load("Images/Misc/Perlin"), center - new Vector2(0, 10), new Rectangle(0, 0, 600, 600), GetShieldColor(), npc.rotation, new Vector2(300, 300), Vector2.One, SpriteEffects.None, 0);
            GameShaders.Misc["ForceField"].UseColor(GetShieldColor());
            GameShaders.Misc["ForceField"].Apply(drawingData);
            drawingData.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            if (npc.ai[0] == 1)
            {
                TeleportationRingDraw(spriteBatch);
                return;
            }

            _teleportationRingScale = 0f;
            _teleportationRingOpacity = 0f;
        }

        private void TeleportationRingDraw(SpriteBatch spriteBatch)
        {
            if (_teleportationRingScale < 1.5f && _teleportationTimer > 250)
            {
                _teleportationRingScale += 0.005f;
            }

            if (_teleportationTimer <= 250)
            {
                _teleportationRingScale -= 0.005f;
            }

            if (_teleportationRingOpacity < 1f)
            {
                _teleportationRingOpacity += 0.005f;
            }

            if (_teleportationRingRotation >= Math.PI * 2)
            {
                _teleportationRingRotation = 0f;
            }
            if (_teleportationTimer < 250 && _teleportationRingOpacity >= 1f)
            {
                _teleportationRingOpacity = 0.5f;
            }
            _teleportationRingRotation += 0.05f;

            Vector2 location = npc.Center - Main.screenPosition;
            spriteBatch.Draw(RING_TEXTURE, location, new Rectangle(0, 0, RING_TEXTURE.Width, RING_TEXTURE.Height), Color.White * _teleportationRingOpacity, (float)(_teleportationRingRotation + 1.04719758f), new Vector2(RING_TEXTURE.Width / 2f, RING_TEXTURE.Height / 2f), _teleportationRingScale, SpriteEffects.None, 1f);
        }
    }
}
