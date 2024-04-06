using UnityEngine;
using System.Linq;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        int currentLevel = 0;

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

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("Levelled up!");
            }
        }

        private  int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            int calculatedLevel = 1;

            Progression.ProgressionCharacterClass player = progression.GetCharacterClasses().Where(cc => cc.characterClass == CharacterClass.Player).First();
            for (int level = 1; level < player.levels.Length; level++)
            {
                if (experience.GetExperience() >= player.levels[level].levelExperience)
                {
                    calculatedLevel++;
                }
                else break;
            }            

            return calculatedLevel;
        }


    }
}
