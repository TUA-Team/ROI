﻿
using ROIWorld = ROI.Worlds.ROIWorld;
#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Worlds;
using ROI.Worlds.Structures.Wasteland;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace ROI.Commands
{
    class GenerateStructure : ModCommand
    {
        public override bool Autoload(ref string name)
        {
            return ROIMod.debug;
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Vector2 position = caller.Player.position;
            Point16 positionInWorld = position.ToPoint16();
            switch (args[0].ToLower())
            {
                case "wastelandgrotto" :
                    WastelandGrotto.Generate(positionInWorld.X, positionInWorld.Y);
                    break;
                case "wasteland" :
                    ROIWorld instance = (ROIWorld) mod.GetModWorld("ROIWorld");
                    instance.WastelandGeneration(new GenerationProgress());
                    break;
                case "wastelake" :
                    WastelandLake.Generate(position);
                    break;
            }
        }

        public override string Command => "genStruct";
        public override CommandType Type => CommandType.World;
    }
}
#endif
