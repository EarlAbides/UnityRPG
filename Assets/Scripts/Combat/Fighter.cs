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
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;

        Animator animator;
        ActionScheduler actionScheduler;
        Mover mover;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();

            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            HandleAttack();
        }

        public bool CanAttack(GameObject attackTarget)
        {
            if (attackTarget == null) return false;

            Health targetToTest = attackTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject attackTarget)
        {
            actionScheduler.StartAction(this);
            target = attackTarget.GetComponent<Health>();
        }

        private void HandleAttack()
        {
            if (target == null || target.IsDead()) return;

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            if (!isInRange)
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            // The animator will trigger Hit()
            TriggerAttack();

            timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("abortAttack");
            animator.SetTrigger("attack");
        }

        public void SpawnWeapon()
        {
            if (weaponPrefab != null && handTransform != null)
            {
                Instantiate(weaponPrefab, handTransform);
            }
        }

        public void Cancel()
        {
            StopAttack();
            mover.Cancel();
            target = null;
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("abortAttack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }
    }
}