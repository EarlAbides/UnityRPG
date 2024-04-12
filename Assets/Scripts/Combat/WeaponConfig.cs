using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        const string weaponName = "Weapon";

        enum Handedness
        {
            Left, Right
        }

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float damage = 5f;
        [SerializeField] float percentBonus = 0f;
        [SerializeField] float range = 2f;
        [SerializeField] Handedness handedness = Handedness.Right;
        [SerializeField] Projectile projectile = null;

        public Weapon Spawn(Transform leftHandTransform, Transform rightHandTransform, Animator animator)
        {
            Weapon weapon = null;

            DestroyOldWeapon(leftHandTransform, rightHandTransform);

            if (equippedPrefab != null)
            {
                Transform hand = GetHand(leftHandTransform, rightHandTransform);
                weapon = Instantiate(equippedPrefab, hand);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target, GameObject instigator, float calculatedDamage)
        {
            if (projectile == null)
            {
                Debug.Log("Projectile not set!");
            }

            Projectile projectileInstance = Instantiate(projectile, GetHand(leftHand, rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetRange()
        {
            return range;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetPercentBonus()
        {
            return percentBonus;
        }

        private Transform GetHand(Transform leftHandTransform, Transform rightHandTransform)
        {
            return (handedness == Handedness.Left) ? leftHandTransform : rightHandTransform;
        }

        private void DestroyOldWeapon(Transform leftHandTransform, Transform rightHandTransform)
        {
            Transform oldWeapon = leftHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = rightHandTransform.Find(weaponName);
            }
            if (oldWeapon == null) return;

            // Rename before destroy because of weird
            // unity timing edge case
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}