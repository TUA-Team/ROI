using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using SpawnCondition = ROI.Models.Spawning.SpawnCondition;

namespace ROI.Loaders
{
    // web's code
    public sealed class SpawnConditionLoader : BaseLoader
    {
        private Dictionary<Type, SpawnCondition> _spawnConditionsByType;


        public override void Initialize(Mod mod)
        {
            base.Initialize(mod);

            _spawnConditionsByType = new Dictionary<Type, SpawnCondition>();

            foreach (TypeInfo spawnConditionType in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(SpawnCondition))))
            {
                if (!(Activator.CreateInstance(spawnConditionType) is SpawnCondition spawnCondition))
                {
                    mod.Logger.Error($"Error while loading {nameof(SpawnCondition)} {spawnConditionType.FullName}.");
                    continue;
                }

                _spawnConditionsByType.Add(spawnConditionType, spawnCondition);
                spawnCondition.Mod = mod;
            }
        }

        public override void Unload()
        {
            _spawnConditionsByType?.Clear();
            _spawnConditionsByType = null;
        }


        public SpawnCondition GetSpawnCondition<T>() where T : SpawnCondition => GetSpawnCondition(typeof(T));

        public SpawnCondition GetSpawnCondition(Type type)
        {
            if (_spawnConditionsByType.ContainsKey(type))
                return null;

            return _spawnConditionsByType[type];
        }


        public int SpawnConditionCount => _spawnConditionsByType.Count;
    }
}