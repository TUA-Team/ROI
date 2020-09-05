using Microsoft.Xna.Framework;
using ROI.Models;
using ROI.Models.Enums;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.NPCs.Bosses.VoidPillar
{
    /// <summary>
    /// npc.ai[0] = Movement AI Phase
    /// </summary>
    public partial class VoidPillar : ModNPC, ISaveableEntity, ICamerable, IMobCamerable<VoidPillar>
    {
        public override string Texture => "Terraria/NPC_507";

        private int movementTimer;
        private bool _movementUp;
        private float _damageReduction;
        private Vector2 _originalPosition;

        private bool _isCurrentlyTeleporting = false;

        public PillarShieldColor ShieldColor { get; internal set; }

        public int ShieldHealth { get; internal set; }

        public float[] extraAI = new float[1];

        private float MovementAIPhase
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float MovementPillarState
        {
            get => extraAI[0];
            set => extraAI[0] = value;
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Void Pillar");
            NPCID.Sets.TrailingMode[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.boss = true;
            npc.width = 174;
            npc.height = 364;
            npc.aiStyle = -1;
            for (int i = 0; i < npc.buffImmune.Length; i++) npc.buffImmune[i] = true;
            npc.lifeMax = (Main.expertMode) ? 25000 : 20000;
            music = MusicID.LunarBoss;
            npc.noGravity = true;
            npc.immortal = true;
            npc.knockBackResist = 0f;

            ShieldColor = PillarShieldColor.Red;
            ShieldHealth = (Main.expertMode) ? 25000 : 20000;

            movementTimer = 100;
            _movementUp = false;
            _damageReduction = (Main.expertMode) ? 0.2f : 0; //Set red shield damage reduction here
            if (Main.npc.Any(i => i.modNPC is VoidPillar)) {
                npc.active = false;
            }
            _originalPosition = npc.position;
        }

        public override void AI() {
            MovementAI();
            StandardPillarMovement();
            if (AnimationAI()) {
                return;
            }

            if (MovementAIPhase != 1f && Main.netMode != NetmodeID.MultiplayerClient) {
                Shockwave();
            }

            if (MovementAIPhase == 0f) {
                if (Main.rand.Next(1000) == 0 && ShieldColor == PillarShieldColor.none) {
                    MovementAIPhase = 1f;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(ShieldHealth);
            writer.Write((byte)ShieldColor);
            writer.Write(movementTimer);
            writer.Write(_shockwaveTimer);
            writer.Write(_animationTimeLeft);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            ShieldHealth = reader.ReadInt32();
            ShieldColor = (PillarShieldColor)reader.ReadByte();
            movementTimer = reader.Read();
            _shockwaveTimer = reader.ReadInt32();
            _animationTimeLeft = reader.ReadInt32();
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) {
            DamageShield(damage);

            if (ShieldColor == PillarShieldColor.Green) {
                string gp1 = player.Male ? "himself" : "herself";
                string gp2 = player.Male ? "his" : "her";
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(
                    Main.rand.Next(new string[] {string.Format("{0} impaled {1}", player.name, gp1),
                    string.Format("{0} swung {1} sword and the sword hit them back", player.name, gp2) }));
                player.Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) {
            DamageShield(damage);

            if (ShieldColor == PillarShieldColor.Green) {
                var plr = Main.player[projectile.owner];
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(
                    string.Format("{0} threw something and was unlucky enough that it came back", plr));
                plr.Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
            if (ShieldColor == PillarShieldColor.none) {
                return true;
            }

            return false;
        }

        public override bool CheckActive() {
            return false;
        }

        private void DamageShield(int damage) {
            ShieldHealth -= damage - (int)(damage * _damageReduction);
            npc.life -= damage - (int)(damage * _damageReduction);

            if (ShieldHealth <= 0 && ShieldColor != PillarShieldColor.Rainbow) {
                ShieldHealth = (Main.expertMode) ? 25000 : 20000;
                npc.life = (Main.expertMode) ? 25000 : 20000;
                SwitchShieldColor();
            }
        }

        private void SwitchShieldColor() {
            npc.netUpdate = true;
            switch (ShieldColor) {
                case PillarShieldColor.Red:
                    ShieldColor = PillarShieldColor.Purple;
                    break;
                case PillarShieldColor.Purple:
                    ShieldColor = PillarShieldColor.Black;
                    break;
                case PillarShieldColor.Black:
                    ShieldColor = PillarShieldColor.Green;
                    break;
                case PillarShieldColor.Green:
                    ShieldColor = PillarShieldColor.Blue;
                    break;
                case PillarShieldColor.Blue:
                    ShieldColor = PillarShieldColor.Rainbow;
                    npc.immortal = false;
                    break;
            }
            ChangeDamageReduction();
        }

        private void ChangeDamageReduction() {
            npc.netUpdate = true;
            if (_isCurrentlyTeleporting) {
                _damageReduction = 0.95f;
                return;
            }
            switch (ShieldColor) {
                case PillarShieldColor.Red:
                    _damageReduction = 0.2f;
                    break;
                case PillarShieldColor.Purple:
                    _damageReduction = 0.9f;
                    break;
                case PillarShieldColor.Black:
                    _damageReduction = 0.2f;
                    break;
                case PillarShieldColor.Green:
                    _damageReduction = 0.5f;
                    break;
                case PillarShieldColor.Blue:
                    _damageReduction = 0.8f;
                    break;
            }

        }

        public Color GetShieldColor() {
            switch (ShieldColor) {
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

        public override bool CheckDead() {
            if (ShieldColor == PillarShieldColor.none) {
                return true;
            }

            return false;
        }


        public void Save(TagCompound tag) {
            tag.Add(nameof(ShieldColor), (byte)ShieldColor);
            tag.Add(nameof(ShieldHealth), ShieldHealth);
            //tag.Add(nameof(_originalPosition), _originalPosition);
            tag.Add(nameof(MovementAIPhase), MovementAIPhase);
        }

        public void Load(TagCompound data) {
            ShieldColor = (PillarShieldColor)data.GetByte(nameof(ShieldColor));
            ShieldHealth = data.GetAsInt(nameof(ShieldHealth));
            MovementAIPhase = data.GetFloat(nameof(MovementAIPhase));

            // VoidSky.GenerateCrack(true);
        }

        public bool SaveHP => true;
    }
}
