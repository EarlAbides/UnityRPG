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

        public void Spawn(Transform leftHandTransform, Transform rightHandTransform, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform hand = (handedness == Handedness.Left) ? leftHandTransform : rightHandTransform;
                Instantiate(equippedPrefab, hand);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetRange()
        {
            return range;
        }

        public float GetDamage()
        {
            return damage;
        }
    }
}