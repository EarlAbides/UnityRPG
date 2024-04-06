using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        ActionScheduler actionScheduler;
        Animator animator;
        bool isDead = false;
        float startingHealth = 100f;

        void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
            startingHealth = healthPoints;
        }

        public int GetPercentHealth()
        {
            return (int)(healthPoints/startingHealth * 100);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
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

            experience.AddExperience(GetComponent<BaseStats>().GetExperienceAward());
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