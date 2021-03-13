using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace ROI.Core.Verlet
{
    public abstract class VerletContext<TBody> where TBody : IVerletBody
    {
        public void Update(TBody body)
        {
            // Update all the points normally
            for (int i = 0; i < body.Points.Count; i++)
            {
                var vertex = body.Points[i];
                Vector2 old = vertex.pos;
                UpdateVertex(i, vertex.old, ref vertex.pos);
                vertex.old = old;
            }

            for (int i = 0; i < UpdateCount; i++)
            {
                adjustSegments();
            }

            OnUpdate(body);

            // Squish all the segments to their proper lengths
            void adjustSegments()
            {
                for (int i = 0; i < body.Segments.Count; i++)
                {
                    var seg = body.Segments[i];

                    var vec = seg.end.pos - seg.start.pos;
                    var len = (seg.start.pos - seg.end.pos).Length();

                    // Determine the percentage of the stick's current length that is unintended
                    var percent = (len - seg.size) / len;
                    seg.start.pos += vec * percent * 0.5f;
                    seg.end.pos -= vec * percent * 0.5f;
                }
            }
        }


        /// <summary>
        /// Use this to determine this new position of the vertex
        /// </summary>
        /// <param name="old"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected abstract void UpdateVertex(int index, Vector2 old, ref Vector2 pos);
        protected abstract void OnUpdate(TBody body);
        public abstract void Draw(SpriteBatch sb, TBody body);

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
        protected int AddBody(TBody body, Vector2[] vertices, float[] sizes)
        {
            var sticks = new (int start, int next)[vertices.Length - 1];
            for (int i = 0; i < sticks.Length; i++)
            {
                sticks[i] = (i, i + 1);
            }
            return AddBody(body, vertices, sticks, sizes);
        }

        /// <summary>
        /// Create some segments
        /// </summary>
        /// <param name="vertices">The positions of the vertices</param>
        /// <param name="sticks">Indices in <paramref name="vertices"/> to use for the segments</param>
        /// <param name="sizes">The length of each corresponding segment in <paramref name="sticks"/></param>
        /// <returns>The id of the first segment added</returns>
        protected int AddBody(TBody body, Vector2[] vertices, (int start, int end)[] sticks, float[] sizes)
        {
            int firstIndex = body.Points.Count;

            for (int i = 0; i < vertices.Length; i++)
            {
                var pos = vertices[i];
                body.Points.Add(new VerletPoint(pos, pos));
            }

            for (int i = 0; i < sticks.Length; i++)
            {
                var (start, end) = sticks[i];
                body.Segments.Add(new VerletSegment(body.Points[start + firstIndex], body.Points[end + firstIndex], sizes[i]));
            }

            return body.Segments.Count - sticks.Length;
        }


        // Since each segment will likely reference points that other segments also use,
        // a dictionary is created. The dictionary is also used since each segment doesn't
        // directly know the index of its consituent points.
        // The keys are the unique positions, the values are the index to each 
        // unique position. The index will then be used later at load. It uses Vector3 because it's
        // already included in the default serializers. This is all done to avoid whoAmI fields.

        protected TagCompound SerializeData(TBody body, out Dictionary<Vector2, int> map)
        {
            var tag = new TagCompound();
            map = new Dictionary<Vector2, int>();


            for (int i = 0; i < body.Segments.Count; i++)
            {
                var seg = body.Segments[i];
                if (!map.ContainsKey(seg.start.pos))
                {
                    map.Add(seg.start.pos, map.Count);
                }
                if (!map.ContainsKey(seg.end.pos))
                {
                    map.Add(seg.end.pos, map.Count);
                }
            }

            tag.Add(nameof(body.Points), map.Keys.ToList());


            var segBuffer = new List<Vector3>();

            for (int i = 0; i < body.Segments.Count; i++)
            {
                var seg = body.Segments[i];
                segBuffer.Add(new Vector3(map[seg.start.pos], map[seg.end.pos], seg.size));
            }

            tag.Add(nameof(body.Segments), segBuffer);


            return tag;
        }

        protected static void BaseDeserialize(TBody body, TagCompound tag)
        {
            var vertices = tag.GetList<Vector2>(nameof(body.Points));
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 vertex = vertices[i];
                body.Points.Add(new VerletPoint(vertex, vertex));
            }

            var sticks = tag.GetList<Vector3>(nameof(body.Segments));
            for (int i = 0; i < sticks.Count; i++)
            {
                var stick = sticks[i];
                body.Segments.Add(new VerletSegment(body.Points[(int)stick.X], body.Points[(int)stick.Y], stick.Z));
            }
        }
    }
}
