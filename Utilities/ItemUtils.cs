﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Utilities
{
    public static class ItemUtils
    {
        public const int
            ARMOR_SLOTS_COUNT = 3,
            ACCESSORY_SLOTS_COUNT = 5,
            MAX_EXTRA_ACCESSORY_SLOTS = 2,

            SOCIAL_ARMOR_START_INDEX = ARMOR_SLOTS_COUNT + ACCESSORY_SLOTS_COUNT + MAX_EXTRA_ACCESSORY_SLOTS;

        public static List<T> GetItemsByType<T>(this Player player,
            bool inventory = false,
            bool armor = false,
            bool armorSocial = false,
            bool accessories = false,
            bool accessoriesSocial = false) where T : ModItem
        {
            List<T> filteredItems = new List<T>();

            if (inventory)
                SearchItems(ref filteredItems, player.inventory);

            if (armor) // TODO Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, 0, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots);
            }

            if (accessories)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots);
            }

            if (armorSocial) // TODO: Verify this works.
            {
                Item[] armorSlots = new Item[ARMOR_SLOTS_COUNT];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX, armorSlots, 0, armorSlots.Length);

                SearchItems(ref filteredItems, armorSlots);
            }

            if (accessoriesSocial)
            {
                Item[] accessorySlots = new Item[ACCESSORY_SLOTS_COUNT + player.extraAccessorySlots];
                Array.Copy(player.armor, SOCIAL_ARMOR_START_INDEX + ARMOR_SLOTS_COUNT, accessorySlots, 0, accessorySlots.Length);

                SearchItems(ref filteredItems, accessorySlots);
            }

            return filteredItems;
        }

        private static void SearchItems<T>(ref List<T> filtered, IEnumerable<Item> items) where T : ModItem
        {
            foreach (Item item in items)
                if (item != null && item.ModItem != null && item.IsOfType<T>())
                    filtered.Add(item.ModItem as T);
        }

        public static bool IsOfType<T>(this Item item, out T mod) where T : ModItem => (mod = item as T) != null;

        public static bool IsOfType<T>(this Item item) where T : ModItem => item.ModItem is T;
    }
}