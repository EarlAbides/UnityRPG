using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent hitEvent = null;

        public void OnHit()
        {
            hitEvent.Invoke();
        }
    }
}
