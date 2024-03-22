using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;

        Animator animator;
        ActionScheduler actionScheduler;
        Mover mover;

        Health target;
        float timeSinceLastAttack = 0;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            HandleAttack();
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        private void HandleAttack()
        {
            if (target != null)
            {
                if (target.IsDead()) return;

                bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
                if (!isInRange)
                {
                    mover.MoveTo(target.transform.position);
                }
                else
                {
                    mover.Cancel();
                    AttackBehavior();
                }
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            // The animator will trigger Hit()
            animator.SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }

        public void Cancel()
        {
            animator.ResetTrigger("abortAttack");
            animator.SetTrigger("abortAttack");
            target = null;
        }

        // Animation Event
        void Hit()
        {
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
    }
}