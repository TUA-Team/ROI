using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace ROI.Worlds.Structures.Wasteland
{
    /// <summary>
    /// Welcome to the Terraria cave update, oh wait, wrong game :/
    /// </summary>
    abstract class Wasteland_Cave : TagSerializable
    {
        public int x, y, width, height;

        public static readonly Func<TagCompound, Wasteland_Cave> DESERIALIZER = Load;

        public abstract string caveTypeName { get; }

        public Rectangle CaveBound => new Rectangle(x, y, width, height);

        public abstract void Generate(GenerationProgress progress);

        public Wasteland_Cave(Rectangle rectangle)
        {
            this.x = rectangle.X;
            this.y = rectangle.Y;
            this.width = rectangle.Width;
            this.height = rectangle.Height;
        }

        /// <summary>
        /// Ran only on client side, allow fancy visual effect
        /// </summary>
        public virtual void ClientSideVisualEffect(Player player)
        {

        }

        //This is only synced once, which is upon world entering
        public void NetSyncSend(BinaryWriter writer)
        {
            if(Main.netMode != NetmodeID.Server)
                return;

            writer.Write(x);
            writer.Write(y);
            writer.Write(width);
            writer.Write(height);
        } 

        //This is only synced once, which is upon world entering
        public void NetSyncReceive(BinaryReader reader)
        {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            width = reader.ReadInt32();
            height = reader.ReadInt32();
        }

        public TagCompound SerializeData()
        {
            return new TagCompound()
            {
                ["bound"] = CaveBound,
                ["caveType"] = caveTypeName
            };
        }

        public static Wasteland_Cave Load(TagCompound tag)
        {
            string caveType = tag.GetString("caveType");
            switch (caveType)
            {
                case "Lost_Wood":
                    return new Wasteland_Lost_Cave(tag.Get<Rectangle>("bound"));
            }
            throw new Exception("Invalid cave type");
        }
    }
}
