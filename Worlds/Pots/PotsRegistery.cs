using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Worlds.Pots.DropTable;
using Terraria;
using Terraria.ID;

namespace ROI.Worlds.Pots
{
    static class PotsRegistery
    {
        /// <summary>
        /// key = Tiles<br />
        /// Value = Pots style
        /// </summary>
        private static Dictionary<int, int[]> vanillaPotsStyle = new Dictionary<int, int[]>();
        private static Dictionary<int, ModPots[]> moddedPotsStyle = new Dictionary<int, ModPots[]>();


        private static Dictionary<int, List<PotsDrop>> dropTable = new Dictionary<int, List<PotsDrop>>();

        static PotsRegistery()
        {
            RegisterVanillaDrop();
        }

        public static void RegisterPotsDrop(int potsType, PotsDrop drop)
        {
            if (!dropTable.ContainsKey(potsType))
            {
                dropTable.Add(potsType, new List<PotsDrop>() { drop });
                return;
            }
            dropTable[potsType].Add(drop);
        }

        public static void RegisterVanillaDrop()
        {
            RegisterDefaultsVanillaPots(PotsTypeID.Surface);
            RegisterDefaultsVanillaPots(PotsTypeID.Ice);
            RegisterDefaultsVanillaPots(PotsTypeID.Jungle);
            RegisterDefaultsVanillaPots(PotsTypeID.Dungeon);
            RegisterDefaultsVanillaPots(PotsTypeID.Hell);
            RegisterDefaultsVanillaPots(PotsTypeID.Corrupt);
            RegisterDefaultsVanillaPots(PotsTypeID.SpiderCave);
            RegisterDefaultsVanillaPots(PotsTypeID.Crimson);
            RegisterDefaultsVanillaPots(PotsTypeID.Pyramid);
            RegisterDefaultsVanillaPots(PotsTypeID.Lihzahrd);
            RegisterDefaultsVanillaPots(PotsTypeID.Marble);
        }

        public static void RegisterDefaultsVanillaPots(PotsTypeID type)
        {
            RegisterDefaultsVanillaPots((int)type);
        }

