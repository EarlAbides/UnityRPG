using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthPoints = -1f;

        ActionScheduler actionScheduler;
        Animator animator;
        BaseStats baseStats;
        bool isDead = false;

        void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
        }

        void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = baseStats.GetStat(Stat.Health);
            }

            baseStats.onLevelUp += LevelUp;
        }

        public int GetPercentHealth()
        {
            return (int)(healthPoints/baseStats.GetStat(Stat.Health) * 100);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetMaxHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetCurrentHealth()
        {
            return healthPoints;
        }

        private void LevelUp()
        {
            healthPoints = baseStats.GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;

            animator.SetTrigger("die");
            actionScheduler.CancelCurrentAction();
            isDead = true;
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.AddExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            Awake();
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}