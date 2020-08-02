using Microsoft.Xna.Framework;
using ROI.Enums;
using ROI.NPCs.Interfaces;
using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Void.VoidPillar
{
    internal sealed partial class VoidPillar : ModNPC, ISavableEntity, ICamerable, IMobCamerable<VoidPillar>
    {
        private float _standardMovementSpeed = 0.5f;

        private int _previousTeleportationTimer;
        private int _teleportationTimer = 20 * 60;
        private int _teleportationOrb = 0;
        private int[] _teleportationOrbID = new int[2];


        private void MovementAI()
        {
            ResetMovementVariable();
            switch (MovementAIPhase)
            {
                case 0:
                    StandardPillarMovement();
                    break;
                case 1:
                    StandardPillarMovement();
                    TeleportationPhase();
                    break;
            }
        }

        private void StandardPillarMovement()
        {
            if (extraAI[0] == 0)
            {
                npc.velocity.Y += 0.002f;
                if (npc.velocity.Y > .1f)
                {
                    extraAI[0] = 1f;
                    npc.netUpdate = true;
                }
            }
            else
            if (extraAI[0] == 1)
            {
                npc.velocity.Y -= 0.002f;
                if (npc.velocity.Y < -.1f)
                {
                    extraAI[0] = 0f;
                    npc.netUpdate = true;
                }
            }
        }

        //in this movement phase, the pillar will act as a rocket and will also shoot stuff, this phase will start at the purple shield and will have a chance to happen in any phase
        //do note that during this phase it might shoot laser or black hole
        private void OffensiveMovement()
        {

        }

        //This phase can happen at any time, during this phase, pillar wil only take 10% of usual damage.
        //Once the teleportation is done, a massive shockwave will be released, dealing 100 damage to everyone affected by it
        //I guess this might be more suitable in VoidPillar.Attack?
        private void TeleportationPhase()
        {
            _teleportationTimer--;
            ChangeDamageReduction();
            npc.velocity = Vector2.Zero;

            if (_teleportationTimer != 0)
            {
                _isCurrentlyTeleporting = true;
            }
            _previousTeleportationTimer = _teleportationTimer;
            if (_teleportationTimer <= 0 && _teleportationRingScale < 0f)
            {
                npc.netUpdate = true;
                _isCurrentlyTeleporting = false;
                npc.position = new Vector2(Main.rand.Next((int)(npc.position.X / 16 - 50), (int)(npc.position.X / 16 + 50)), Main.rand.Next((int)(npc.position.Y / 16 - 5), (int)(npc.position.Y / 16 + 5))) * 16;
                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("VoidTeleportationShockwave"), 100, 1f, Main.myPlayer);
                if (ShieldColor == PillarShieldColor.Red)
                {
                    MovementAIPhase = 0f;
                }
            }
        }



        //during this phase, the pillar will literally be a teleporting cannon, shooting laser at the player that will inflict 50 raw damnage to the player.
        //Will shoot 4-5 time, then go to normal offensive movement
        private void TeleportingCannon()
        {

        }


        private void ResetMovementVariable()
        {
            if (npc.ai[0] != 1f)
            {
                _teleportationRingScale = 0f;
                _isCurrentlyTeleporting = false;
                _teleportationOrb = 0;
                _teleportationOrbID = new int[2];
                _teleportationTimer = 15 * 60;
            }
        }

        internal enum MovementPhase : byte
        {
            RegularPillar = 0,
            Teleportation = 1,
            Offensive = 2,
            TeleportingCannon = 3,
            RotatingCannon = 4,
            OffensiveCannon = 5,
            Summoning = 6
        }
    }
}
