using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 10f;

        Health target = null;

        void Update()
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target)
        {
            this.target = target;
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
    }
}
