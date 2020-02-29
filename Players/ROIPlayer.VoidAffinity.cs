using ROI.Helpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        public int voidAffinity;
        public int maxVoidAffinity;

        public string[] buffType;
        public int[] buffTime;

        private void VAInit()
        {
            voidAffinity = 0;
            maxVoidAffinity = 50;

            buffType = new string[10];
            for (int i = 0; i < 10; i++)
            {
                buffType[i] = "nil"; 
            }
            buffTime = new int[10];
        }

        private TagCompound VASave()
        {
            return new TagCompound
            {
                [nameof(voidAffinity)] = voidAffinity,
                [nameof(maxVoidAffinity)] = maxVoidAffinity,

                [nameof(buffType)] = new List<string>(buffType),
                [nameof(buffTime)] = new List<int>(buffTime)
            };
        }

        private void VALoad(TagCompound tag)
        {
            voidAffinity = tag.GetInt(nameof(voidAffinity));
            maxVoidAffinity = tag.GetInt(nameof(maxVoidAffinity));

            buffType = new List<string>(tag.GetList<string>(nameof(buffType))).ToArray();
            buffTime = new List<int>(tag.GetList<int>(nameof(buffTime))).ToArray();
        }

        public void AddVoidBuff(string type, int time)
        {
            for (int i = 0; i < 10; i++)
            {
                if (buffTime[i] == 0)
                {
                    buffType[i] = type;
                    buffTime[i] = time;
                    return;
                }
            }
            Array.Copy(buffType, 0, buffType, 1, 9);
            buffType[0] = type;
            Array.Copy(buffTime, 0, buffTime, 1, 9);
            buffTime[0] = time;
        }

        //TODO: MP sync

        private void VAUpdate()
        {
            for (int i = 0; i < 10; i++)
            {
                if (buffTime[i] > 0)
                {
                    VoidBuffHelper.GetBuff(buffType[i]).Update(player);
                    buffTime[i]--;
                }
            }
        }
    }
}