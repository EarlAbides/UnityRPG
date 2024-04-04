using System;
using System.Collections;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] Progressions progressions = null;

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public int[] levelHealthPoints;
        }

        [Serializable]
        class Progressions
        {
            public ProgressionCharacterClass[] characterProgressions = new ProgressionCharacterClass[]
            {
                new ProgressionCharacterClass
                {
                    characterClass = CharacterClass.Player,
                    levelHealthPoints = new int[] {50, 100, 200, 400, 700}
                },
                new ProgressionCharacterClass
                {
                    characterClass = CharacterClass.Grunt,
                    levelHealthPoints = new int[] {20, 40, 80, 160, 320}
                },
                new ProgressionCharacterClass
                {
                    characterClass = CharacterClass.GruntArcher,
                    levelHealthPoints = new int[] {15, 30, 60, 120, 240}
                },
                new ProgressionCharacterClass
                {
                    characterClass = CharacterClass.GruntCaptain,
                    levelHealthPoints = new int[] {30, 60, 120, 240, 480}
                }
            };
        }
    }
}