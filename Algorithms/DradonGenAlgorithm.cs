using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;

namespace Bubble_Defender.Algorithm
{
    public class DradonGenAlgorithm
    {
        public UnifiedRandom ran = new UnifiedRandom((int) DateTime.Now.Ticks);

        public List<Point> mainLineList;
        public Dictionary<Point, Point> BranchOffList;

        public int passAmount = 0;
        public int chanceForNextPass = 5;

        public int canvaWidth;
        public int canvaHeight;

        public Point startingPoint;
        public Point endingPoint;

        public int minDistanceX;
        public int minDistanceY;

        public int maxDistanceX;
        public int maxDistanceY;

        public bool invertedX = false;

        public DradonGenAlgorithm()
        {
            mainLineList = new List<Point>();
            BranchOffList = new Dictionary<Point, Point>();
        }

        public DradonGenAlgorithm SetMaxDistance(int x)
        {
            this.maxDistanceX = x;
            return this;
        }

        public DradonGenAlgorithm SetMinDistance(int x)
        {
            this.minDistanceX = x;
            return this;
        }

        public DradonGenAlgorithm SetMaxJump(int y)
        {
            this.maxDistanceY = y;
            return this;
        }

        public DradonGenAlgorithm SetMinJump(int y)
        {
            this.minDistanceY = y;
            return this;
        }

        public DradonGenAlgorithm SetStartingPoint(Point point)
        {
            this.startingPoint = point;
            return this;
        }

        public DradonGenAlgorithm SetCanvaWidth(int width)
        {
            this.canvaWidth = width;
            return this;
        }

        public DradonGenAlgorithm SetCanvaHeight(int height)
        {
            this.canvaHeight = height;
            return this;
        }

        /// <summary>
        /// Never use this, k thanks byw
        /// </summary>
        public void Run()
        {
            loop:
            goto loop;
        }

        public void GenerateMainBranch()
        {
            Point currentPoint = startingPoint;
            Point previousPoint;
            mainLineList.Add(startingPoint);
            while (InCanvasX(currentPoint.X) && InCanvasY(currentPoint.Y))
            {
                previousPoint = currentPoint;

                int nextPointMovementX = ran.Next(minDistanceX, maxDistanceX);
                int nextPointMovementY = ran.Next(minDistanceY, maxDistanceY);

                if (invertedX)
                {
                    nextPointMovementX *= -1;
                }

                currentPoint.X += nextPointMovementX;

                if (ran.Next(3) == 0)
                {
                    nextPointMovementY *= -1;
                    nextPointMovementY /= 4;
                }
                currentPoint.Y += nextPointMovementY / 4;

                if (!InCanvasX(currentPoint.X))
                {
                    currentPoint = previousPoint;
                    invertedX = !invertedX;
                    nextPointMovementY = ran.Next(minDistanceY, maxDistanceY);
                    currentPoint.Y += (int) (nextPointMovementY);
                    if (!InCanvas(currentPoint.X, currentPoint.Y))
                    {
                        endingPoint = previousPoint;
                        break;
                    }

                }
                mainLineList.Add(currentPoint);
            }
            mainLineList.Add(endingPoint);
            List<Vector2> vector = new List<Vector2>();
            foreach (Point point in mainLineList)
            {
                vector.Add(point.ToVector2());
            }
            GenerateBranchOff();
        }

        public void GenerateBranchOff()
        {
            foreach (var point in mainLineList)
            {
                if (point == startingPoint || point == endingPoint)
                {
                    continue;
                }
                if (ran.Next(2) == 0)
                {
                    Point previousBranchPoint = new Point();
                    bool loop = false;
                    branch:

                    Point potentialNewPointAddition = new Point((ran.Next(2) == 0) ? ran.Next(20, 25) : - ran.Next(20, 25),
                        (ran.Next(2) == 0) ? ran.Next(20, 25) : - ran.Next(20, 25));

                    Point branchPoint = new Point(point.X + potentialNewPointAddition.X, point.Y + potentialNewPointAddition.Y);

                    BranchOffList.Add((!loop) ? point : previousBranchPoint, branchPoint);
                    if (ran.Next(5) == 0)
                    {
                        loop = true;
                        previousBranchPoint = branchPoint;
                        goto branch;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BasicEffect effect = new BasicEffect(Main.graphics.GraphicsDevice);

            for (int i = 0; i < mainLineList.Count - 1; i++)
            {
                List<VertexPositionColor> vertices = new List<VertexPositionColor>();

                vertices.Add(new VertexPositionColor(new Vector3(ConvertToScreenCoordinates(mainLineList[i].ToVector2()), 0), Color.White));
                vertices.Add(new VertexPositionColor(new Vector3(ConvertToScreenCoordinates(mainLineList[i + 1].ToVector2()), 0), Color.White));
                VertexBuffer buffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), 2, BufferUsage.None);
            
                buffer.SetData<VertexPositionColor>(vertices.ToArray());

                //effect.Projection = projection;
                

                Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);
                Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                foreach (var effectTechnique in effect.CurrentTechnique.Passes)
                {
                    effectTechnique.Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
                }
            }

            foreach (Point point in BranchOffList.Keys)
            {
                List<VertexPositionColor> vertices = new List<VertexPositionColor>();

                vertices.Add(new VertexPositionColor(new Vector3(ConvertToScreenCoordinates(point.ToVector2()), 0), Color.White));
                vertices.Add(new VertexPositionColor(new Vector3(ConvertToScreenCoordinates(BranchOffList[point].ToVector2()), 0), Color.White));
                VertexBuffer buffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), 2, BufferUsage.None);
            
                buffer.SetData<VertexPositionColor>(vertices.ToArray());

                //effect.Projection = projection;
                

                Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);
                Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                foreach (var effectTechnique in effect.CurrentTechnique.Passes)
                {
                    effectTechnique.Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
                }
            }

            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, 2, 2);
            Color[] data = new Color[2* 2];
            for(int pixel=0;pixel<data.Length;pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = Color.Red;
            }

            //set the color
            texture.SetData(data);

            spriteBatch.Begin();

            foreach (Point point in mainLineList)
            {
                spriteBatch.Draw(texture, point.ToVector2(), Color.Black);
            }

            foreach (Point point in BranchOffList.Values)
            {
                spriteBatch.Draw(texture, point.ToVector2(), Color.Black);
            }

            spriteBatch.End();
        }

        private static Vector2 ConvertToScreenCoordinates(Vector2 conv)
        {  
            float outX = (conv.X) / (Main.graphics.GraphicsDevice.Viewport.Width / 2) - 1;
            float outY = (conv.Y) / (Main.graphics.GraphicsDevice.Viewport.Height / -2) + 1;

            return new Vector2(outX, outY);
        }

        public bool InCanvas(int x, int y)
        {
            
            return x >= 0 && y >= 0 && x <= canvaWidth && y <= canvaHeight;
        }

        public bool InCanvasX(int x)
        {
            return x >= 0 && x <= canvaWidth;
        }

        public bool InCanvasY(int y)
        {
            return  y >= 0 && y <= canvaHeight;
        }
    }
}
