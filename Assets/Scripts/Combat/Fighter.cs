using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Animator animator;
        ActionScheduler actionScheduler;
        Mover mover;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            HandleAttack();
        }

        public Health GetTarget()
        {
            return target;
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

        public void Cancel()
        {
            StopAttack();
            mover.Cancel();
            target = null;
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(leftHandTransform, rightHandTransform, animator);
        }

        private void HandleAttack()
        {
            if (target == null || target.IsDead()) return;

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("abortAttack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                // Launch projectile
                currentWeapon.LaunchProjectile(leftHandTransform, rightHandTransform, target, gameObject, damage);
            }
            else
            {
                // Regular attack
                // target.TakeDamage(gameObject, currentWeapon.GetDamage());
                target.TakeDamage(gameObject, damage);
            }
        }

        // Another animation event from bow animation
        void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = Resources.Load<Weapon>((string)state);
            if (weapon != null)
            {
                EquipWeapon(weapon);
            }
        }
    }
}