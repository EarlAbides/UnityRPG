using System;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            return characterClasses.Where(cc => cc.characterClass == characterClass)
                    .First()
                    .levels[level - 1] // zero based index
                    .healthPoints;
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public CharacterLevel[] levels;
        }

        [Serializable]
        class CharacterLevel
        {
            public float healthPoints;
        }
    }
}