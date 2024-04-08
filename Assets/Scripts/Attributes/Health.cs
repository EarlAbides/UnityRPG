using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += LevelUp;
        }

        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = baseStats.GetStat(Stat.Health);
            }
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= LevelUp;
        }

        public int GetPercentHealth()
        {
            return (int)(healthPoints/baseStats.GetStat(Stat.Health) * 100);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            float totalDamage = damage;
            print(gameObject.name + " took damage: " + totalDamage);

            healthPoints = Mathf.Max(healthPoints - totalDamage, 0);
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
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}