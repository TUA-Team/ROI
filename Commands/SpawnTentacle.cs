using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.NPCs.Void;
using ROI.Worlds;
using ROI.Worlds.Structures.Wasteland;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Commands
{
    class SpawnTentacle : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.debug;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Vector2 position = caller.Player.position;
            Point16 positionInWorld = position.ToPoint16();

            ROIWorld.tenctacleList.Add(new Tentacle(position - new Vector2(0, 32f) * 2f, int.Parse(args[0]), new Vector2(float.Parse(args[1]), float.Parse(args[2])), 6, 6, 4f, 10f));

        }

        public override string Command => "tentacle";
        public override CommandType Type => CommandType.World;
    }
}
