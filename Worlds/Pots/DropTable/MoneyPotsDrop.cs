using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROI.Worlds.Pots.DropTable
{
    public delegate void ModifyMoneyAmount(int x, int y, ref float money);
    sealed class MoneyPotsDrop : PotsDrop
    {
        public event ModifyMoneyAmount modifyAmount;
        
        public MoneyPotsDrop(Func<int, int, bool> successFunc) : base("Money", successFunc)
        {
            modifyAmount += delegate (int x, int y, ref float money)
            {
                if ((double)y < Main.worldSurface) { money *= 0.5f; }
                else if ((double)y < Main.rockLayer) { money *= 0.75f; }
                else if (y > Main.maxTilesY - 250) { money *= 1.25f; }
            };

            modifyAmount += (int x, int y, ref float money) => { money *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f; };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.rand.Next(4) == 0)
                {
                    money *= 1f + (float)Main.rand.Next(5, 11) * 0.01f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.rand.Next(8) == 0)
                {
                    money *= 1f + (float)Main.rand.Next(10, 21) * 0.01f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.rand.Next(12) == 0)
                {
                    money *= 1f + (float)Main.rand.Next(20, 41) * 0.01f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.rand.Next(16) == 0)
                {
                    money *= 1f + (float)Main.rand.Next(40, 81) * 0.01f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.rand.Next(20) == 0)
                {
                    money *= 1f + (float)Main.rand.Next(50, 101) * 0.01f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.expertMode)
                {
                    money *= 2.5f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.expertMode && Main.rand.Next(2) == 0)
                {
                    money *= 1.25f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.expertMode && Main.rand.Next(3) == 0)
                {
                    money *= 1.5f;
                }
            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (Main.expertMode && Main.rand.Next(4) == 0)
                {
                    money *= 1.75f;
                }
            };
            //Pots drop modifier
            modifyAmount += (int x, int y, ref float money) =>
            {
                float potsModifier;
                int potsType = Main.tile[x, y].type;
                if (potsType == 28)
                {
                    int potsIndex = Main.tile[x, y].frameY / 18;
                    //Divide by 2 to get the right potion sprite and then -1 to get the right index
                    if(potsIndex > 1)
                        potsIndex = (int) (Math.Ceiling(potsType / 2f) - 1);
                    //Get the actual pots type
                    potsIndex /= 3;
                    //Then get the coin modifier based on what type it is
                    potsModifier = PotsRegistery.PotsSpecificModifer(potsIndex);
                    money *= potsModifier;
                    return;
                }

                potsModifier = PotsRegistery.PotsSpecificModifer(Main.tile[x, y].type);
                money *= potsModifier;

            };
            modifyAmount += (int x, int y, ref float money) =>
            {
                if (NPC.downedBoss1)
                    money *= 1.1f;

                if (NPC.downedBoss2)
                    money *= 1.1f;

                if (NPC.downedBoss3)
                    money *= 1.1f;

                if (NPC.downedMechBoss1)
                    money *= 1.1f;

                if (NPC.downedMechBoss2)
                    money *= 1.1f;

                if (NPC.downedMechBoss3)
                    money *= 1.1f;

                if (NPC.downedPlantBoss)
                    money *= 1.1f;

                if (NPC.downedQueenBee)
                    money *= 1.1f;

                if (NPC.downedGolemBoss)
                    money *= 1.1f;

                if (NPC.downedPirates)
                    money *= 1.1f;

                if (NPC.downedGoblins)
                    money *= 1.1f;

                if (NPC.downedFrost)
                    money *= 1.1f;
            };
            modifyAmount += (int x, int y, ref float money) => { };
        }

        public override void ExecuteDrop(int x, int y)
        {
            float money = 200 + WorldGen.genRand.Next(-100, 101);
            modifyAmount?.Invoke(x, y, ref money);
            while ((int)money > 0) {
                if (money > 1000000f) {
                    int num18 = (int)(money / 1000000f);
                    if (num18 > 50 && Main.rand.Next(2) == 0)
                        num18 /= Main.rand.Next(3) + 1;

                    if (Main.rand.Next(2) == 0)
                        num18 /= Main.rand.Next(3) + 1;

                    money -= (float)(1000000 * num18);
                    Item.NewItem(x * 16, y * 16, 16, 16, 74, num18);
                    continue;
                }

                if (money > 10000f) {
                    int num19 = (int)(money / 10000f);
                    if (num19 > 50 && Main.rand.Next(2) == 0)
                        num19 /= Main.rand.Next(3) + 1;

                    if (Main.rand.Next(2) == 0)
                        num19 /= Main.rand.Next(3) + 1;

                    money -= (float)(10000 * num19);
                    Item.NewItem(x * 16, y * 16, 16, 16, 73, num19);
                    continue;
                }

                if (money > 100f) {
                    int num20 = (int)(money / 100f);
                    if (num20 > 50 && Main.rand.Next(2) == 0)
                        num20 /= Main.rand.Next(3) + 1;

                    if (Main.rand.Next(2) == 0)
                        num20 /= Main.rand.Next(3) + 1;

                    money -= (float)(100 * num20);
                    Item.NewItem(x * 16, y * 16, 16, 16, 72, num20);
                    continue;
                }

                int num21 = (int)money;
                if (num21 > 50 && Main.rand.Next(2) == 0)
                    num21 /= Main.rand.Next(3) + 1;

                if (Main.rand.Next(2) == 0)
                    num21 /= Main.rand.Next(4) + 1;

                if (num21 < 1)
                    num21 = 1;

                money -= (float)num21;
                Item.NewItem(x * 16, y * 16, 16, 16, 71, num21);
            }

        }
    }
}
