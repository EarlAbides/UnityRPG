using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        enum Handedness
        {
            Left, Right
        }

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float damage = 5f;
        [SerializeField] float range = 2f;
        [SerializeField] Handedness handedness = Handedness.Right;
        [SerializeField] Projectile projectile = null;

        public void Spawn(Transform leftHandTransform, Transform rightHandTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform hand = GetHand(leftHandTransform, rightHandTransform);
                Instantiate(equippedPrefab, hand);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform leftHand, Transform rightHand, Health target)
        {
            if (projectile == null)
            {
                Debug.Log("Projectile not set!");
            }

            Projectile projectileInstance = Instantiate(projectile, GetHand(leftHand, rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }

        public float GetRange()
        {
            return range;
        }

        public float GetDamage()
        {
            return damage;
        }

        private Transform GetHand(Transform leftHandTransform, Transform rightHandTransform)
        {
            return (handedness == Handedness.Left) ? leftHandTransform : rightHandTransform;
        }
    }
}