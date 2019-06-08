using Terraria;

namespace ROI
{
    static class ROIExtension
    {
        public static void SetAllTrue(this bool[] self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                self[i] = true;
            }
        }

        public static void SetAllFalse(this bool[] self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                self[i] = false;
            }
        }

        public static void Kill(this NPC npc)
        {
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
        }
    }
}
