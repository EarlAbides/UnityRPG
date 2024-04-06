using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience Experience;
        

        private void Awake()
        {
            Experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        
        void Update()
        {
            Text text = GetComponent<Text>();
            text.text = String.Format("{0}", Experience.GetExperience()); 
        }
    }
}
