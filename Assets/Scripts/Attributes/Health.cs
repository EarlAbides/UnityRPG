using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent takeDamage;
        LazyValue<float> healthPoints;

        ActionScheduler actionScheduler;
        Animator animator;
        BaseStats baseStats;
        bool isDead = false;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += LevelUp;
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= LevelUp;
        }

        public int GetPercentHealth()
        {
            return (int)(healthPoints.value / baseStats.GetStat(Stat.Health) * 100);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            float totalDamage = damage;

            healthPoints.value = Mathf.Max(healthPoints.value - totalDamage, 0);
            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke();
            }
        }

        public float GetMaxHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetCurrentHealth()
        {
            return healthPoints.value;
        }

        private void LevelUp()
        {
            healthPoints.value = baseStats.GetStat(Stat.Health);
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}