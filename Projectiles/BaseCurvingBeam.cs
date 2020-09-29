using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Projectiles
{
    /// <summary>
    /// Reference from this http://www.paradeofrain.com/2007/10/04/continuous-2d-ribbons-in-xna/
    /// Remember to remake it entirely @webmillio
    /// </summary>
    internal class BaseCurvingBeam : ModProjectile
    {
        private readonly int whoAmILinkedTo = -1;

        public sealed override string Texture => "ROI/Projectiles/BlankProjectile";

        public override bool CloneNewInstances => false;

        public BezierCurve curve;

        public override void SetDefaults()
        {
            projectile.damage = 0;
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 2000;
        }

        private bool CheckBeamCollision()
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (curve == null)
            {
                return false;
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            List<Vector2> pointList = curve.GetPoints(1000);
            for (int i = 1; i < pointList.Count; i++)
            {
                float deltaX = pointList[i].X - pointList[i - 1].X;
                float deltaY = pointList[i].Y - pointList[i - 1].Y;
                float angle = (float)Math.Atan2(deltaY, deltaX);
                Vector2 position = pointList[i - 1] - Main.screenPosition;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                spriteBatch.Draw(ModContent.GetTexture("ROI/Projectiles/BrimstoneProjectile"), position, new Rectangle((int)-distance, 0, (int)(distance * 1.2f), 40), /*new Color(64, 12, 68)*/Main.DiscoColor, angle, new Vector2(0, 0), Vector2.One, SpriteEffects.None, 0f);
            }


            spriteBatch.End();

            spriteBatch.Begin();
            return true;
        }

        public override void Kill(int timeLeft)
        {

        }

        public class BezierCurve
        {
            private readonly Vector2[] _controlPoints;

            public BezierCurve(params Vector2[] controls)
            {
                _controlPoints = controls;
            }

            /// <summary>
            /// Return a Vector2 at value T along the bezier curve.
            /// </summary>
            /// <param name="T">How far along the bezier curve to return a point.</param>
            /// <returns></returns>
            public Vector2 Evaluate(float T)
            {
                if (T < 0f)
                {
                    T = 0f;
                }

                if (T > 1f)
                {
                    T = 1f;
                }

                return PrivateEvaluate(_controlPoints, T);
            }

            /// <summary>
            /// Get a list of points along the bezier curve. Must be at least 2.
            /// </summary>
            /// <param name="amount">The amount of points to get.</param>
            /// <returns>A list of Vector2s representing the points.</returns>
            public List<Vector2> GetPoints(int amount)
            {
                if (amount < 2)
                {
                    throw new ArgumentException("How am I supposed to get one (or, heck, less) point on a bezier curve? You need to have more than two points specified for the number!");
                }

                float perStep = 1f / (amount - 1);

                List<Vector2> points = new List<Vector2>();

                for (float step = 0f; step <= 1f; step += perStep)
                {
                    points.Add(Evaluate(step));
                }

                return points;
            }

            private Vector2 PrivateEvaluate(Vector2[] points, float T)
            {
                if (points.Length > 2)
                {
                    Vector2[] nextPoints = new Vector2[points.Length - 1];
                    for (int k = 0; k < points.Length - 1; k++)
                    {
                        nextPoints[k] = Vector2.Lerp(points[k], points[k + 1], T);
                    }
                    return PrivateEvaluate(nextPoints, T);
                }
                else
                {
                    return Vector2.Lerp(points[0], points[1], T);
                }
            }

            public Vector2 this[int x]
            {
                get
                {
                    return _controlPoints[x];
                }
                set
                {
                    _controlPoints[x] = value;
                }
            }
        }
    }
}
