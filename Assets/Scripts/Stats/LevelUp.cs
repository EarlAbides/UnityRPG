using UnityEngine;

namespace RPG.Stats
{
    public class LevelUp : MonoBehaviour
    {
        [SerializeField] GameObject levelUpEffect = null;

        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += DoLevelUp;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= DoLevelUp;
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