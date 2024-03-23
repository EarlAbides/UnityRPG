using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        ActionScheduler actionScheduler;
        Animator animator;
        bool isDead = false;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            animator.SetTrigger("die");
            actionScheduler.CancelCurrentAction();
            isDead = true;
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}