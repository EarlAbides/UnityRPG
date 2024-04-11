using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        void Start()
        {
            // Spawn(10);
        }

        public void Spawn(float damage)
        {
            print("Gonna spawn");

            print("Spawining");
            DamageText damageText = Instantiate<DamageText>(damageTextPrefab, transform);
            print("Spawned");
        }
    }
}