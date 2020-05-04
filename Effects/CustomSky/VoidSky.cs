using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Enums;
using ROI.NPCs.Void.VoidPillar;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ROI.Effects.CustomSky
{
	internal class VoidSky : Terraria.Graphics.Effects.CustomSky
	{
		/// <summary>
		/// You know the concept of shattering the universe by filling it with crack like if the reality was breaking?
		/// Welp here you have it
		/// </summary>
		private struct SkyCrack
		{
			internal static float glow = 0.5f;

			internal static Texture2D backTexture = ROIMod.instance.GetTexture("Textures/CustomSky/Void/SkyCrack");
			internal static Texture2D glowTexture = ROIMod.instance.GetTexture("Textures/CustomSky/Void/SkyCrackGlow");

			/// <summary>
			/// This one is really important, basically allow you to know if it's followed by another crack or not
			/// </summary>
			internal bool isAnEnd;

			internal float scaling;

			/// <summary>
			/// Coordinate stuff
			/// </summary>
			internal Vector2 position;
			internal Vector2 gotoPoint;

			/// <summary>
			/// Color of the crack in the sky
			/// </summary>
			internal Color crackColor;

			internal PillarShieldColor associatedShieldColor;



			public SkyCrack(Vector2 position, Vector2 gotoPoint, Color crackColor, PillarShieldColor associatedShieldColor)
			{
				this.isAnEnd = true;
				this.position = position;
				this.gotoPoint = position + gotoPoint;
				this.crackColor = crackColor;
				this.associatedShieldColor = associatedShieldColor;
				this.scaling = 0.2f;
			}

			public void Draw(SpriteBatch spriteBatch)
			{
				float r = (gotoPoint - position).ToRotation() - 1.57f;

				spriteBatch.Draw(backTexture, position,
					new Rectangle(0, 0, 8, 8), Color.White * 0.01f, Math.Abs(r),
					new Vector2(0, 0), new Vector2(scaling, Vector2.Distance(gotoPoint, position)), 0, 0);
				/*spriteBatch.Draw(glowTexture, position,
					new Rectangle(0, 0, 8, 8), Color.White * glow, Math.Abs(r),
					new Vector2(0, 0), new Vector2(Vector2.Distance(gotoPoint, position), scaling), 0, 0);*/
			}
		}

		private bool _IsActive;
		private float _fadeOpacity = 0f;
		private PillarShieldColor currentPillarShieldColor;

		private readonly Texture2D _voidSkyFirstLayer = ROIMod.instance.GetTexture("Textures/CustomSky/Void/SkyFirstLayer");

		private static List<SkyCrack> crackList;

		public override void Activate(Vector2 position, params object[] args)
		{
			_IsActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			_fadeOpacity = 0f;
			_IsActive = false;
		}

		public override void Update(GameTime gameTime)
		{
			if (_fadeOpacity <= 1f)
			{
				_fadeOpacity += 0.05f;
			}
			ModNPC npc = Main.npc.FirstOrDefault(i => i.modNPC is VoidPillar).modNPC;
			if (npc != null)
			{
				VoidPillar pillar = npc as VoidPillar;
				if (currentPillarShieldColor < pillar.ShieldColor)
				{
					currentPillarShieldColor = pillar.ShieldColor;
					for (int i = 0; i < crackList.Count; i++)
					{
						SkyCrack crack = crackList[i];
						crack.scaling += 0.2f;
						crackList[i] = crack;
					}

					GenerateCrack(false);
				}
			}

			SkyCrack.glow -= 0.005f;
			if (SkyCrack.glow < 0.1f)
			{
				SkyCrack.glow = 0.7f;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
			{
				//spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * this._fadeOpacity);
				spriteBatch.Draw(this._voidSkyFirstLayer, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * this._fadeOpacity));
			}

			foreach (SkyCrack skyCrack in crackList)
			{
				if (skyCrack.associatedShieldColor <= currentPillarShieldColor)
				{
					skyCrack.Draw(spriteBatch);
				}
			}
		}

		public override bool IsActive()
		{
			return _IsActive;
		}

		public override void Reset()
		{
			_fadeOpacity = 0f;
		}

		public override float GetCloudAlpha()
		{
			return 0.2f;
		}

		/// <summary>
		/// Method to generate the sky crack
		/// </summary>
		/// <param name="pillarAlreadyGenerated">Only useful upon entering world</param>
		public static void GenerateCrack(bool pillarAlreadyGenerated = false, bool wipe = false)
		{
			UnifiedRandom random = new UnifiedRandom();

			if (pillarAlreadyGenerated && crackList == null)
			{

				VoidPillar pillar = Main.npc.FirstOrDefault(i => i.modNPC is VoidPillar).modNPC as VoidPillar;
				byte phaseToGoTo = (byte)pillar.ShieldColor;
				crackList = new List<SkyCrack>();
				crackList.Clear();
				GenerateOriginalPoint(random);
				GenerateCrackOnWorldEntering(1, phaseToGoTo);
				return;
			}

			if (wipe)
			{
				crackList.Clear();
			}

			if (crackList.Count > 0 && pillarAlreadyGenerated)
			{
				return;
			}

			if (crackList.Count == 0)
			{
				GenerateOriginalPoint(random);
			}

			int length = crackList.Count;

			for (int i = 0; i < length; i++)
			{
				if (!crackList[i].isAnEnd)
				{
					continue;
				}

				SkyCrack crack = crackList[i];
				crack.isAnEnd = true;
				crackList[i] = crack;


				int quadrant = DetermineQuadrant(crack.gotoPoint - crack.position);
				int numberOfBranchOut = random.Next(1, 2);

				for (int numberOfBranch = 0; numberOfBranch < numberOfBranchOut; numberOfBranch++)
				{
					crackList.Add(GenerateNextCrack(random, crack.position, quadrant));
				}
			}
		}

		private static void GenerateOriginalPoint(UnifiedRandom random)
		{
			Vector2 startingPoint = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			for (int i = 0; i < 4; i++)
			{
				crackList.Add(GenerateNextCrack(random, startingPoint, i));
			}
		}

		private static SkyCrack GenerateNextCrack(UnifiedRandom random, Vector2 startingPoint, int quadrant)
		{
			switch (quadrant)
			{
				case 0:
					return new SkyCrack(startingPoint, new Vector2(random.NextFloat(75, 100), random.NextFloat(-75, -100)), Color.White, PillarShieldColor.Red);
					break;
				case 1:
					return new SkyCrack(startingPoint, new Vector2(random.NextFloat(75, 100), random.NextFloat(75, 100)), Color.White, PillarShieldColor.Red);
					break;
				case 2:
					return new SkyCrack(startingPoint, new Vector2(random.NextFloat(-100, -75), random.NextFloat(75, 100)), Color.White, PillarShieldColor.Red);
					break;
				default:
					return new SkyCrack(startingPoint, new Vector2(random.NextFloat(-100, -75), random.NextFloat(-100, -75)), Color.White, PillarShieldColor.Red);
					break;
			}
		}

		private static int DetermineQuadrant(Vector2 velocity)
		{
			if (velocity.X >= 1)
			{
				if (velocity.Y <= -1)
				{
					return 1;
				}
				else
				{
					return 2;
				}
			}
			else
			{
				if (velocity.Y <= -1)
				{
					return 4;
				}
				else
				{
					return 3;
				}
			}
			return -1;
		}

		private static void GenerateCrackOnWorldEntering(byte currentPhase, byte phaseToGoTo)
		{
			UnifiedRandom random = new UnifiedRandom();

			if (currentPhase < phaseToGoTo)
			{
				int length = crackList.Count;
				for (int i = 0; i < length; i++)
				{
					if (!crackList[i].isAnEnd)
					{
						continue;
					}

					SkyCrack crack = crackList[i];
					crack.isAnEnd = true;
					crack.scaling = 0.2f * currentPhase;
					crackList[i] = crack;


					int quadrant = DetermineQuadrant(crack.gotoPoint - crack.position);
					int numberOfBranchOut = random.Next(1, 2);

					for (int numberOfBranch = 0; numberOfBranch < numberOfBranchOut; numberOfBranch++)
					{
						crackList.Add(GenerateNextCrack(random, crack.position, quadrant));
					}
				}

				currentPhase++;
				GenerateCrackOnWorldEntering(currentPhase, phaseToGoTo);
			}
		}
	}
}
