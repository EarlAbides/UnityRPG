using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 10f;

        Health target = null;
        float damage = 0f;

        void Update()
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * collider.height / 2.0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
