using System;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetDamageText(float value)
        {
            TextMeshProUGUI damageText = GetComponentInChildren<TextMeshProUGUI>();
            damageText.text = String.Format("{0:0}", value);
        }
    }
}