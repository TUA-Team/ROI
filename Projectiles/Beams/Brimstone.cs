using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;

namespace ROI.Projectiles.Beams
{
    /// <summary>
    /// ai[0] = dunno
    /// ai[1] = angle 
    /// </summary>
    class Brimstone : BaseCurvingBeam
    {
        public bool turn;

        public Vector2 middlePoint;
        public Vector2 finalPoint;
        public float _angle;

        public override void SetDefaults()
        {
            base.SetDefaults();

        }

        public override void AI()
        {

            ModifyBeamVelocity(_angle);
            /*
			switch (projectile.ai[0])
			{
				case 0f:
					newC = projectile.position + new Vector2(20 * 16, 50 * 16);
					curve[0][1] = curve[0][1] + new Vector2(0, 50);
					if (curve[0][1].Y >= newC.Y)
					{
						projectile.ai[0] = 1f;
					}
					break;
				case 1f:
					newC = projectile.position + new Vector2(20 * 16, - (50 * 16));
					curve[0][1] = curve[0][1] + new Vector2(0, -50);
					if (curve[0][1].Y <= newC.Y)
					{
						projectile.ai[0] = 0f;
					}
					break;
			}*/
        }

        internal void ModifyBeamVelocity(double angleInDegree)
        {
            int count = 180;

            //set start angle to base angle
            float currentAngle = projectile.ai[1];
            float perCurve = MathHelper.TwoPi / count;

            //parameters for you to change
            float length = 900f;
            float tangentLength = 900f;

            currentAngle += perCurve;

            if (currentAngle > MathHelper.TwoPi)
            {
                currentAngle = 0;
            }


            Vector2 perp = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
            Vector2 tang = new Vector2(perp.Y, -perp.X);


            curve = new BaseCurvingBeam.BezierCurve(
                projectile.position,
                projectile.position + perp * length * 0.75f,
                projectile.position + perp * length + tang * tangentLength * 0.5f,
                projectile.position + perp * length + tang * tangentLength);
            projectile.ai[1] = currentAngle;

        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(middlePoint);
            writer.WriteVector2(finalPoint);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            middlePoint = reader.ReadVector2();
            finalPoint = reader.ReadVector2();
        }
    }
}
