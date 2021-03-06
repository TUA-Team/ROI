using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet.Contexts.Chains
{
    public class VerletChainContext : VerletContext<IVerletChain>
    {
        public VerletChainContext(string tex)
        {
            Tex = tex;
        }


        protected override void OnUpdate(IVerletChain body)
        {
            // Reset anchor
            // TODO: This assumes that chains will only be generated with the very first index as an anchor
            var anchor = body.Points[0];
            anchor.pos = anchor.old;
        }

        protected override void UpdateVertex(int index, Vector2 old, ref Vector2 pos)
        {
            // pos - old = velocity
            pos += pos - old;

            pos.Y += Gravity;

            for (int i = 0; i < Main.player.Length; i++)
            {
                var plr = Main.player[i];
                if (plr.active &&
                    plr.Hitbox.Contains(pos.ToPoint()))
                {
                    // Transfer some player velocity to the vine
                    pos += plr.velocity * CollisionMult;
                }
            }
        }


        public override void Draw(SpriteBatch sb, IVerletChain body)
        {
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

            for (int i = 0; i < body.Segments.Count; i++)
            {
                VerletSegment seg = body.Segments[i];

                var color = Lighting.GetColor((int)(seg.start.pos.X / 16f), (int)(seg.start.pos.Y / 16f));

                sb.Draw(ROIMod.Instance.GetTexture(Tex),
                    seg.start.pos - Main.screenPosition + zero,
                    body.DrawData[i].source,
                    color,
                    (seg.end.pos - seg.start.pos).ToRotation() + 1.57079632679f,//Math.PI * 0.5f,
                    body.DrawData[i].origin,
                    1,
                    SpriteEffects.None,
                    0);
            }
        }


        protected virtual string Tex { get; }
        protected virtual float Gravity { get; } = 0.3f;
        protected virtual float CollisionMult { get; } = 0.2f;


        public TagCompound SerializeData(IVerletChain body)
        {
            var tag = SerializeData(body, out var map);
            //tag.add(anchors, anchors.Select(x => map[x]).ToList()
            tag.Add(nameof(body.DrawData), body.DrawData);
            return tag;
        }

        public static new void BaseDeserialize(IVerletChain body, TagCompound tag)
        {
            VerletContext<IVerletChain>.BaseDeserialize(body, tag);

            /*var anchorBuffer = tag.GetList<int>(nameof(anchors));
            for (int i = 0; i < anchorBuffer.Count; i++)
            {
                ctx.anchors.Add(ctx.points[anchorBuffer[i]]);
            }*/

            body.DrawData = tag.GetList<ChainDrawData>(nameof(body.DrawData));
        }
    }
}
