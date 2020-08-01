using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    internal class ROIModSupport
    {
        internal static void Load()
        {
            //CalamityModHook.Load();
        }

        //I could make an actual injection dedicated class, but I'm lazy
        internal class CalamityModHook
        {
            public delegate void orig_ChasmGenerator(int i, int j, int steps, bool ocean = false);

            public delegate void hook_ChasmGenerator(orig_ChasmGenerator orig, int i, int j, int steps, bool ocean = false);

            public static event hook_ChasmGenerator ChasmGenerator_Hook
            {
                add
                {
                    if (ModLoader.Mods.Any(i => i.Name == "CalamityMod"))
                    {
                        Type type = ReflectionUtilities.GetType(ModLoader.GetMod("CalamityMod").Code, "WorldGenerationMethods");
                        if (type != null)
                        {
                            HookEndpointManager.Add<hook_ChasmGenerator>(type.GetMethod("ChasmGenerator", BindingFlags.Static | BindingFlags.Public), value);
                        }
                    }
                }
                remove
                {
                    if (ModLoader.Mods.Any(i => i.Name == "CalamityMod"))
                    {
                        Type type = ReflectionUtilities.GetType(ModLoader.GetMod("CalamityMod").Code, "WorldGenerationMethods", typeof(ModWorld));
                        if (type != null)
                        {
                            HookEndpointManager.Remove<hook_ChasmGenerator>(type.GetMethod("ChasmGenerator", BindingFlags.Static | BindingFlags.Public), value);
                        }
                    }
                }
            }

            //For reference only
            internal static event ILContext.Manipulator CalamityMod_UnderworldIsland
            {
                add
                {
                    if (ModLoader.Mods.Any(i => i.Name == "CalamityMod"))
                    {
                        Type type = ReflectionUtilities.GetType(ModLoader.GetMod("CalamityMod").Code, "WorldGenerationMethods", typeof(ModWorld));
                        if (type != null)
                        {
                            HookEndpointManager.Modify<hook_ChasmGenerator>(type.GetMethod("ChasmGenerator", BindingFlags.Static | BindingFlags.Public), value);
                        }
                    }
                }
                remove
                {
                    if (ModLoader.Mods.Any(i => i.Name == "CalamityMod"))
                    {
                        Type type = ReflectionUtilities.GetType(ModLoader.GetMod("CalamityMod").Code, "WorldGenerationMethods", typeof(ModWorld));
                        if (type != null)
                        {
                            HookEndpointManager.Unmodify<hook_ChasmGenerator>(type.GetMethod("ChasmGenerator", BindingFlags.Static | BindingFlags.Public), value);
                        }
                    }
                }
            }

            internal static void Load()
            {
                ChasmGenerator_Hook += NewChasmGenerator;
            }

            internal static void NewChasmGenerator(orig_ChasmGenerator orig, int i, int j, int steps, bool ocean = false)
            {
                if (WorldGen.crimson)
                    return;
                orig(i, j, steps, ocean);
            }
        }

        internal static class ReflectionUtilities
        {
            public static bool TypeExist(Assembly assembly, Type type, Type extendedType = null)
            {
                if (extendedType == null)
                    return assembly.GetTypes().Any(i => i.Name == type.Name);
                return assembly.GetTypes().Any(i => i.Name == type.Name && i.IsInstanceOfType(extendedType));
            }

            public static bool TypeExist(Assembly assembly, string type, Type extendedType = null)
            {
                if (extendedType == null)
                    return assembly.GetTypes().Any(i => i.Name == type);
                return assembly.GetTypes().Any(i => i.Name == type && i.IsInstanceOfType(extendedType));
            }


            public static Type GetType(Assembly assembly, Type type, Type extendedType = null)
            {
                try
                {
                    if (TypeExist(assembly, type, extendedType))
                    {
                        return assembly.GetTypes().Single(i => i.Name == type.Name && i.IsInstanceOfType(extendedType));
                    }
                }
                catch (Exception e)
                {
                    Main.statusText = $"More than one {type.Name} was found in {assembly.FullName} assembly, pls make sure you have the right extend";
                }

                return null;
            }

            public static Type GetType(Assembly assembly, string type, Type extendedType = null)
            {
                try
                {
                    if (!TypeExist(assembly, type, extendedType))
                    {
                        return null;
                    }
                    if (extendedType == null)
                    {
                        return assembly.GetTypes().Single(i => i.Name == type);
                    }
                    return assembly.GetTypes().Single(i => i.Name == type && i.IsInstanceOfType(extendedType));

                }
                catch (Exception e)
                {
                    Main.statusText = $"More than one {type} was found in {assembly.FullName} assembly, pls make sure you have the right extend";
                }

                return null;
            }
        }
    }
}
