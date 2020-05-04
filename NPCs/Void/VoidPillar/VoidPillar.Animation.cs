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
		private bool _firstDialog = false;
	    private bool _secondDialog = false;
	    private bool _finalDialog = false;

	    private int _animationTimeLeft = 0;
	    private float opacityProgress = 0f;

		//private int AnimationTimer =  
		public bool CurrentlyExecuting { get; set; } = true;

		public bool ScrollingEffect => true;

		public Vector2 GetCameraPosition()
		{
			return new Vector2(npc.Center.X - Main.screenWidth / 2, npc.Center.Y - Main.screenHeight / 2);
		}

		public bool AnimationAI()
		{
			if (CurrentlyExecuting)
			{
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = npc.position;
				dust = Main.dust[Terraria.Dust.NewDust(position, npc.width, npc.height, 27, 0f, 0f, 0, new Color(255, 255, 255), 2.434211f)];
				dust.noLight = true;

				_animationTimeLeft++;
				npc.Opacity = (float)(_animationTimeLeft / 600f);
				if (!_firstDialog)
				{
					Main.NewText("<codex> A player as breached the void, the original pillar has finally broke into reality. Be ready to fight it", Color.Black);
					_firstDialog = true;
				}

				if (!_secondDialog && _animationTimeLeft == 300)
				{
					Main.NewText("<codex> The void pillar, a weapon that became a living entity, linked to someone, if the pillar die, the person will get massively debuffed for the next couple of minute", Color.Black);
					_secondDialog = true;
				}

				if (!_finalDialog && _animationTimeLeft == 600)
				{
					Main.NewText("<codex> On a final note, be aware of the heart beat, it can kill anything that get hit by the shockwave", Color.Black);
					_finalDialog = true;
					CurrentlyExecuting = false;
				}
				return true;
			}
			return false;
		}
	}
}
