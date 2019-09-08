using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.NPCs.Interfaces;
using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Void.VoidPillar
{
	internal sealed partial class VoidPillar : ModNPC, ISavableEntity, ICamerable, IMobCamerable<VoidPillar>
	{
		private float _standardMovementSpeed = 0.5f;

		private void MovementAI()
		{
			switch (MovementAIPhase)
			{
				case 0:
					StandardPillarMovement();
					break;
			}
		}

		private void StandardPillarMovement()
		{
			movementTimer--;
			if (movementTimer == 0)
			{
				movementTimer = 275;
				_movementUp = !_movementUp;
				npc.netUpdate = true;
				_standardMovementSpeed = 0.21f;
			}

			if (_movementUp)
			{
				npc.position.Y -= _standardMovementSpeed;
			}
			else
			{
				npc.position.Y += _standardMovementSpeed;
			}

			_standardMovementSpeed -= 0.0005f;
		}

		//in this mouvement phase, the pillar will act as a roch and will also shoot stuff, this phase will start at the purple shield and will have a chance to happen in any phase
		//do note that during this phase it might shoot laser or black hole
		private void OffensiveMovement()
		{

		}

		//This phase can happen at any time, during this phase, pillar wil only take 10% of usual damage.
		//Once the teleportation is done, a massive shockwave will be released, dealing 100 damage to everyone affected by it
		//I guess this might be more suitable in VoidPillar.Attack?
		private void TeleportationPhase()
		{
			npc.velocity = Vector2.Zero;
		}

		//during this phase, the pillar will literally be a teleporting cannon, shooting laser at the player that will inflict 50 raw damnage to the player.
		//Will shoot 4-5 time, then go to normal offensive movement
		private void TeleportingCannon()
		{

		}	
	}
}
