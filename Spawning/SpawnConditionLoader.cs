using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace ROI.Spawning
{
    public static class SpawnConditionLoader
    {
        private static Dictionary<Type, SpawnCondition> _spawnConditionsByType;


        #region Loading

        internal static void Load()
        {
            _spawnConditionsByType = new Dictionary<Type, SpawnCondition>();

            foreach (Mod mod in ModLoader.Mods)
                if (mod.Code != null)
                    foreach (TypeInfo spawnConditionType in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(SpawnCondition))))
                    {
                        SpawnCondition spawnCondition = Activator.CreateInstance(spawnConditionType) as SpawnCondition;

                        if (spawnCondition == null)
                        {
                            ROIMod.Log.Error($"Error while loading {nameof(SpawnCondition)} {spawnConditionType.FullName}.");
                            continue;
                        }

                        _spawnConditionsByType.Add(spawnConditionType, spawnCondition);
                        spawnCondition.Mod = mod;
                    }
        }

        internal static void Unload()
        {
            _spawnConditionsByType.Clear();
            _spawnConditionsByType = null;
        }

        #endregion


        public static SpawnCondition GetSpawnCondition<T>() where T : SpawnCondition => GetSpawnCondition(typeof(T));

        public static SpawnCondition GetSpawnCondition(Type type)
        {
            if (_spawnConditionsByType.ContainsKey(type))
                return null;

            return _spawnConditionsByType[type];
        }


        public static int SpawnConditionCount => _spawnConditionsByType.Count;
    }
}