using UnityEngine;
using System.Linq;
using System;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private int currentLevel = 0;

        public event Action onLevelUp;

        void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                CalculateLevel();
            }
            return currentLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                onLevelUp();
            }
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            int calculatedLevel = 1;

            for (int level = 1; level < progression.GetCharacterClassLevelCount(CharacterClass.Player); level++)
            {
                if (experience.GetExperience() >= progression.GetStat(Stat.LevelExperience, CharacterClass.Player, level)) // player.levels[level].levelExperience)
                {
                    calculatedLevel++;
                }
                else break;
            }            

            return calculatedLevel;
        }


    }
}
