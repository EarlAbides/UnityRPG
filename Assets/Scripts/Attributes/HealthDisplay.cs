using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        
        void Update()
        {
            Text text = GetComponent<Text>();
            text.text = String.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth()); 
        }
    }
}
