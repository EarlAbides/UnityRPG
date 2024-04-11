using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damage)
        {
            DamageText damageText = Instantiate<DamageText>(damageTextPrefab, transform);
            damageText.SetDamageText(damage);
        }
    }
}