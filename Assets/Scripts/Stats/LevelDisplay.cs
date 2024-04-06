using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {

        BaseStats baseStats = null;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        
        void Update()
        {
            Text text = GetComponent<Text>();
            text.text = String.Format("{0}", baseStats.GetLevel()); 
        }
    }
}
