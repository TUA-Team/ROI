using LiquidAPI;
using LiquidAPI.LiquidMod;
using Microsoft.Xna.Framework;
using ROI.Items.Placeables.Wasteland;
using ROI.Items.Summons;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Crafting
{
    class LiquidCrafting
    {
        public static List<LiquidRecipe> recipe = new List<LiquidRecipe>();

        internal static void Load()
        {

        }

        internal static void Unload()
        {

        }

        internal static void PostLoad()
        {
            LiquidRecipe recipe = new LiquidRecipe(ROIMod.instance);
            recipe.AddIngredient(ItemID.DirtBlock, 100);
            recipe.SetLiquid(LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste").Type);
            recipe.SetResult(ModContent.ItemType<Wasteland_Dirt>(), 10);
            recipe.AddRecipe();
            recipe = new LiquidRecipe(ROIMod.instance);
            recipe.AddIngredient(ItemID.NightsEdge, 1);
            recipe.AddIngredient(ItemID.Excalibur, 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 3);
            recipe.SetLiquid(LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste").Type);
            recipe.SetResult(ItemID.TerraBlade, 1);
            recipe.AddRecipe();
            recipe = new LiquidRecipe(ROIMod.instance);
            recipe.AddIngredient(ItemID.TissueSample, 15);
            recipe.AddIngredient(ItemID.ViciousMushroom, 5);
            recipe.AddIngredient(ItemID.CrimsonSeeds, 2);
            recipe.AddIngredient(ItemID.Vertebrae, 30);
            recipe.SetLiquid(LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste").Type);
            recipe.SetResult(ModContent.ItemType<ClumpOfRadioactiveMeat>(), 1);
            recipe.AddRecipe();
        }

        public static bool RecipeMatch(Item[] ingredient)
        {
            Dictionary<int, int> ingredientDictionary = new Dictionary<int, int>();
            for (int i = 0; i < recipe.Count && recipe[i].result.type != 0; i++)
            {
                foreach (Item item in ingredient)
                {
                    Point16 itemPositionInWorld = ((item.BottomLeft - new Vector2(0, 2)) / 16).ToPoint16();
                    Tile tile = Main.tile[itemPositionInWorld.X, itemPositionInWorld.Y];

                    if (tile.liquid > 100)
                    {
                        LiquidRef liquidRef = LiquidWorld.grid[itemPositionInWorld.X, itemPositionInWorld.Y];
                        if (liquidRef.TypeID == recipe[i].liquidType)
                        {
                            Item[] items = Main.item.Where(index => ((index.BottomLeft - new Vector2(0, 2))  / 16).ToPoint16() == itemPositionInWorld).ToArray();
                            foreach (var confusion in items)
                            {
                                if (!ingredientDictionary.ContainsKey(item.type))
                                    ingredientDictionary.Add(item.type, item.stack);
                            }

                            bool isRecipeAvailable = true;
                            for (int j = 0; j < recipe[i].ingredients.Length && recipe[i].ingredients[j].type != 0; j++)
                            {
                                //I'm lost
                                Item item2 = recipe[i].ingredients[j];
                                if (item2.type == 0)
                                    break;

                                int numberOfItemRequiredLeft = item2.stack;

                                if (ingredientDictionary.ContainsKey(item2.type))
                                {
                                    numberOfItemRequiredLeft -= ingredientDictionary[item2.type];
                                }

                                if (numberOfItemRequiredLeft > 0)
                                {
                                    isRecipeAvailable = false;
                                    break;
                                }
                            }

                            if (isRecipeAvailable)
                            {
                                foreach (Item item1 in items)
                                {
                                    for (int j = 0; j < recipe[i].ingredients.Length && recipe[i].ingredients[j].type != 0; j++)
                                    {
                                        //I'm lost
                                        Item item2 = recipe[i].ingredients[j];
                                        if (item2.type != item1.type)
                                        {
                                            continue;
                                        }

                                        item1.stack -= item2.stack;
                                        if (item1.stack <= 0)
                                        {
                                            item1.TurnToAir();
                                        }
                                    }
                                }

                                Item.NewItem(item.position, recipe[i].result.type, recipe[i].result.stack);
                                break;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
