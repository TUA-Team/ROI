using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfinityRealm.NPCs.Void.VoidPillar
{
    /// <summary>
    /// AI slot usage
    /// <list type="bullet">
    /// 
    /// </list>>
    /// 0 -
    /// 1 -
    /// 2 -
    /// 3 -
    /// 
    /// </summary>
    internal sealed class VoidPillar : ModNPC
    {
        public override string Texture => "Terraria/NPC_507";

        private int movementTimer = 100;
        private PillarShieldColor shieldColor = PillarShieldColor.Black;
        private bool movementUp = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Pillar - Manifestation of the void");
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 174;
            npc.height = 364;
            npc.aiStyle = -1;
            npc.buffImmune.SetAllTrue();
            npc.lifeMax = 20000;
            music = MusicID.LunarBoss;
            npc.noGravity = true;
            npc.immortal = true;
            npc.dontTakeDamage = true;            
        }

        public override void AI()
        {
            PillarMovement();
        }

        public void DecideAttack()
        {
            switch (shieldColor)
            {
                case PillarShieldColor.Red:
                    break;
                case PillarShieldColor.Purple:
                    break;
                case PillarShieldColor.Black:
                    break;
                case PillarShieldColor.Green:
                    break;
                case PillarShieldColor.Blue:
                    break;
                default:
                    break;
            }
        }

        #region Pillar debuff

        /// <summary>
        /// Red color shield, will drain health from the player directly
        /// </summary>
        public void DebuffRed()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                {
                    continue;
                }
                PlayerDeathReason death = PlayerDeathReason.ByCustomReason(Main.player[i].name + " life was consumed by a manifestation of the void.");
                Main.player[i].Hurt(death, 5, 0, false, true, false, 5);
                Main.player[i].lifeRegen = 0;
            }
        }

        /// <summary>
        /// Purple color shield, weaken the player by 75%
        /// </summary>
        public void DebuffPurple()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                {
                    continue;
                }

                Main.player[i].allDamage /= 0.75f;
            }
        }

        #endregion

        public void PillarMovement()
        {
            movementTimer--;
            if (movementTimer == 0)
            {
                movementTimer = 100;
                movementUp = !movementUp;
            }

            if (movementUp)
            {
                npc.position.Y -= 0.2f;
            }
            else
            {
                npc.position.Y += 0.2f;
            }

        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte) shieldColor);
            writer.Write(movementTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            shieldColor = (PillarShieldColor) reader.ReadByte();
            movementTimer = reader.Read();
        }

        private Color GetShieldColor()
        {
            switch (shieldColor)
            {
                case PillarShieldColor.Black:
                    return Color.Black;
                case PillarShieldColor.Blue:
                    return Color.Blue;
                case PillarShieldColor.Green:
                    return Color.Green;
                case PillarShieldColor.Purple:
                    return Color.Purple;
                case PillarShieldColor.Red:
                    return Color.Red;
                default:
                    return Main.DiscoColor;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            Vector2 center = npc.Center - Main.screenPosition;
            DrawData drawingData = new DrawData(TextureManager.Load("Images/Misc/Perlin"), center - new Vector2(0, 10),  new Rectangle(0, 0, 600, 600), Main.DiscoColor, npc.rotation, new Vector2(300, 300), Vector2.One, SpriteEffects.None, 0);
            GameShaders.Misc["ForceField"].UseColor(Main.DiscoColor);
            GameShaders.Misc["ForceField"].Apply(drawingData);
            drawingData.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }

        

        private enum PillarShieldColor : byte
        {
            /// <summary>
            /// Red color shield, will drain health from the player directly
            /// </summary>
            Red = 0,
            /// <summary>
            /// Purple color shield, weaken the player by 75%
            /// </summary>
            Purple = 1,
            /// <summary>
            /// Black color shield, blind the player, making the fight harder, permanent night
            /// </summary>
            Black = 2,
            /// <summary>
            /// Green color shield, Will reflect damage done to it
            /// </summary>
            Green = 3,
            /// <summary>
            /// Blue color shield, -60% flight time and -50% movement speed
            /// </summary>
            Blue = 4,
            /// <summary>
            /// Will spawn random boss before plantera, final phase of the boss, still unsure
            /// </summary>
            Rainbow = 5
        }
    }
}
