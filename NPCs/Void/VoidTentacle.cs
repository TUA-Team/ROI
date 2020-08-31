using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL.Terraria.Achievements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ROI.NPCs.Void
{
    public class Tentacle
    {
            public Vector2 startingPosition;
            public Vector2 endPosition;

            public List<Vector2> positionList;
            public List<Vector2> previousPosList;

            public long leftOverTime;

            public float startingWidth;
            public float startingHeight;

            public float widthModifier;
            public float heightModifier;

            public Vector2 gravity;

            public Tentacle(Vector2 startingPosition, int height, Vector2 gravity, float startingWidth = 5, float startingHeight = 5, float widthModifier = 4, float heightModifier = 20)
            {
                this.startingPosition = startingPosition;
                this.widthModifier = widthModifier;
                this.heightModifier = heightModifier;

                this.leftOverTime = 0;

                this.gravity = gravity;

                this.positionList = new List<Vector2>();
                this.previousPosList = new List<Vector2>();
                InitPoint(height);
            }

            private void InitPoint(int height)
            {
                for (int i = 0; i < height; i++)
                {
                    positionList.Add(new Vector2(0f, -heightModifier * i) + startingPosition);
                    previousPosList.Add(new Vector2(0f, -heightModifier * i) + startingPosition);
                }

                endPosition = positionList[positionList.Count - 1];
            }

            public void Update(GameTime gameTime)
            {
                leftOverTime += gameTime.ElapsedGameTime.Ticks;
                long stepTicks = TimeSpan.TicksPerSecond / 60;

                while (leftOverTime >= stepTicks)
                {
                    leftOverTime -= stepTicks;

                    List<Vector2> tempPositionList = new List<Vector2>(positionList);

                    for (int i = 1; i < positionList.Count; i++)
                    {
                        float k = 100f;
                        float d = 7500f;
                        
                        //Get velocity for each point of the tentacle
                        Vector2 velocity = (positionList[i] - previousPosList[i]) / 60f;
                        Vector2 acceleration = new Vector2(0, 0);

                        Vector2 difference = positionList[i] - positionList[i - 1];

                        acceleration -= k * Vector2.Normalize(difference) * (difference.Length() - heightModifier);

                        if (i < positionList.Count - 1)
                        {
                            difference = positionList[i] - positionList[i + 1];

                            acceleration -= k * Vector2.Normalize(difference) * (difference.Length() - heightModifier);
                        }

                        acceleration += gravity;

                        acceleration -= d * velocity;
                            
                        positionList[i] = 2 * positionList[i] - previousPosList[i] + acceleration / 3600 / 2;
                    }

                    previousPosList = tempPositionList;

                }
            }

            public void Draw()
            {
                Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, 2, 2);
                Color[] data = new Color[2* 2];
                for(int pixel=0;pixel<data.Length;pixel++)
                {
                    //the function applies the color according to the specified pixel
                    data[pixel] = Color.Red;
                }

                //set the color
                texture.SetData(data);

                VertexBuffer buffer;

                BasicEffect effect;

                Matrix projection = Matrix.CreateOrthographic(20, 20, 0.001f, 100f);

                effect = new BasicEffect(Main.graphics.GraphicsDevice);

                List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();

                Vector2 normal = new Vector2(2f);

                Vector2 substractAmount = normal / positionList.Count;

                Vector2 normalAhead = normal - substractAmount;

                float j = widthModifier;
                float j2 = widthModifier;

                for (int i = 0; i < positionList.Count - 1; i++)
                {
                    j = widthModifier * (1f - (float) (i) / (positionList.Count - 1));
                    j2 = widthModifier * (1f - (float) (i + 1) / (positionList.Count - 1));


                    normal = Normalize(positionList, i);
                    normalAhead = Normalize(positionList, i + 1);

                    Vector2 firstUp = positionList[i] - normal * 5 * j;
                    Vector2 firstDown = positionList[i] + normal * 5 * j;
                    Vector2 secondUp = positionList[i + 1] - (normalAhead * 5 * j2);
                    Vector2 secondDown = positionList[i + 1] + (normalAhead * 5 * j2);

                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(firstDown), 0), new Vector2(0, 1)));
                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(secondDown), 0), new Vector2(1, 1)));
                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(firstUp), 0), new Vector2(0, 0)));

                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(firstUp), 0), new Vector2(0, 0)));
                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(secondDown), 0), new Vector2(1, 1)));
                    vertices.Add(new VertexPositionTexture(new Vector3(ConvertToScreenCoordinates(secondUp), 0), new Vector2(1, 0)));
                }

                buffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionTexture), vertices.Count, BufferUsage.WriteOnly);
                buffer.SetData<VertexPositionTexture>(vertices.ToArray());

                //effect.Projection = projection;
                effect.VertexColorEnabled = true;

                Main.graphics.GraphicsDevice.SetVertexBuffer(buffer);
                Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                foreach (EffectPass currentTechniquePass in effect.CurrentTechnique.Passes)
                {
                    currentTechniquePass.Apply();
                    
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertices.Count / 3);
                }

                foreach (Vector2 vector2 in positionList)
                {
                    Main.spriteBatch.Draw(texture, vector2, Color.Red);
                }
            }

            private static Vector2 Normalize(List<Vector2> vector, int index)
            {
                if (vector.Count == 0) return Vector2.One;

                if (vector.Count == 1) return Vector2.Normalize(Rotate90Degree(vector[0]));

                if (vector.Count == index + 1) return Vector2.Normalize(Rotate90Degree(vector[index]));

                return Vector2.Normalize(Rotate90Degree(vector[index] - vector[index + 1]));
            }

            private static Vector2 Rotate90Degree(Vector2 vector)
            {
                return new Vector2(-vector.Y, vector.X);
            }

            public struct Collision
            {
                private Rectangle[] collisionRectangle;
            }

            private static Vector2 ConvertToScreenCoordinates(Vector2 conv)
            {  
                float outX = (conv.X - Main.screenPosition.X) / (Main.screenWidth / 2) - 1;
                float outY = (conv.Y - Main.screenPosition.Y) / (Main.screenHeight / -2) + 1;

                return new Vector2(outX, outY);
            }
        }
    
}
