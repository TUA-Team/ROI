using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityCore.API.Pots;
using InfinityCore.API.Pots.DropTable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ObjectData;

namespace ROI.Tiles.Wasteland
{
    class Wasteland_Pots_Big : ModPots
    {
        public override int width => 2;

        public override int height => 2;

        public override int amountOfHorizontalStyle => 4;

        public override void SetExtraDefaults(ref TileObjectData data)
        {
            data.CopyFrom(TileObjectData.Style2x2);
            data.LavaDeath = true;
            data.Width = 2;
            data.Height = 2;
            data.CoordinateHeights = new[] {16, 18};
        }

        public override void RegisterPotsDrop(List<PotsDrop> dropList)
        {
            dropList.Clear();
            dropList.Add(new PortalCoinPotsDrop(200));
            dropList.Add(new ItemsPotsDrop("WastelandStone", new []{mod.ItemType("Uranium_Chunk"), mod.ItemType("Wasteland_Ore")}, new []{ 1, 10 }, (i, i1) => Main.rand.Next(70) == 0));
            dropList.Add(new PotsDropListPotsDrop("default", (i, i1) => true, 
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
                        if ((double)j < Main.rockLayer && Terraria.WorldGen.genRand.Next(2) == 0)
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
    }
}
