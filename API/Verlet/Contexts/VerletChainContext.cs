using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet.Contexts
{
    public abstract class VerletChainContext : VerletContext
    {
        protected IList<int> anchors = new List<int>();
        protected IList<ChainDrawData> drawData = new List<ChainDrawData>();


        protected override void OnUpdate()
        {
            // Reset anchors
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = points[anchors[i]];
                anchor.pos = anchor.old;
            }
        }

        protected override Vector2 UpdateVertex(int index, Vector2 old, Vector2 pos)
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

            return pos;
        }


        protected override void DrawSegment(SpriteBatch sb, int index, Vector2 start, Vector2 end)
        {
            // TODO: figure out how to cache tex without incurring memory leaks
            // TODO: learn how to interpolate the colors
            var color = Lighting.GetColor((int)(start.X / 16f), (int)(start.Y / 16f));
            sb.Draw(ROIMod.Instance.GetTexture(Tex),
                start - Main.screenPosition,
                drawData[index].source,
                color,
                (end - start).ToRotation() + 1.57079632679f,//Math.PI * 0.5f,
                drawData[index].origin,
                1,
                SpriteEffects.None,
                0);
        }


        public abstract string Tex { get; }
        public virtual float Gravity => 0.3f;
        public virtual float CollisionMult => 0.04f;


        public override TagCompound SerializeData()
        {
            var tag = base.SerializeData();
            tag.Add(nameof(Tex), Tex);
            tag.Add(nameof(Gravity), Gravity);
            tag.Add(nameof(CollisionMult), CollisionMult);
            tag.Add(nameof(anchors), anchors);
            tag.Add(nameof(drawData), drawData);
            return tag;
        }

        protected static void BaseDeserialize(VerletChainContext ctx, TagCompound tag)
        {
            VerletContext.BaseDeserialize(ctx, tag);
            ctx.anchors = tag.GetList<int>(nameof(anchors));
            ctx.drawData = tag.GetList<ChainDrawData>(nameof(drawData));
        }
    }
}
