using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, (characterClass == CharacterClass.Player) ? GetLevel() : startingLevel);
        }

        public int GetLevel()
        {
            return progression.GetLevel(GetComponent<Experience>().GetExperience());
        }
    }
}
