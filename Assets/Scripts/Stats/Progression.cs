using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            return characterClasses.Where(cc => cc.characterClass == characterClass)
                    .First()
                    .GetCharacterLevel(level)
                    .GetStat(stat);
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public CharacterLevel[] levels;

            public CharacterLevel GetCharacterLevel(int level)
            {
                // Levels are 0 base, passed in level is 1 base
                return (level <= levels.Length) ? levels[level - 1] : levels[levels.Length - 1];
            }
        }

        [Serializable]
        class CharacterLevel
        {
            public float healthPoints;
            public float experienceReward;

            private Dictionary<Stat, float> lookupTable = null; 

            public float GetStat(Stat stat)
            {
                if (lookupTable == null) BuildLookup();
                if (!lookupTable.ContainsKey(stat)) return 0f;
                return lookupTable[stat];
            }

            private void BuildLookup()
            {
                if (lookupTable != null) return;

                lookupTable = new Dictionary<Stat, float>
                {
                    { Stat.Health, healthPoints },
                    { Stat.ExperienceReward, experienceReward}
                };
            }
        }
    }
}