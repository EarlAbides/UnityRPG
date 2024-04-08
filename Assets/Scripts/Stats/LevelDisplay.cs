using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {

        BaseStats baseStats = null;
        Text text;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            text = GetComponent<Text>();
        }
        
        void Update()
        {
            text.text = String.Format(" {0}", baseStats.GetLevel()); 
        }
    }
}
