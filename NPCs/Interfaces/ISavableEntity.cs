using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.NPCs.Interfaces
{
    interface ISavableEntity
    {
        /// <summary>
        /// Basically allow to save custom to ModNPC
        /// </summary>
        /// <returns>Return the data you want to be saved</returns>
        TagCompound Save();
        /// <summary>
        /// Load your custom NPC data from here
        /// </summary>
        /// <param name="data"></param>
        void Load(TagCompound data);
        /// <summary>
        /// Decide if you wanna save ModNPC health, could be useful for the final boss?
        /// </summary>
        bool SaveHP { get; }
    }
}