        public static void RegisterDefaultsVanillaPots(int type)
        {
            float potsModifier = PotsSpecificModifer(type);
            int maxValue = (int)(250f / ((potsModifier + 1f) / 2f));
            RegisterPotsDrop(type, new PortalCoinPotsDrop(maxValue));
            if (type == (int)PotsTypeID.Dungeon)
            {
                RegisterPotsDrop(type, new ItemPotsDrop("GoldenKey", ItemID.GoldenKey, 1, (int x, int y) => WorldGen.genRand.Next(40) == 0 && (double)y > Main.worldSurface));
            }
            RegisterPotsDrop(type, new LayerBasedItemsPotsDrop("Potion",
                //TO DO: Convert all these to item ID
                new List<int[]>()
                {
                    new int[] {292, 298, 299, 290, 2322, 2324, 2325, 2350, 2350, 2350},
                    new int[] {289, 290, 291, 298, 299, 303, 304, 2322, 2329, 2350, 2350},
                    new int[] {295, 296, 297, 299, 301, 302, 303, 304, 305, 2322, 2323, 2327, 2329, 2350, 2350},
                    new int[] {288, 288, 293, 294, 295, 296, 297, 300, 301, 302, 304, 305, 2323, 2326},
                },
                new List<int[]>()
                {
                    new int[] {1, 1, 1, 1, 1, 1, 1, 3, 3, 3},
                    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3},
                    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3},
                    new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                },
                (i, i1) => WorldGen.genRand.Next(45) == 0 || (Main.rand.Next(45) == 0 && Main.expertMode)));
            RegisterPotsDrop(type, new ItemPotsDrop("WormholePotion", ItemID.WormholePotion, 1, (i, i1) => Main.netMode == 2 && Main.rand.Next(30) == 0));
            RegisterPotsDrop(type, new PotsDropListPotsDrop("default", (i, i1) => true, 
                new List<PotsDrop>()
                {
                    new SpecialExecutePotsDrop("Heart", (i, j) => true, (i, j) =>
                    {
                        if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLife < Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statLifeMax2) {
                            Item.NewItem(i * 16, j * 16, 16, 16, 58);
                            if (Main.rand.Next(2) == 0)
                                Item.NewItem(i * 16, j * 16, 16, 16, 58);

                            if (Main.expertMode) {
                                if (Main.rand.Next(2) == 0)
                                    Item.NewItem(i * 16, j * 16, 16, 16, 58);

                                if (Main.rand.Next(2) == 0)
                                    Item.NewItem(i * 16, j * 16, 16, 16, 58);
                            }
                        }
                    }),
                    new SpecialExecutePotsDrop("Mana", (i, j) => true, (i, j) =>
                    {
                        if (Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statMana < Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].statManaMax2) {
                            Item.NewItem(i * 16, j * 16, 16, 16, 184);
                        }
                    }),
                    new SpecialExecutePotsDrop("Torch", (i, j) => true, (i, j) =>
                    {
                        int torchAmount = Main.rand.Next(2, 6);
                        if (Main.expertMode)
                            torchAmount += Main.rand.Next(1, 7);

                        if (Main.tile[i, j].liquid > 0)
                            Item.NewItem(i * 16, j * 16, 16, 16, 282, torchAmount);
                        else
                            Item.NewItem(i * 16, j * 16, 16, 16, 8, torchAmount);
                    }),
                    new SpecialExecutePotsDrop("Ammo", (i, j) => true, (i, j) =>
                    {
                        int stack = Main.rand.Next(10, 21);
                        int type2 = 40;
                        if ((double)j < Main.rockLayer && WorldGen.genRand.Next(2) == 0)
                            type2 = ((!Main.hardMode) ? 42 : 168);

                        if (j > Main.maxTilesY - 200)
                            type2 = 265;
                        else if (Main.hardMode)
                            type2 = ((Main.rand.Next(2) != 0) ? 47 : 278);

                        Item.NewItem(i * 16, j * 16, 16, 16, type2, stack);
                    }),
                    new SpecialExecutePotsDrop("HealingPotion", (i, j) => true, (i, j) =>
                    {
                        int potionType = 28;
                        if (j > Main.maxTilesY - 200 || Main.hardMode)
                            potionType = 188;

                        int quantity = 1;
                        if (Main.expertMode && Main.rand.Next(3) != 0)
                            quantity++;

                        Item.NewItem(i * 16, j * 16, 16, 16, potionType, quantity);
                    }),
                    new SpecialExecutePotsDrop("Bomb", (i, j) => true, (i, j) =>
                    {
                        int bombsAmount = Main.rand.Next(4) + 1;
                        if (Main.expertMode)
                            bombsAmount += Main.rand.Next(4);

                        Item.NewItem(i * 16, j * 16, 16, 16, 166, bombsAmount);
                    }),
                    new SpecialExecutePotsDrop("RopesOrMoney", (i, j) => true, (i, j) =>
                    {
                        if (j < Main.maxTilesY - 200 && !Main.hardMode) {
                            int stack2 = Main.rand.Next(20, 41);
                            Item.NewItem(i * 16, j * 16, 16, 16, 965, stack2);
                        }
                        else
                        {
                            new MoneyPotsDrop((i1, i2) => true).ExecuteDrop(i, j);
                        }
                    }),
                    new MoneyPotsDrop((i1, i2) => true)
                }));
        }

        /// <summary>
        /// If vanilla pots, here is the order:<br />
        /// [0] = PortalCoin<br />
        /// IF Dungeon Pots <br />
        /// [1] = GoldenKey <br />
        /// Everything after is index + 1 for dungeon pots <br />
        /// [1] = PotionDrop<br />
        /// [2] = Wormhole_MP<br />
        /// [3] = DefaultDrop See note bellow<br />
        ///<br /> <br />
        ///
        /// DefaultDrop info<br />
        /// Have 8 possible drop, which are ran trough a randomizer of 7 (6 if in the underworld or in hardmode)<br />
        /// 0 = Heart drop<br />
        /// 1 = Torch drop, if a pots is a biome pots, it will drop the torch matching it's biome<br />
        /// 2 = Ammo drop<br />
        /// 3 = Healing pots drop<br />
        /// 4 = bomb drop<br />
        /// 5 (underworld and hardmode, otherwise this is 6) =  coin drop<br />
        /// 5 (if not in hardmode or in underworld) = Rope
        /// </summary>
        /// <param name="potsType"></param>
        /// <returns></returns>
        public static List<PotsDrop> ModifyPotsDrop(int potsType)
        {
            return dropTable[potsType];
        }

        public static void ExecuteDrop(int x, int y, int potsType)
        {
            if (WorldGen.destroyObject)
                return;
            bool flag = false;
            int num = 0;
            int num2 = y;
            for (num += Main.tile[x, y].frameX / 18; num > 1; num -= 2) {
            }

            num *= -1;
            num += x;

            int num3 = Main.tile[x, y].frameY / 18;
            int num4 = 0;
            while (num3 > 1) {
                num3 -= 2;
                num4++;
            }

            num2 -= num3;
            for (int k = num; k < num + 2; k++) {
                for (int l = num2; l < num2 + 2; l++) {
                    if (Main.tile[k, l] == null)
                        Main.tile[k, l] = new Tile();

                    int num5;
                    for (num5 = Main.tile[k, l].frameX / 18; num5 > 1; num5 -= 2) {
                    }

                    if (!Main.tile[k, l].active() || Main.tile[k, l].type != 28 || num5 != k - num || Main.tile[k, l].frameY != (l - num2) * 18 + num4 * 36)
                        flag = true;
                }

                if (Main.tile[k, num2 + 2] == null)
                    Main.tile[k, num2 + 2] = new Tile();

                if (!WorldGen.SolidTile2(k, num2 + 2))
                    flag = true;
            }

            if (!flag)
                return;

            for (int m = num; m < num + 2; m++) {
                for (int n = num2; n < num2 + 2; n++) {
                    if (Main.tile[m, n].type == 28 && Main.tile[m, n].active())
                    {
                        Main.tile[m, n].type = 0;
                        Main.tile[m, n].active(false);
                    }
                        
                }
            }

            foreach (PotsDrop potsDrop in dropTable[potsType])
            {
                if (potsDrop.successFunc(x, y))
                {
                    potsDrop.ExecuteDrop(x, y);
                    return;
                }
                WorldGen.destroyObject = true;
            }

            WorldGen.destroyObject = true;
        }

        internal static float PotsSpecificModifer(int potsType)
        {
            switch ((PotsTypeID)potsType)
            {
                case PotsTypeID.Surface:
                    return 1f;
                case PotsTypeID.Ice:
                    return 1.25f;
                case PotsTypeID.Jungle:
                    return 1.75f;
                case PotsTypeID.Dungeon:
                    return 1.9f;
                case PotsTypeID.Hell:
                    return 2.1f;
                case PotsTypeID.Corrupt:
                case PotsTypeID.Crimson:
                    return 1.6f;
                case PotsTypeID.SpiderCave:
                    return 3.5f;
                case PotsTypeID.Pyramid:
                    return 10f;
                case PotsTypeID.Lihzahrd:
                    return (Main.hardMode) ? 4f : 1f;
                case PotsTypeID.Marble:
                    return 2f;
                default:
                    
                    return 1f;
            }
        }
    }
}
