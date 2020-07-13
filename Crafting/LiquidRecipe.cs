using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL.Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Crafting
{
    public class LiquidRecipe
    {
        public Item[] ingredients = new Item[100];

        public Func<bool> condition = null;

        public int liquidType = 0;

        public Item result = new Item();

        public Mod mod { get; private set; }

        public static int nextIngredientIndex = 0;


        public LiquidRecipe(Mod mod)
        {
            this.mod = mod;
            nextIngredientIndex = 0;
            for (int i = 0; i < 100; i++)
            {
                ingredients[i] = new Item();
            }
        }

        public void AddIngredient(Item item)
        {
            ingredients[nextIngredientIndex].SetDefaults(item.type);
            ingredients[nextIngredientIndex].stack = item.stack;
            nextIngredientIndex++;
        }

        public void AddIngredient(int item, int stack)
        {
            ingredients[nextIngredientIndex].SetDefaults(item);
            ingredients[nextIngredientIndex].stack = stack;
            nextIngredientIndex++;
        }

        public void SetResult(int item, int stack)
        {
            result.SetDefaults(item);
            result.stack = stack;
        }

        public void SetResult(Item item)
        {
            result.SetDefaults(item.type);
            result.stack = item.stack;
        }

        public void SetLiquid(int liquid)
        {
            this.liquidType = liquid;
        }

        public void AddRecipe()
        {
            LiquidCrafting.recipe.Add(this);
        }
    }
}
