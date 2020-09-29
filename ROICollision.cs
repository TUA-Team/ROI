using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;

namespace ROI
{
    static class ROICollision
    {

        public static Vector2 WallCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1) {
			Collision.up = false;
            Collision.down = false;
			Vector2 result = Velocity;
			Vector2 vector = Velocity;
			Vector2 vector2 = Position + Velocity;
			Vector2 vector3 = Position;
			int value = (int)(Position.X / 16f) - 1;
			int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int value3 = (int)(Position.Y / 16f) - 1;
			int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			int num5 = Utils.Clamp(value, 0, Main.maxTilesX - 1);
			value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
			value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
			value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
			float num6 = (value4 + 3) * 16;
			Vector2 vector4 = default(Vector2);
			for (int i = num5; i < value2; i++) {
				for (int j = value3; j < value4; j++) {
					// Check if Tile is null Or It's not active or 
					if (Main.tile[i, j] == null || Main.tile[i, j].wall != 0 || !Main.tile[i, j].active() || Main.tile[i, j].inActive() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
						continue;

					vector4.X = i * 16;
					vector4.Y = j * 16;
					int num7 = 16;
					if (Main.tile[i, j].halfBrick()) {
						vector4.Y += 8f;
						num7 -= 8;
					}

					if (!(vector2.X + (float)Width > vector4.X) || !(vector2.X < vector4.X + 16f) || !(vector2.Y + (float)Height > vector4.Y) || !(vector2.Y < vector4.Y + (float)num7))
						continue;

					bool flag = false;
					bool flag2 = false;
					if (Main.tile[i, j].slope() > 2) {
						if (Main.tile[i, j].slope() == 3 && vector3.Y + Math.Abs(Velocity.X) >= vector4.Y && vector3.X >= vector4.X)
							flag2 = true;

						if (Main.tile[i, j].slope() == 4 && vector3.Y + Math.Abs(Velocity.X) >= vector4.Y && vector3.X + (float)Width <= vector4.X + 16f)
							flag2 = true;
					}
					else if (Main.tile[i, j].slope() > 0) {
						flag = true;
						if (Main.tile[i, j].slope() == 1 && vector3.Y + (float)Height - Math.Abs(Velocity.X) <= vector4.Y + (float)num7 && vector3.X >= vector4.X)
							flag2 = true;

						if (Main.tile[i, j].slope() == 2 && vector3.Y + (float)Height - Math.Abs(Velocity.X) <= vector4.Y + (float)num7 && vector3.X + (float)Width <= vector4.X + 16f)
							flag2 = true;
					}

					if (flag2)
						continue;

					if (vector3.Y + (float)Height <= vector4.Y) {
                        Collision.down = true;
						if ((!(Main.tileSolidTop[Main.tile[i, j].type] && fallThrough) || !(Velocity.Y <= 1f || fall2)) && num6 > vector4.Y) {
							num3 = i;
							num4 = j;
							if (num7 < 16)
								num4++;

							if (num3 != num && !flag) {
								result.Y = vector4.Y - (vector3.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
								num6 = vector4.Y;
							}
						}
					}
					else if (vector3.X + (float)Width <= vector4.X && !Main.tileSolidTop[Main.tile[i, j].type]) {
						if (Main.tile[i - 1, j] == null)
							Main.tile[i - 1, j] = new Tile();

						if (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4) {
							num = i;
							num2 = j;
							if (num2 != num4)
								result.X = vector4.X - (vector3.X + (float)Width);

							if (num3 == num)
								result.Y = vector.Y;
						}
					}
					else if (vector3.X >= vector4.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type]) {
						if (Main.tile[i + 1, j] == null)
							Main.tile[i + 1, j] = new Tile();

						if (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3) {
							num = i;
							num2 = j;
							if (num2 != num4)
								result.X = vector4.X + 16f - vector3.X;

							if (num3 == num)
								result.Y = vector.Y;
						}
					}
					else if (vector3.Y >= vector4.Y + (float)num7 && !Main.tileSolidTop[Main.tile[i, j].type]) {
                        Collision.up = true;
						num3 = i;
						num4 = j;
						result.Y = vector4.Y + (float)num7 - vector3.Y + ((gravDir == 1) ? 0.01f : 0f);
						if (num4 == num2)
							result.X = vector.X;
					}
				}
			}

			return result;
		}

    }
}
