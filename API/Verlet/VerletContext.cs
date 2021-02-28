using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet
{
    public abstract class VerletContext : TagSerializable
    {
        protected IList<VerletPoint> points = new List<VerletPoint>();
        protected IList<VerletSegment> segments = new List<VerletSegment>();


        public void Update()
        {
            // Update all the points normally
            for (int i = 0; i < points.Count; i++)
            {
                var vertex = points[i];
                var newPos = UpdateVertex(i, vertex.old, vertex.pos);
                vertex.old = vertex.pos;
                vertex.pos = newPos;
            }

            for (int i = 0; i < UpdateCount; i++)
            {
                adjustSegments();
            }

            OnUpdate();

            // Squish all the segments to their proper lengths
            void adjustSegments()
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    var seg = segments[i];
                    var start = points[seg.start];
                    var end = points[seg.next];

                    var vec = end.pos - start.pos;
                    var len = (start.pos - end.pos).Length();

                    // Determine the percentage of the stick's current length that is unintended
                    var percent = (len - seg.size) / len;

                    // Since vec is the segment as an isolated vector, we can just multiply it by 
                    // the percent and remove the unintended length, but also divide it by two
                    // so that each vertex moves closer to the middle by half the extra length
                    start.pos += vec * percent * 0.5f;
                    end.pos -= vec * percent * 0.5f;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            for (int i = 0; i < segments.Count; i++)
            {
                VerletSegment seg = segments[i];
                DrawSegment(sb, i, points[seg.start].pos, points[seg.next].pos);
            }
            sb.End();
        }


        /// <summary>
        /// Use this to determine this new position of the vertex
        /// </summary>
        /// <param name="old"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected abstract Vector2 UpdateVertex(int index, Vector2 old, Vector2 pos);
        protected abstract void OnUpdate();
        protected abstract void DrawSegment(SpriteBatch sb, int index, Vector2 start, Vector2 end);

        protected virtual int UpdateCount => 2;


        protected float[] FillSizes(int len, float size)
        {
            var sizes = new float[len];
            for (int i = 0; i < len; i++)
            {
                sizes[i] = size;
            }
            return sizes;
        }

        /// <summary>
        /// Just like <see cref="AddSegments(Vector2[], (int start, int end)[], float[])"/>, but sticks will be automatically created in order
        /// </summary>
        /// <param name="vertices">The positions of the vertices</param>
        /// <param name="sizes">The length of each corresponding segment in <paramref name="sticks"/></param>
        protected void AddSegments(Vector2[] vertices, float[] sizes)
        {
            var sticks = new (int start, int next)[vertices.Length - 1];
            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i] = (i, i + 1);
            }
            AddSegments(vertices, sticks, sizes);
        }

        /// <summary>
        /// Create some segments
        /// </summary>
        /// <param name="vertices">The positions of the vertices</param>
        /// <param name="sticks">Indices in <paramref name="vertices"/> to use for the segments</param>
        /// <param name="sizes">The length of each corresponding segment in <paramref name="sticks"/></param>
        protected void AddSegments(Vector2[] vertices, (int start, int end)[] sticks, float[] sizes)
        {
            int firstIndex = points.Count;

            for (int i = 0; i < vertices.Length; i++)
            {
                var pos = vertices[i];
                points.Add(new VerletPoint(pos, pos));
            }

            for (int i = 0; i < sticks.Length; i++)
            {
                var (start, end) = sticks[i];
                segments.Add(new VerletSegment(start + firstIndex, end + firstIndex, sizes[i]));
            }
        }


        public virtual TagCompound SerializeData() => new TagCompound
        {
            [nameof(points)] = points,
            [nameof(segments)] = segments
        };

        protected static void BaseDeserialize(VerletContext ctx, TagCompound tag)
        {
            ctx.points = tag.GetList<VerletPoint>(nameof(points));
            ctx.segments = tag.GetList<VerletSegment>(nameof(segments));
        }
    }
}
