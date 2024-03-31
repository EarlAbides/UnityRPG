using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Transform target = null;
        [SerializeField] float speed = 10f;

        void Update()
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            if (collider == null) 
            {
                return target.position;
            }
            return target.position + Vector3.up * collider.height / 2.0f;
        }
    }
}
