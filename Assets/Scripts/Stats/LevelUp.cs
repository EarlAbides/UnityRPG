using UnityEngine;

namespace RPG.Stats
{
    public class LevelUp : MonoBehaviour
    {
        [SerializeField] GameObject levelUpEffect = null;

        private void Start()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            baseStats.onLevelUp += DoLevelUp;
        }

        private void DoLevelUp()
        {
            if (levelUpEffect != null)
            {
                Instantiate(levelUpEffect, transform);
            }
            
        }
    }
}