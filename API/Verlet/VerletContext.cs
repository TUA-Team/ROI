using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet
{
    public abstract class VerletContext
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

                    var vec = seg.end.pos - seg.start.pos;
                    var len = (seg.start.pos - seg.end.pos).Length();

                    // Determine the percentage of the stick's current length that is unintended
                    var percent = (len - seg.size) / len;
                    seg.start.pos += vec * percent * 0.5f;
                    seg.end.pos -= vec * percent * 0.5f;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            for (int i = 0; i < segments.Count; i++)
            {
                VerletSegment seg = segments[i];
                DrawSegment(sb, i, seg.start.pos, seg.end.pos);
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
        /// <returns>The id of the first segment added</returns>
        protected int AddBody(Vector2[] vertices, float[] sizes)
        {
            var sticks = new (int start, int next)[vertices.Length - 1];
            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i] = (i, i + 1);
            }
            return AddBody(vertices, sticks, sizes);
        }

        /// <summary>
        /// Create some segments
        /// </summary>
        /// <param name="vertices">The positions of the vertices</param>
        /// <param name="sticks">Indices in <paramref name="vertices"/> to use for the segments</param>
        /// <param name="sizes">The length of each corresponding segment in <paramref name="sticks"/></param>
        /// <returns>The id of the first segment added</returns>
        protected int AddBody(Vector2[] vertices, (int start, int end)[] sticks, float[] sizes)
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
                segments.Add(new VerletSegment(points[start + firstIndex], points[end + firstIndex], sizes[i]));
            }

            return segments.Count - sticks.Length;
        }


        // Since each segment will likely reference points that other segments also use,
        // a dictionary is created. The dictionary is also used since each segment doesn't
        // directly know the index of its consituent points.
        // The keys are the unique positions, the values are the index to each 
        // unique position. The index will then be used later at load. It uses Vector3 because it's
        // already included in the default serializers.

        protected TagCompound SerializeData(out Dictionary<Vector2, int> map)
        {
            var tag = new TagCompound();
            map = new Dictionary<Vector2, int>();


            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                if (!map.ContainsKey(seg.start.pos))
                {
                    map.Add(seg.start.pos, map.Count);
                }
                if (!map.ContainsKey(seg.end.pos))
                {
                    map.Add(seg.end.pos, map.Count);
                }
            }

            tag.Add(nameof(points), map.Keys.ToList());


            var segBuffer = new List<Vector3>();

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                segBuffer.Add(new Vector3(map[seg.start.pos], map[seg.end.pos], seg.size));
            }

            tag.Add(nameof(segments), segBuffer);


            return tag;
        }

        protected static void BaseDeserialize(VerletContext ctx, TagCompound tag)
        {
            var vertices = tag.GetList<Vector2>(nameof(points));
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 vertex = vertices[i];
                ctx.points.Add(new VerletPoint(vertex, vertex));
            }

            var sticks = tag.GetList<Vector3>(nameof(segments));
            for (int i = 0; i < sticks.Count; i++)
            {
                var stick = sticks[i];
                ctx.segments.Add(new VerletSegment(ctx.points[(int)stick.X], ctx.points[(int)stick.Y], stick.Z));
            }
        }
    }
}
