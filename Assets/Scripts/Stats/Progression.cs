using System;
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

            public float GetStat(Stat stat)
            {
                switch (stat)
                {
                    case Stat.Health:
                        return healthPoints;
                    case Stat.ExperienceReward:
                        return experienceReward;
                    default:
                        return 0f;
                }
            }
        }
    }
}