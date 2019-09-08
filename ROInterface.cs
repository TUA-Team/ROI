using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ROI
{
	public interface ITreeHook
	{
		void PostDrawTreeTop(SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin);
	}

	public interface ICamerable
	{
		bool CurrentlyExecuting { get; set; }
		bool ScrollingEffect { get; }

		Vector2 GetCameraPosition();		
	}

	public interface IMobCamerable<T> where T : ModNPC, ICamerable
	{
		bool AnimationAI();
	}
}
